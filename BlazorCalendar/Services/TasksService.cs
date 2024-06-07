using BlazorCalendar.Models;
using BlazorCalendar.Models.ViewModel;
using BlazorCalendar.Styles;

namespace BlazorCalendar.Services
{
    public class TasksService
    {
        public List<Tasks> GetAllTasks()
        {
            DateTime today = DateTime.Today;
            var TasksList = new List<Tasks>()
            {
                 new Tasks { ID = 0, DateStart = today.AddHours(11), DateEnd = today.AddHours(16), Code = "HELLO", Color = "#FFD800", Caption = "Lorem ipsum dolor sit amet", FillStyle = FillStyleEnum.BackwardDiagonal },
                 new Tasks { ID = 10, DateStart = today.AddDays(3), DateEnd = today.AddDays(5), Code = "HELLO", Color = "#FFD800", Caption = "Lorem ipsum dolor sit amet", FillStyle = FillStyleEnum.BackwardDiagonal },
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


        public List<Tasks> GetTasksForWeekViewModel(DateTime FirstDate, DateTime LastDate, List<Tasks> tasks, TimeDivisionEnum timeDivisionEnum)
        {
            var TasksList = tasks.Where(x => x.DateStart.Date >= FirstDate.Date && x.DateStart.Date <= LastDate.Date &&
                                (x.DateStart.TimeOfDay != TimeSpan.Zero && x.DateEnd.TimeOfDay != TimeSpan.Zero)).ToList();

            TasksList = GetTaskPositionForDayGrid(TasksList, new TimeDivision(timeDivisionEnum).Minutes);

            return TasksList;
        }

        public List<Tasks> GetTasksForDayViewModel(DateTime day, List<Tasks> tasks, TimeDivisionEnum timeDivisionEnum)
        {
            var TasksList = tasks.Where(x => (x.DateStart.Date == day.Date || x.DateEnd.Date == day.Date) && x.DateStart.TimeOfDay != TimeSpan.Zero || x.DateEnd.TimeOfDay != TimeSpan.Zero).ToList();

            TasksList = GetTaskPositionForDayGrid(TasksList, new TimeDivision(timeDivisionEnum).Minutes);

            return TasksList;
        }

        public List<Tasks> GetTaskPositionForDayGrid(List<Tasks> tasks, int minutes)
        {
            tasks.OrderBy(x => x.DateStart).ThenBy(x => x.DateEnd).ToList();

            for (int i = 0; i < tasks.Count; i++)
            {
                Tasks currentTask = tasks[i];

                DateTime taskStartHour = currentTask.DateStart;
                DateTime taskEndHour = currentTask.DateEnd;

                TimeSpan duration = taskEndHour - taskStartHour;
                int rowSpan = (int)Math.Ceiling(duration.TotalMinutes) / minutes;

                int startRowIndex = (int)((taskStartHour.TimeOfDay.TotalMinutes) / minutes) + 1;

                currentTask.RowStart = startRowIndex;
                currentTask.RowEnd = startRowIndex + rowSpan;
            }

            var tasksGroupedByDate = tasks
                .Where(x => x.DateStart.TimeOfDay != TimeSpan.Zero || x.DateEnd.TimeOfDay != TimeSpan.Zero)
               .GroupBy(t => t.DateStart.Date)
               .ToList();

            foreach (var group in tasksGroupedByDate)
            {
                var tasksInDay = group.OrderBy(t => t.DateStart).ToList();

                for (int i = 0; i < tasksInDay.Count; i++)
                {
                    var currentTask = tasksInDay[i];
                    currentTask.ColumnStart = 1;

                    for (int j = 0; j < i; j++)
                    {
                        var previousTask = tasksInDay[j];
                        if (currentTask.DateStart < previousTask.DateEnd)
                        {
                            currentTask.ColumnStart = previousTask.ColumnStart + 1;
                        }
                    }
                }
            }
            return tasks;
        }


        public List<Tasks> GetAllDayTaskPositionForDayGrid(List<Tasks> tasks, DateTime firstDateOfWeek)
        {
            HashSet<(int, DateTime)> dayAndNumbers = new HashSet<(int number, DateTime day)>();
            int counter = 0;

            for (int i = 2; i <= 8; i++)
            {
                dayAndNumbers.Add((i, firstDateOfWeek.AddDays(counter)));
                counter++;
            }

            var lastDateOfWeek = firstDateOfWeek.AddDays(6);
            List<Tasks> tasksForWeek = tasks.Where(task =>
                                             task.DateStart.TimeOfDay == TimeSpan.Zero && // Starts at 12 AM
                                             task.DateEnd.TimeOfDay == TimeSpan.Zero && // Ends at 12 AM
                                             (task.DateStart.Date <= lastDateOfWeek && task.DateEnd.Date >= firstDateOfWeek) // Falls within the week
                                         )
                                         .OrderBy(task => task.DateStart)
                                         .ThenBy(task => task.DateEnd)
                                         .ToList();

            if (tasksForWeek.Count == 0)
            {
                return new List<Tasks>();
            }

            // Set the column start and end
            foreach (var task in tasksForWeek)
            {
                if (task.DateStart.Date < firstDateOfWeek)
                {
                    task.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == firstDateOfWeek).Item1;
                }
                else
                {
                    task.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == task.DateStart.Date).Item1;
                }
                if (task.DateEnd.Date > lastDateOfWeek)
                {
                    task.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == lastDateOfWeek).Item1 + 1;
                }
                else
                {
                    task.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == task.DateEnd.Date).Item1 + 1;
                }
            }

