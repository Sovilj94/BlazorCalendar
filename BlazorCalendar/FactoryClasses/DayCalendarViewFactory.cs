using BlazorCalendar.Models;
using BlazorCalendar.Models.DayViewModels;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Services;
using BlazorCalendar.Styles;

namespace BlazorCalendar.FactoryClasses
{
    public class DayCalendarViewFactory : ICalendarViewFactory
    {
        private readonly TasksService _tasksService;
        private readonly string[] _dayNames = { "Ned", "Pon", "Uto", "Sre", "Čet", "Pet", "Sub" };

        public DayCalendarViewFactory()
        {
            _tasksService = new TasksService();
        }

        public ICalendarView CreateCalendarView(DateTime today, TimeDivisionEnum timeDivision, List<ICalendarEvent> calendarEvents)
        {
            // Inicijalizacija view modela
            DayCalendarViewModel dayCalendarViewModel = new DayCalendarViewModel();
            dayCalendarViewModel.DayHeaderViewModel = new DayDayHeaderViewModel
            {
                DayName = _dayNames[(int)today.DayOfWeek],
                DayDate = today.ToString("dd.MM")
            };

            dayCalendarViewModel.AllDayViewModel = new DayAllDayViewModel
            {
                Events = _tasksService.GetTasksForAllDayViewModel(_tasksService.GetAllTasks()),
                FirstDateWeek = today.Date,
                TimeCellViewModel = new DayTimeCellViewModel(),
                GridItemsViewModel = new List<DayGridItemViewModel>()
            };
            dayCalendarViewModel.AllDayViewModel.GridItemsViewModel = GetGridItemsForAllDayComponent(dayCalendarViewModel.AllDayViewModel.Events, today.Date);

            // TimeCells for AllDayViewModel
            DayTimeCellViewModel allDayTimeCell = new DayTimeCellViewModel
            {
                IsAllDayTimesCell = true,
                Time = today,
                Column = 1,
                CSSGridPosition = $";grid-column-start:1; height:100px; border-right:1px solid #ccc"
            };
            dayCalendarViewModel.AllDayViewModel.TimeCellViewModel = allDayTimeCell;

            // TimeSideBar
            dayCalendarViewModel.TimeSideBarViewModel = new DayTimeSideBarViewModel
            {
                TimeDivision = new TimeDivision(timeDivision)
            };

            // Main Day View
            dayCalendarViewModel.DayViewModel = new DayDayViewModel
            {
                Day = today,
                TimeDivision = new TimeDivision(timeDivision),
                GridItemsViewModel = GetGridItemsForDayComponent(_tasksService.GetTasksForDayViewModel(today, _tasksService.GetAllTasks()), new TimeDivision(timeDivision).Minutes, today),
                TimeCellsViewModel = new List<DayTimeCellViewModel>(),
            };

            if (dayCalendarViewModel.DayViewModel.GridItemsViewModel != null && dayCalendarViewModel.DayViewModel.GridItemsViewModel.Count != 0)
            {
                dayCalendarViewModel.DayViewModel.MaxNumberOfColumns = dayCalendarViewModel.DayViewModel.GridItemsViewModel.Max(x => x.ColumnStart);
            }
            else
            {
                dayCalendarViewModel.DayViewModel.MaxNumberOfColumns = 1;
            }

            dayCalendarViewModel.AllDayViewModel.GridItemListViewModel = new DayGridItemListViewModel();
            dayCalendarViewModel.AllDayViewModel.GridItemListViewModel.GridItemsViewModel = new List<DayGridItemViewModel>();
            dayCalendarViewModel.AllDayViewModel.GridItemListViewModel.GridItemsViewModel = dayCalendarViewModel.AllDayViewModel.GridItemsViewModel;

            for (int dayTime = 0; dayTime < dayCalendarViewModel.DayViewModel.TimeDivision.NumberOfCells; dayTime++)
            {
                DateTime time = dayCalendarViewModel.DayViewModel.Day.AddMinutes(dayTime * dayCalendarViewModel.DayViewModel.TimeDivision.Minutes);
                int row = dayTime + 1;

                DayTimeCellViewModel timeCell = new DayTimeCellViewModel
                {
                    Row = row,
                    ColumnsSpan = dayCalendarViewModel.DayViewModel.MaxNumberOfColumns,
                    CSSGridPosition = $"grid-row:{row}; grid-column:1 / span {dayCalendarViewModel.DayViewModel.MaxNumberOfColumns};",
                    Time = time,
                    CSSbackground = "" //GetBackground(dayCalendarViewModel.DayViewModel.Day, dayCalendarViewModel.DayViewModel)
                };

                dayCalendarViewModel.DayViewModel.TimeCellsViewModel.Add(timeCell);
            }

            dayCalendarViewModel.Events = _tasksService.GetAllTasks().Where(e => e.DateStart.Date == today.Date).ToList();

            return dayCalendarViewModel;
        }

