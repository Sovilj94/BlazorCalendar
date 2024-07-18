using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.MonthViewModels;
using BlazorCalendar.Styles;

namespace BlazorCalendar.FactoryClasses.CalculatePosition
{
    public class MonthPositionCalculator : ICalendarPositionCalculator<MonthCalendarViewModel>
    {
        public void CalculatePositions(MonthCalendarViewModel viewModel, DateTime firstDate, int? offsetCell, List<ICalendarEvent> eventList)
        {
            if (viewModel == null || eventList == null) return;

            DateTime LastDay = new DateTime(firstDate.Year, firstDate.Month, 1).AddMonths(1);

            var occupiedPosition = new TaskPosition[32];
            var eventCounter = new int[32]; // Track the number of events per day
            for (int i = 0; i < 32; ++i)
            {
                occupiedPosition[i] = new TaskPosition();
            }

            string? classPosition;
            string taskContent = "";
            string? taskComment = null;
            bool onmMultiLine = false;
            bool draggable = false;

            // Sort the events by start date first and then by duration (longest first)
            var sortedEventList = eventList.OrderBy(e => e.DateStart.Date)
                                           .ThenByDescending(e => (e.DateEnd.Date - e.DateStart.Date).TotalDays)
                                           .ToList();

            viewModel.MonthGridItemViewModels ??= new List<MonthGridItemViewModel>();
            viewModel.MonthGridItemsListViewModels ??= new List<MonthGridItemListViewModel>();

            for (var k = 0; k < sortedEventList.Count; k++)
            {
                ICalendarEvent t = sortedEventList[k];

                if ((t.DateStart.Date <= firstDate && firstDate <= t.DateEnd.Date) ||
                    (t.DateStart.Date > firstDate && LastDay > t.DateEnd.Date) ||
                    (t.DateStart.Date < LastDay && LastDay <= t.DateEnd.Date))
                {
                    draggable = t.NotBeDraggable ? false : draggable;

                    DateTime Start = t.DateStart.Date < firstDate ? firstDate : t.DateStart.Date;
                    DateTime End = t.DateEnd.Date >= LastDay ? LastDay.AddDays(-1) : t.DateEnd.Date;

                    int x = (int)((Start.Day + offsetCell - 1) % 7 + 1);
                    int y = (int)((Start.Day + offsetCell - 1) / 7 + 2);
                    int s = (int)(End.Date - Start.Date).TotalDays + 1;

                    classPosition = null;

                    TaskPosition position = occupiedPosition[Start.Day];
                    bool positionFound = false;

                    for (int i = 0; i < 3 && !positionFound; i++)
                    {
                        bool canPlace = true;

                        for (int j = Start.Day; j < Start.Day + s && canPlace; j++)
                        {
                            canPlace &= !(occupiedPosition[j].Top && i == 0) &&
                                         !(occupiedPosition[j].Center && i == 1) &&
                                         !(occupiedPosition[j].Bottom && i == 2);
                        }

                        if (canPlace)
                        {
                            positionFound = true;
                            for (int j = Start.Day; j < Start.Day + s; j++)
                            {
                                if (i == 0) occupiedPosition[j].Top = true;
                                else if (i == 1) occupiedPosition[j].Center = true;
                                else if (i == 2) occupiedPosition[j].Bottom = true;
                            }

                            classPosition = i == 0 ? "monthly-task-first" :
                                            i == 1 ? "monthly-task-second" :
                                                     "monthly-task-bottom";
                        }
                    }

                    string borderClass = "border-start";
                    do
                    {
                        string row = $"grid-column:{x} / span {s}; grid-row:{y};";

                        if (classPosition is not null)
                        {
                            taskContent = string.IsNullOrWhiteSpace(t.Caption) ? t.Code : t.Caption;

                            if (t.DateStart.Hour + t.DateStart.Minute > 0)
                            {
                                taskContent = $"{t.DateStart.ToString("t")} {taskContent}";
                            }

                            taskComment = string.IsNullOrWhiteSpace(t.Comment) ? null : t.Comment;

                            string taskColor = Colors.GetHatching(t.FillStyle, t.Color);
                            if (!String.IsNullOrEmpty(t.ForeColor))
                            {
                                taskColor = taskColor + $"color:{t.ForeColor}";
                            }

                            MonthGridItemViewModel gridItem = new MonthGridItemViewModel
                            {
                                CSSGridPosition = row,
                                GridItemColor = taskColor,
                                CSSClass = $"fade-in monthly-task {borderClass} cursor-pointer {classPosition}",
                                Event = t
                            };
                            viewModel.MonthGridItemViewModels.Add(gridItem);

                            eventCounter[Start.Day]++; // Increment the event counter for the day

                            // Check if more than 3 events and add placeholder
                            if (eventCounter[Start.Day] > 3)
                            {
                                viewModel.MonthGridItemViewModels.RemoveAll(g => g.Event.DateStart.Day == Start.Day && g.Event.DateEnd.Day == Start.Day);
                                MonthGridItemListViewModel placeholderItem = new MonthGridItemListViewModel
                                {
                                    CSSGridPosition = $"grid-column:{x} / span 1; grid-row:{y};",
                                    CSSClass = "fade-in monthly-more-tasks noselect",
                                    EventCounter = eventCounter[Start.Day]
                                };
                                viewModel.MonthGridItemsListViewModels.Add(placeholderItem);
                            }
                        }
                        else
                        {
                            MonthGridItemListViewModel gridItem = new MonthGridItemListViewModel
                            {
                                CSSGridPosition = $"grid-column:{x} / span 1; grid-row:{y};",
                                CSSClass = "fade-in monthly-more-tasks noselect",
                                EventCounter = 1
                            };
                            viewModel.MonthGridItemsListViewModels.Add(gridItem);
                        }

                        onmMultiLine = false;
                        if (x + s > 8)
                        {
                            onmMultiLine = true;

                            Start = Start.AddDays(8 - x);
                            End = t.DateEnd.Date >= LastDay ? LastDay.AddDays(-1) : t.DateEnd.Date;

                            x = (int)(Start.Day + offsetCell - 1) % 7 + 1;
                            y = (int)(Start.Day + offsetCell - 1) / 7 + 2;
                            s = (int)(End.Date - Start.Date).TotalDays + 1;

                            borderClass = "";
                        }

                    } while (onmMultiLine);

                    Start = t.DateStart.Date < firstDate ? firstDate : t.DateStart.Date;
                    End = t.DateEnd.Date >= LastDay ? LastDay.AddDays(-1) : t.DateEnd.Date;

                    for (int d = Start.Day; d <= End.Day; d++)
                    {
                        occupiedPosition[d].Counter++;
                    }
                }
            }
        }
    }
}