            // Set the row start
            if (tasksForWeek.Count > 1)
            {
                for (int i = 1; i < tasksForWeek.Count; i++)
                {
                    tasksForWeek[0].RowStart = 1;

                    if (tasksForWeek[i].ColumnStart < tasksForWeek[0].ColumnEnd)
                    {
                        tasksForWeek[i].RowStart = tasksForWeek[i - 1].RowStart + 1;
                    }
                    else
                    {
                        tasksForWeek[i].RowStart = 1;
                    }
                }
            }
            else
            {
                tasksForWeek[0].RowStart = 1;
            }

            return tasksForWeek;
        }

        public List<GridItemViewModel> GetGridItemsForAllDayComponent(List<Tasks> tasks, DateTime firstDateOfWeek)
        {
            HashSet<(int, DateTime)> dayAndNumbers = new HashSet<(int number, DateTime day)>();

            List<GridItemViewModel> gridItems = new List<GridItemViewModel>();
            int counter = 0;

            for (int i = 2; i <= 8; i++)
            {
                dayAndNumbers.Add((i, firstDateOfWeek.AddDays(counter)));
                counter++;
            }

            var lastDateOfWeek = firstDateOfWeek.AddDays(6);
            List<Tasks> tasksForWeek = tasks.Where(task =>
                                             task.DateStart.TimeOfDay == TimeSpan.Zero &&
                                             task.DateEnd.TimeOfDay == TimeSpan.Zero &&
                                             (task.DateStart.Date <= lastDateOfWeek && task.DateEnd.Date >= firstDateOfWeek)
                                         )
                                         .OrderBy(task => task.DateStart)
                                         .ThenBy(task => task.DateEnd)
                                         .ToList();

            if (tasksForWeek.Count == 0)
            {
                return new List<GridItemViewModel>();
            }

            foreach (var task in tasksForWeek)
            {
                var colorHatching = Colors.GetHatching(task.FillStyle, task.Color);
                GridItemViewModel gridItem = new GridItemViewModel
                {
                    Task = task,
                    GridItemColor = $"{colorHatching} color:{task.ForeColor}",
                    TaskColor = $"{colorHatching} color:{task.ForeColor}",
                    ClassPin = string.IsNullOrWhiteSpace(task.Comment) ? null : " pin",
                    ClassPointer = " cursor-pointer",
                };
                
                if (task.DateStart.Date < firstDateOfWeek)
                {
                    task.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == firstDateOfWeek).Item1;
                    gridItem.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == firstDateOfWeek).Item1;

                }
                else
                {
                    task.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == task.DateStart.Date).Item1;
                    gridItem.ColumnStart = dayAndNumbers.First(x => x.Item2.Date == task.DateStart.Date).Item1;
                }
                if (task.DateEnd.Date > lastDateOfWeek)
                {
                    task.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == lastDateOfWeek).Item1 + 1;
                    gridItem.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == lastDateOfWeek).Item1 + 1;
                }
                else
                {
                    task.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == task.DateEnd.Date).Item1 + 1;
                    gridItem.ColumnEnd = dayAndNumbers.First(x => x.Item2.Date == task.DateEnd.Date).Item1 + 1;
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
    }
}