        public List<DayGridItemViewModel> GetGridItemsForDayComponent(List<ICalendarEvent> events, int minutes, DateTime day)
        {
            try
            {
                List<DayGridItemViewModel> gridItems = new List<DayGridItemViewModel>();

                // Initialize a dictionary to keep track of occupied time slots and columns
                Dictionary<int, HashSet<int>> occupiedSlots = new Dictionary<int, HashSet<int>>();

                // Order events by start time, then by end time
                events = events.Where(x => x.DateStart.TimeOfDay != TimeSpan.Zero || x.DateEnd.TimeOfDay != TimeSpan.Zero)
                               .OrderBy(x => x.DateStart)
                               .ThenBy(x => x.DateEnd)
                               .ToList();

                foreach (var currentEvent in events)
                {
                    if (currentEvent.DateStart.Date <= day.Date && currentEvent.DateEnd.Date >= day.Date)
                    {
                        DayGridItemViewModel gridItem = new DayGridItemViewModel
                        {
                            Event = currentEvent,
                            EventColor = $"{Colors.GetHatching(currentEvent.FillStyle, currentEvent.Color)};color:{currentEvent.ForeColor}",
                            ClassPin = string.IsNullOrWhiteSpace(currentEvent.Comment) ? null : " pin",
                            ClassPointer = " cursor-pointer",
                            Day = day
                        };

                        DateTime taskStartHour, taskEndHour;
                        if (currentEvent.DateStart.Date == day.Date && currentEvent.DateEnd.Date > day.Date)
                        {
                            // Event starts today but ends on a future date
                            taskStartHour = currentEvent.DateStart;
                            taskEndHour = day.AddDays(1).Date;
                        }
                        else if (currentEvent.DateStart.Date < day.Date && currentEvent.DateEnd.Date == day.Date)
                        {
                            // Event started in the past but ends today
                            taskStartHour = day.Date;
                            taskEndHour = currentEvent.DateEnd;
                        }
                        else if (currentEvent.DateStart.Date < day.Date && currentEvent.DateEnd.Date > day.Date)
                        {
                            // Event spans across the whole day
                            taskStartHour = day.Date;
                            taskEndHour = day.AddDays(1).Date;
                        }
                        else
                        {
                            // Event starts and ends today
                            taskStartHour = currentEvent.DateStart;
                            taskEndHour = currentEvent.DateEnd;
                        }

                        TimeSpan duration = taskEndHour - taskStartHour;
                        int rowSpan = (int)Math.Ceiling(duration.TotalMinutes / minutes);
                        int startRowIndex = (int)(taskStartHour.TimeOfDay.TotalMinutes / minutes) + 1;

                        gridItem.RowStart = startRowIndex;
                        gridItem.RowEnd = startRowIndex + rowSpan;

                        // Find the first available column
                        gridItem.ColumnStart = 1;
                        for (int row = gridItem.RowStart; row < gridItem.RowEnd; row++)
                        {
                            if (!occupiedSlots.ContainsKey(row))
                            {
                                occupiedSlots[row] = new HashSet<int>();
                            }

                            while (occupiedSlots[row].Contains(gridItem.ColumnStart))
                            {
                                gridItem.ColumnStart++;
                            }
                        }

                        // Mark the time slots as occupied
                        for (int row = gridItem.RowStart; row < gridItem.RowEnd; row++)
                        {
                            if (!occupiedSlots.ContainsKey(row))
                            {
                                occupiedSlots[row] = new HashSet<int>();
                            }
                            occupiedSlots[row].Add(gridItem.ColumnStart);
                        }

                        // Set the CSS grid position
                        gridItem.CSSGridPosition = $"grid-row:{gridItem.RowStart} / span {rowSpan}; grid-column:{gridItem.ColumnStart};";
                        gridItems.Add(gridItem);

                        // If the event ends at midnight and spans multiple days, remove it to avoid duplication
                        if (currentEvent.DateEnd.TimeOfDay == TimeSpan.Zero && currentEvent.DateEnd.Date == day.Date && currentEvent.DateStart.Date < day.Date)
                        {
                            gridItems.Remove(gridItem);
                        }
                    }
                }

                return gridItems;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new ApplicationException("Error generating grid items for day component", ex);
            }
        }



