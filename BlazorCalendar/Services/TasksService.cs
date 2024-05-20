using BlazorCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                 new Tasks { ID = 1, DateStart = today.AddDays(4).AddHours(8), DateEnd = today.AddDays(4).AddHours(11), Code = "😉 CP", Color = "#19C319", Caption = "Lorem ipsum dolor sit amet" } ,
                 new Tasks { ID = 2, DateStart = today.AddDays(-2).AddHours(8), DateEnd = today.AddDays(-2).AddHours(20), Code = "POD", Color = "#844fe7", Caption = "Podcast DevApps", FillStyle = FillStyleEnum.ZigZag } ,
                 new Tasks { ID = 3, DateStart = today.AddHours(5), DateEnd = today.AddHours(10), Code = "CALL", Color = "#eb3c37", ForeColor = "#222", Caption = "Lorem ipsum dolor sit amet", FillStyle=FillStyleEnum.CrossDots },
                 new Tasks { ID = 4, DateStart = today.AddDays(31), DateEnd = today.AddDays(31), Code = "MTG", Color = "#19C319", Caption = "MTG:France" },
                 new Tasks { ID = 5, DateStart = today.AddDays(40), DateEnd = today.AddDays(42), Code = "DEV", Color = "#FFD800", Comment="on Teams template", Caption = "Fusce quis purus eu ante" },
                 new Tasks { ID = 6, DateStart = today.AddDays(32), DateEnd = today.AddDays(33), Code = "MEET", Color = "#0d6efd" },
                 new Tasks { ID = 7, DateStart = today.AddDays(32), DateEnd = today.AddDays(32), Code = "BLAZOR", Color = "#FFC3FF", Caption = "Blazor Dev" } ,
                 new Tasks { ID = 8, DateStart = today.AddDays(45).AddHours(8), DateEnd = today.AddDays(45).AddHours(9), Code = "MEETING", Color = "#2DD7D7", Comment="Julien's test" },
                 new Tasks { ID = 9, DateStart = today.AddDays(-8), DateEnd = today.AddDays(-7), Code = "MEET⭐", Color = "#0d6efd",Caption = "MTG:France" }
            };

            return TasksList;
        }

        public List<Tasks> GetTaskPositionForDayGrid(DateTime day, List<Tasks> tasks,int minutes)
        {
            List<Tasks> tasksForDate = tasks
           .Where(task => task.DateStart.Date == day.Date || task.DateEnd.Date == day.Date)
           .OrderBy(x => x.DateStart)
           .ThenBy(x => x.DateEnd)
           .ToList();

            for (int i = 0; i < tasksForDate.Count; i++)
            {
                Tasks currentTask = tasksForDate[i];

                if (currentTask.DateStart.TimeOfDay != TimeSpan.Zero || currentTask.DateEnd.TimeOfDay != TimeSpan.Zero)
                {
                    DateTime taskStartHour = currentTask.DateStart > day ? currentTask.DateStart : day;
                    DateTime taskEndHour = currentTask.DateEnd < day.AddDays(1) ? currentTask.DateEnd : day.AddDays(1);

                    TimeSpan duration = taskEndHour - taskStartHour;
                    int rowSpan = (int)Math.Ceiling(duration.TotalMinutes) / minutes;

                    int startRowIndex = (int)((taskStartHour.TimeOfDay.TotalMinutes) / minutes) + 1;
                    currentTask.RowStart = startRowIndex;
                    currentTask.RowEnd = startRowIndex + rowSpan;

                    int nearestColumn = 1;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        Tasks previousTask = tasksForDate[j];
                        if (previousTask.RowEnd > currentTask.RowStart)
                        {
                            nearestColumn = Math.Max(nearestColumn, previousTask.ColumnStart + 1);
                        }
                    }

                    currentTask.ColumnStart = nearestColumn;
                }
            }

            return tasksForDate;
        }
        public List<Tasks> GetAllDayTaskPositionForDayGrid(DateTime day, List<Tasks> tasks, int minutes,DateTime FirstDateWeek)
        {
            var lastDateWeek = FirstDateWeek.AddDays(6);
            List<Tasks> tasksForDate = tasks
                .Where(task => task.DateStart.Date >= FirstDateWeek.Date && task.DateEnd.Day - task.DateStart.Day >= 1)
                .OrderBy(x => x.DateStart)
                .ThenBy(x => x.DateEnd)
                .ToList();

            for (int i = 0; i < tasksForDate.Count; i++)
            {
                Tasks currentTask = tasksForDate[i];

                if (currentTask.DateStart.TimeOfDay == TimeSpan.Zero && currentTask.DateEnd.TimeOfDay == TimeSpan.Zero)
                {
                    TimeSpan duration = currentTask.DateEnd - currentTask.DateStart;

                    int numberOfDays = (currentTask.DateEnd - currentTask.DateStart).Days;

                    //get day number of the week
                    int startDayNumber = (int)currentTask.DateStart.DayOfWeek;
                    int endDayNumber = (int)currentTask.DateEnd.DayOfWeek;




                    if (numberOfDays >= 1)
                    {
                        int row = 1;

                        currentTask.ColumnStart = startDayNumber + 2;
                        if (endDayNumber == 0)
                        {
                            currentTask.ColumnEnd = 9;
                        }
                        else
                        {
                            currentTask.ColumnEnd = endDayNumber + 2;
                        }
                        currentTask.RowStart = row;
                        row++;
                    }
                }
            }

            return tasksForDate;
        }
    }
}
