using BlazorCalendar.Models;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.WeekViewModels;
using BlazorCalendar.Services;
using BlazorCalendar.Styles;
using System.Globalization;

namespace BlazorCalendar.FactoryClasses
{
    public class WeekCalendarViewFactory : ICalendarViewFactory
    {
        private readonly TasksService _tasksService;
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly string[] _dayNames = { "Ned", "Pon", "Uto", "Sre", "Čet", "Pet", "Sub" };

        public WeekCalendarViewFactory()
        {
            _tasksService = new TasksService();
        }

        public ICalendarView CreateCalendarView(DateTime firstDate, TimeDivisionEnum timeDivision, List<ICalendarEvent> calendarEvents)
        {
            // Prvi i zadnji dan u sedmici
            var firstDayOfWeek = _culture.DateTimeFormat.FirstDayOfWeek;
            var firstDateWeek = firstDate.AddDays(-(firstDate.DayOfWeek - firstDayOfWeek));
            var lastDateOfWeek = firstDateWeek.AddDays(6);

            // Inicijalizacija view modela
            WeekCalendarViewModel weekCalendarViewModel = new WeekCalendarViewModel();
            weekCalendarViewModel.DayHeader = new List<WeekDayHeaderViewModel>();
            weekCalendarViewModel.AllDay = new WeekAllDayViewModel();
            weekCalendarViewModel.TimeSideBar = new WeekTimeSideBarViewModel();
            weekCalendarViewModel.DayCalendar = new List<WeekDayViewModel>();
            weekCalendarViewModel.Tasks = _tasksService.GetTasksForWeekViewModel(firstDateWeek, lastDateOfWeek, _tasksService.GetAllTasks());

            // Popunjavanje AllDayViewModela
            weekCalendarViewModel.AllDay.Events = _tasksService.GetTasksForAllDayViewModel(_tasksService.GetAllTasks());
            weekCalendarViewModel.AllDay.FirstDateWeek = firstDateWeek;
            weekCalendarViewModel.AllDay.TimeCells = new List<WeekTimeCellViewModel>();
            weekCalendarViewModel.AllDay.GridItems = new List<WeekGridItemViewModel>();
            weekCalendarViewModel.AllDay.GridItems = GetGridItemsForAllDayComponent(weekCalendarViewModel.AllDay.Events, firstDateWeek);

            // TimeCells
            for (int column = 0; column < 7; column++)
            {
                WeekTimeCellViewModel timeCell = new WeekTimeCellViewModel();
                timeCell.IsAllDayTimesCell = true;
                timeCell.Time = weekCalendarViewModel.AllDay.FirstDateWeek.AddDays(column);
                timeCell.Column = column + 2;
                timeCell.CSSGridPosition = $"grid-row:1 / span 5; grid-column-start:{column + 2}; height:100px; border-right:1px solid #ccc";

                weekCalendarViewModel.AllDay.TimeCells.Add(timeCell);
            }

            // TimeSideBar
            weekCalendarViewModel.TimeSideBar.TimeDivision = new TimeDivision(timeDivision);

            // Popunjavanje DayHeaderViewModel i DayCalendarViewModel
            for (int i = 0; i < 7; i++)
            {
                int d = Dates.GetNumOfDay(i);
                DateTime day = firstDateWeek.AddDays(i);
                var dayHeader = new WeekDayHeaderViewModel
                {
                    DayName = _dayNames[d],
                    DayDate = day.ToString("dd.MM")
                };

                var dayCalendar = new WeekDayViewModel
                {
                    Day = day,
                    TimeDivision = new TimeDivision(timeDivision),
                    GridItems = GetGridItemsForDayComponent(_tasksService.GetTasksForDayViewModel(day, weekCalendarViewModel.Tasks), new TimeDivision(timeDivision).Minutes, day),
                    TimeCells = new List<WeekTimeCellViewModel>()
                };

                if (dayCalendar.GridItems != null && dayCalendar.GridItems.Count != 0)
                {
                    dayCalendar.MaxNumberOfColumns = dayCalendar.GridItems.Max(x => x.ColumnStart);
                }
                else
                {
                    dayCalendar.MaxNumberOfColumns = 1;
                }

                for (int dayTime = 0; dayTime < dayCalendar.TimeDivision.NumberOfCells; dayTime++)
                {
                    DateTime time = dayCalendar.Day.AddMinutes(dayTime * dayCalendar.TimeDivision.Minutes);
                    int row = dayTime + 1;

                    WeekTimeCellViewModel timeCell = new WeekTimeCellViewModel();

                    timeCell.Row = row;
                    timeCell.ColumnsSpan = dayCalendar.MaxNumberOfColumns;
                    timeCell.CSSGridPosition = $"grid-row:{timeCell.Row}; grid-column:1 / span {timeCell.ColumnsSpan};";
                    timeCell.Time = time;
                    timeCell.CSSbackground = GetBackground(dayCalendar.Day, dayCalendar);

                    dayCalendar.TimeCells.Add(timeCell);
                }

                weekCalendarViewModel.DayHeader.Add(dayHeader);
                weekCalendarViewModel.DayCalendar.Add(dayCalendar);
            }

            return weekCalendarViewModel;
        }

