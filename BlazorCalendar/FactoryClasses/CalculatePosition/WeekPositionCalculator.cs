using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.WeekViewModels;
using BlazorCalendar.Styles;

namespace BlazorCalendar.FactoryClasses.CalculatePosition
{
    public class WeekPositionCalculator
    {
        public List<WeekGridItemViewModel> CalculateGridItemsForDayComponent(List<ICalendarEvent> events, int minutes, DateTime day)
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
                        taskStartHour = currentEvent.DateStart;
                        taskEndHour = day.AddDays(1).Date;
                    }
                    else if (currentEvent.DateStart.Date < day.Date && currentEvent.DateEnd.Date == day.Date)
                    {
                        taskStartHour = day.Date;
                        taskEndHour = currentEvent.DateEnd;
                    }
                    else if (currentEvent.DateStart.Date < day.Date && currentEvent.DateEnd.Date > day.Date)
                    {
                        taskStartHour = day.Date;
                        taskEndHour = day.AddDays(1).Date;
                    }
                    else
                    {
                        taskStartHour = currentEvent.DateStart;
                        taskEndHour = currentEvent.DateEnd;
                    }

                    TimeSpan duration = taskEndHour - taskStartHour;
                    int rowSpan = (int)Math.Ceiling(duration.TotalMinutes / minutes);
                    int startRowIndex = (int)(taskStartHour.TimeOfDay.TotalMinutes / minutes) + 1;

                    gridItem.RowStart = startRowIndex;
                    gridItem.RowEnd = startRowIndex + rowSpan;

                    gridItems.Add(gridItem);

                    if (currentEvent.DateEnd.TimeOfDay == TimeSpan.Zero && currentEvent.DateEnd.Date == day.Date && currentEvent.DateStart.Date < day.Date)
                        gridItems.Remove(gridItem);
                }
            }

            CalculatePositionsForDayComponent(gridItems, minutes, day);
            return gridItems;
        }

        public List<WeekGridItemViewModel> CalculateGridItemsForAllDayComponent(List<ICalendarEvent> events, DateTime firstDateOfWeek)
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


            CalculatePositionsForAllDayComponent(gridItems, firstDateOfWeek);
            return gridItems;
        }

        public void CalculatePositionsForDayComponent(List<WeekGridItemViewModel> gridItems, int minutes, DateTime day)
        {
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
        }

        public void CalculatePositionsForAllDayComponent(List<WeekGridItemViewModel> gridItems, DateTime firstDateOfWeek)
        {
            if (gridItems.Count == 0)
                return;

            int daysInWeek = 7;
            List<List<int>> occupiedRows = new List<List<int>>();
            for (int i = 0; i < daysInWeek; i++)
            {
                occupiedRows.Add(new List<int>());
            }

            foreach (var gridItem in gridItems)
            {
                int rowStart = 1;
                bool foundRow = false;

                while (!foundRow)
                {
                    foundRow = true;
                    for (int day = gridItem.ColumnStart - 2; day < gridItem.ColumnEnd - 2; day++)
                    {
                        if (occupiedRows[day].Contains(rowStart))
                        {
                            foundRow = false;
                            rowStart++;
                            break;
                        }
                    }
                }

                gridItem.RowStart = rowStart;
                for (int day = gridItem.ColumnStart - 2; day < gridItem.ColumnEnd - 2; day++)
                {
                    occupiedRows[day].Add(rowStart);
                }

                gridItem.CSSGridPosition = $"grid-row-start:{gridItem.RowStart}; grid-column:{gridItem.ColumnStart} / span {gridItem.ColumnEnd - gridItem.ColumnStart};";
            }
        }
    }
}
