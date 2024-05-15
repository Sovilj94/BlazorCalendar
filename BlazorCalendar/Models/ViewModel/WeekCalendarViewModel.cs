using System;
using System.Collections.Generic;
using BlazorCalendar.Models.ViewModel.Interfaces;

namespace BlazorCalendar.Models.ViewModel
{
    public class WeekCalendarViewModel : ICalendarView
    {
        List<DayHeaderViewModel> DayHeader { get; set; }

        List<AllDayViewModel> AllDay { get; set; }

        List<DayCalendarViewModel> Days { get; set; }

        SideBarViewModel SideBar { get; set; }
    }
}