        public List<WeekGridItemViewModel> GetGridItemsForDayComponent(List<ICalendarEvent> events, int minutes, DateTime day)
        {
            try
            {
                List<WeekGridItemViewModel> gridItems = new List<WeekGridItemViewModel>();

                events = events.Where(x => x.DateStart.TimeOfDay != TimeSpan.Zero || x.DateEnd.TimeOfDay != TimeSpan.Zero)
                               .OrderBy(x => x.DateStart)
                               .ThenBy(x => x.DateEnd)
                               .ToList();

                foreach (var currentEvent in events)
                {
                    if (currentEvent.DateStart.Date <= day.Date && currentEvent.DateEnd.Date >= day.Date)
                    {
                        WeekGridItemViewModel gridItem = new WeekGridItemViewModel
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

                        gridItems.Add(gridItem);

                        // naci srecnije rijesenje 
                        if (currentEvent.DateEnd.TimeOfDay == TimeSpan.Zero && currentEvent.DateEnd.Date == day.Date && currentEvent.DateStart.Date < day.Date)
                            gridItems.Remove(gridItem);
                    }
                }


                var gridItemsGroupedByDate = gridItems.GroupBy(t => t.Day.Date).ToList();

                foreach (var group in gridItemsGroupedByDate)
                {
                    var gridItemsInDay = group.OrderBy(t => t.RowStart).ThenBy(t => t.RowEnd).ToList();

                    for (int i = 0; i < gridItemsInDay.Count; i++)
                    {
                        var currentGridItem = gridItemsInDay[i];
                        currentGridItem.ColumnStart = 1;

                        for (int j = 0; j < i; j++)
                        {
                            var previousGridItem = gridItemsInDay[j];

                            // Check for overlap
                            if (currentGridItem.RowStart < previousGridItem.RowEnd && currentGridItem.RowEnd > previousGridItem.RowStart)
                            {
                                currentGridItem.ColumnStart = Math.Max(currentGridItem.ColumnStart, previousGridItem.ColumnStart + 1);
                            }
                        }
                    }
                }

                foreach (var grItem in gridItems)
                {
                    string gridRow = $"grid-row: {grItem.RowStart} / span {grItem.RowEnd - grItem.RowStart};";
                    string gridColumn = $"grid-column-start: {grItem.ColumnStart};";
                    string CSSGridPosition = $"{gridRow} {gridColumn}";
                    grItem.CSSGridPosition = CSSGridPosition;
                }

                return gridItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error grouping grid items: {ex.Message}");
                return new List<WeekGridItemViewModel>();
            }
        }

        public List<WeekGridItemViewModel> GetGridItemsForAllDayComponent(List<ICalendarEvent> events, DateTime firstDateOfWeek)
        {
            HashSet<(int, DateTime)> dayAndNumbers = new HashSet<(int number, DateTime day)>();

            List<WeekGridItemViewModel> gridItems = new List<WeekGridItemViewModel>();
            int counter = 0;

            for (int i = 2; i <= 8; i++)
            {
                dayAndNumbers.Add((i, firstDateOfWeek.AddDays(counter)));
                counter++;
            }

            var lastDateOfWeek = firstDateOfWeek.AddDays(6);
            List<ICalendarEvent> eventsForWeek = events.Where(ev =>
                                             ev.DateStart.TimeOfDay == TimeSpan.Zero &&
                                             ev.DateEnd.TimeOfDay == TimeSpan.Zero &&
                                             (ev.DateStart.Date <= lastDateOfWeek && ev.DateEnd.Date >= firstDateOfWeek))
                                         .OrderBy(ev => ev.DateStart)
                                         .ThenBy(ev => ev.DateEnd)
                                         .ToList();

            if (eventsForWeek.Count == 0)
            {
                return new List<WeekGridItemViewModel>();
            }

            foreach (var ev in eventsForWeek)
            {
                var colorHatching = Colors.GetHatching(ev.FillStyle, ev.Color);
                WeekGridItemViewModel gridItem = new WeekGridItemViewModel
                {
                    Event = ev,
                    GridItemColor = $"{colorHatching} color:{ev.ForeColor}",
                    EventColor = $"{colorHatching} color:{ev.ForeColor}",
                    ClassPin = string.IsNullOrWhiteSpace(ev.Comment) ? null : " pin",
                    ClassPointer = " cursor-pointer",
                };

                if (ev.DateStart.Date < firstDateOfWeek)
                {
                    gridItem.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == firstDateOfWeek).Item1;

                }
                else
                {
                    gridItem.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == ev.DateStart.Date).Item1;
                }
                if (ev.DateEnd.Date > lastDateOfWeek)
                {
                    gridItem.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == lastDateOfWeek).Item1 + 1;
                }
                else
                {
                    gridItem.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == ev.DateEnd.Date).Item1 + 1;
                }
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

        private string GetBackground(DateTime day, WeekDayViewModel dayViewModel)
        {
            int d = (int)day.DayOfWeek;

            if (d == 6)
            {
                return $"background:{dayViewModel.SaturdayColor}";
            }
            else if (d == 0)
            {
                return $"background:{dayViewModel.SundayColor}";
            }

            return $"background:{dayViewModel.WeekDaysColor}";
        }
    }
}
