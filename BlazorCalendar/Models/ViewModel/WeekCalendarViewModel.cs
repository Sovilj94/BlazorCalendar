using System;
using System.Collections.Generic;
using BlazorCalendar.Models.ViewModel.Interfaces;

namespace BlazorCalendar.Models.ViewModel
{
    public class WeekCalendarViewModel : ICalendarView
    {
        public DayHeaderViewModel DayHeader { get; set; }

        public List<AllDayViewModel> AllDay { get; set; }

        public DayCalendarViewModel DayCalendar { get; set; } = new DayCalendarViewModel();

        public TimeSideBarViewModel TimeSideBar { get; set; } = new TimeSideBarViewModel();
    }
}