        public List<DayGridItemViewModel> GetGridItemsForAllDayComponent(List<ICalendarEvent> events, DateTime selectedDate)
        {
            List<DayGridItemViewModel> gridItems = new List<DayGridItemViewModel>();

            // Filter events for the selected day
            List<ICalendarEvent> eventsForDay = events.Where(ev =>
                                                 ev.DateStart.Date <= selectedDate.Date &&
                                                 ev.DateEnd.Date >= selectedDate.Date)
                                             .OrderBy(ev => ev.DateStart)
                                             .ThenBy(ev => ev.DateEnd)
                                             .ToList();

            if (eventsForDay.Count == 0)
            {
                return new List<DayGridItemViewModel>();
            }

            foreach (var ev in eventsForDay)
            {
                var colorHatching = Colors.GetHatching(ev.FillStyle, ev.Color);
                DayGridItemViewModel gridItem = new DayGridItemViewModel
                {
                    Event = ev,
                    GridItemColor = $"{colorHatching} color:{ev.ForeColor}",
                    EventColor = $"{colorHatching} color:{ev.ForeColor}",
                    ClassPin = string.IsNullOrWhiteSpace(ev.Comment) ? null : " pin",
                    ClassPointer = " cursor-pointer",
                };

                gridItem.ColumnStart = 1;
                gridItem.ColumnEnd = 2;

                gridItems.Add(gridItem);
            }

            if (gridItems.Count > 0)
            {
                // Initialize the first item
                gridItems[0].RowStart = 1;
                gridItems[0].CSSGridPosition = $"grid-row-start:{gridItems[0].RowStart}; grid-column:{gridItems[0].ColumnStart} / span {gridItems[0].ColumnEnd - gridItems[0].ColumnStart};";

                for (int i = 1; i < gridItems.Count; i++)
                {
                    // Determine the RowStart based on the previous item's ColumnEnd
                    if (gridItems[i].ColumnStart < gridItems[i - 1].ColumnEnd)
                    {
                        gridItems[i].RowStart = gridItems[i - 1].RowStart + 1;
                    }
                    else
                    {
                        gridItems[i].RowStart = 1;
                    }

                    // Update CSSGridPosition for the current item
                    gridItems[i].CSSGridPosition = $"grid-row-start:{gridItems[i].RowStart}; grid-column:{gridItems[i].ColumnStart} / span {gridItems[i].ColumnEnd - gridItems[i].ColumnStart};";
                }
            }

            return gridItems;
        }
    }
}
