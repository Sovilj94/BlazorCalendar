using BlazorCalendar.Models;
using BlazorCalendar.Models.DayViewModels;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.WeekViewModels;
using BlazorCalendar.Styles;

namespace BlazorCalendar.Services
{
    public class TasksService
    {
        public List<ICalendarEvent> GetAllTasks()
        {
            DateTime today = DateTime.Today;
            var TasksList = new List<ICalendarEvent>()
            {
                 new Tasks { ID = 0, DateStart = today.AddHours(11), DateEnd = today.AddHours(16), Code = "HELLO", Color = "#FFD800", Caption = "Lorem ipsum dolor sit amet", FillStyle = FillStyleEnum.BackwardDiagonal },
                 new Tasks { ID = 15, DateStart = today.AddDays(3), DateEnd = today.AddDays(5), Code = "POD1", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 18, DateStart = today.AddDays(3), DateEnd = today.AddDays(5), Code = "POD1", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 16, DateStart = today.AddDays(3), DateEnd = today.AddDays(4), Code = "POD2", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 17, DateStart = today.AddDays(3), DateEnd = today.AddDays(4), Code = "POD3", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 10, DateStart = today.AddDays(3), DateEnd = today.AddDays(5), Code = "HELLO2", Color = "#FFD800", Caption = "Lorem ipsum dolor sit amet", FillStyle = FillStyleEnum.BackwardDiagonal },
                 new Tasks { ID = 11, DateStart = today.AddDays(5), DateEnd = today.AddDays(7), Code = "Woha", Color = "#FFD801", Caption = "Lorem ipsum dolor sit amet", FillStyle = FillStyleEnum.BackwardDiagonal },
                 new Tasks { ID = 1, DateStart = today.AddDays(2).AddHours(5), DateEnd = today.AddDays(2).AddHours(11), Code = "😉 CP", Color = "#19C319", Caption = "Lorem ipsum dolor sit amet" } ,
                 new Tasks { ID = 2, DateStart = today.AddDays(-2).AddHours(8), DateEnd = today.AddDays(-2).AddHours(20), Code = "POD", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 3, DateStart = today.AddHours(5), DateEnd = today.AddHours(10), Code = "CALL", Color = "#eb3c37", ForeColor = "#222", Caption = "Lorem ipsum dolor sit amet", FillStyle=FillStyleEnum.CrossDots },
                 new Tasks { ID = 4, DateStart = today.AddDays(31), DateEnd = today.AddDays(31), Code = "MTG", Color = "#19C319", Caption = "MTG:France" },
                 new Tasks { ID = 6, DateStart = today.AddDays(32), DateEnd = today.AddDays(33), Code = "MEET", Color = "#0d6efd" },
                 new Tasks { ID = 7, DateStart = today.AddDays(32), DateEnd = today.AddDays(32), Code = "BLAZOR", Color = "#FFC3FF", Caption = "Blazor Dev" } ,
                 new Tasks { ID = 8, DateStart = today.AddDays(45).AddHours(8), DateEnd = today.AddDays(45).AddHours(9), Code = "MEETING", Color = "#2DD7D7", Comment="Julien's test" },
                 new Tasks { ID = 9, DateStart = today.AddDays(-8), DateEnd = today.AddDays(-7), Code = "MEET⭐", Color = "#0d6efd",Caption = "MTG:France" },
                 new Tasks { ID = 12, DateStart = today.AddDays(2), DateEnd = today.AddDays(6), Code = "MTG", Color = "#19C319", Caption = "MTG:France" },
                 new Tasks { ID = 14, DateStart = today.AddDays(1), DateEnd = today.AddDays(3), Code = "MEET", Color = "#0d6efd" },
            };

            return TasksList;
        }

        public List<ICalendarEvent> GetTasksForWeekViewModel(DateTime FirstDate, DateTime LastDate, List<ICalendarEvent> tasks)
        {
            var TasksList = tasks.Where(x => x.DateStart.Date >= FirstDate.Date && x.DateStart.Date <= LastDate.Date &&
                                (x.DateStart.TimeOfDay != TimeSpan.Zero && x.DateEnd.TimeOfDay != TimeSpan.Zero)).ToList();

            return TasksList;
        }

        public List<ICalendarEvent> GetTasksForDayViewModel(DateTime day, List<ICalendarEvent> tasks)
        {
            var TasksList = tasks.Where(x => x.DateStart.Date == day.Date || x.DateEnd.Date == day.Date).ToList();

            return TasksList;
        }

        public List<ICalendarEvent> GetTasksForAllDayViewModel(List<ICalendarEvent> events)
        {
            var EventList = events.Where(x => x.DateStart.TimeOfDay == TimeSpan.Zero && x.DateEnd.TimeOfDay == TimeSpan.Zero).ToList();

            return EventList;
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


        public List<DayGridItemViewModel> GetGridItemsForDDayComponent(List<ICalendarEvent> events, int minutes, DateTime day)
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



        public List<DayGridItemViewModel> GetGridItemsForDAllDayComponent(List<ICalendarEvent> events, DateTime selectedDate)
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
