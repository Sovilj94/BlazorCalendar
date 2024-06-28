using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.MonthViewModels;
using BlazorCalendar.Styles;

namespace BlazorCalendar.FactoryClasses.CalculatePosition
{
    public class MonthPositionCalculator : ICalendarPositionCalculator<MonthCalendarViewModel>
    {
        public void CalculatePositions(MonthCalendarViewModel viewModel, DateTime firstDate, int? offsetCell, List<ICalendarEvent> eventList)
        {
            DateTime LastDay = new DateTime(firstDate.Year, firstDate.Month, 1).AddMonths(1);

            if (eventList is not null)
            {
                var occupiedPosition = new TaskPosition[32];
                for (int i = 0; i < 32; ++i)
                {
                    occupiedPosition[i] = new TaskPosition();
                }

                string? classPosition;
                string taskContent = "";
                string? taskComment = null;
                bool onmMultiLine = false;
                bool draggable = false;

                for (var k = 0; k < eventList.Count; k++)
                {
                    ICalendarEvent t = eventList[k];

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

                        if (position.Top == false)
                        {
                            for (int i = Start.Day; i < Start.Day + s; ++i)
                            {
                                occupiedPosition[i].Top = true;
                            }
                            classPosition = "monthly-task-first";
                        }
                        else if (position.Center == false)
                        {
                            for (int i = Start.Day; i < Start.Day + s; ++i)
                            {
                                occupiedPosition[i].Center = true;
                            }
                            classPosition = "monthly-task-second";
                        }
                        else if (position.Bottom == false)
                        {
                            for (int i = Start.Day; i < Start.Day + s; ++i)
                            {
                                occupiedPosition[i].Bottom = true;
                            }
                            classPosition = "monthly-task-bottom";
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
                            }
                            else
                            {
                                MonthGridItemViewModel gridItem = new MonthGridItemViewModel
                                {
                                    CSSGridPosition = $"grid-column:{x} / span 1; grid-row:{y};",
                                    CSSClass = "fade-in monthly-more-tasks noselect",
                                    Event = t
                                };
                                viewModel.MonthGridItemViewModels.Add(gridItem);
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
}
