using BlazorCalendar.Models;
using BlazorCalendar.Models.ViewModel;
using BlazorCalendar.Models.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BlazorCalendar.Services
{
    public class CalendarViewFactory : ICalendarViewFactory
    {
        private readonly TasksService _tasksService;
        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly string[] _dayNames = { "Ned", "Pon", "Uto", "Sre", "Čet", "Pet", "Sub" };

        public CalendarViewFactory(TasksService tasksService)
        {
            _tasksService = tasksService;
        }

        public ICalendarView CreateCalendarView(DisplayedView viewType, DateTime firstDate, TimeDivisionEnum timeDivision)
        {
            switch (viewType)
            {
                case DisplayedView.Weekly:
                    return CreateWeekCalendarView(firstDate, timeDivision);
                case DisplayedView.Daily:
                    // Implement and return the daily view model creation logic here
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }

        private ICalendarView CreateWeekCalendarView(DateTime firstDate, TimeDivisionEnum timeDivision)
        {
            var weekCalendarViewModel = new WeekCalendarViewModel();

            var firstDayOfWeek = _culture.DateTimeFormat.FirstDayOfWeek;
            var firstDateWeek = firstDate.AddDays(-(firstDate.DayOfWeek - firstDayOfWeek));
            var lastDateOfWeek = firstDateWeek.AddDays(6);

            weekCalendarViewModel.DayHeader = new List<DayHeaderViewModel>();
            weekCalendarViewModel.DayCalendar = new List<DayCalendarViewModel>();
            weekCalendarViewModel.AllDay = new AllDayViewModel();
            weekCalendarViewModel.TimeSideBar = new TimeSideBarViewModel();
            weekCalendarViewModel.Tasks = _tasksService.GetTasksForWeekViewModel(firstDateWeek, lastDateOfWeek, _tasksService.GetAllTasks(), timeDivision);

            for (int i = 0; i < 7; i++)
            {
                int d = Dates.GetNumOfDay(i);
                DateTime day = firstDateWeek.AddDays(i);
                var dayHeader = new DayHeaderViewModel
                {
                    DayName = _dayNames[d],
                    DayDate = day.ToString("dd.MM")
                };

                var dayCalendar = new DayCalendarViewModel
                {
                    Day = day,
                    TimeDivision = new TimeDivision(timeDivision),
                    DayTasks = _tasksService.GetTasksForDayViewModel(day, weekCalendarViewModel.Tasks, timeDivision)
                };

                if (dayCalendar.DayTasks != null && dayCalendar.DayTasks.Count != 0)
                {
                    dayCalendar.MaxNumberOfColumns = dayCalendar.DayTasks.Max(x => x.ColumnStart);
                }
                else
                {
                    dayCalendar.MaxNumberOfColumns = 1;
                }

                weekCalendarViewModel.DayHeader.Add(dayHeader);
                weekCalendarViewModel.DayCalendar.Add(dayCalendar);
            }

            weekCalendarViewModel.AllDay.Tasks = _tasksService.GetAllDayTaskPositionForDayGrid(_tasksService.GetAllTasks(), firstDateWeek);
            weekCalendarViewModel.AllDay.FirstDateWeek = firstDateWeek;
            weekCalendarViewModel.TimeSideBar.TimeDivision = new TimeDivision(timeDivision);

            return weekCalendarViewModel;
        }
    }
}
