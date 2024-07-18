using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.WeekViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorCalendar.FactoryClasses.CalculatePosition
{
    public class WeekPositionCalculator : ICalendarPositionCalculator<WeekCalendarViewModel>
    {
        public void CalculatePositions(WeekCalendarViewModel viewModel, DateTime firstDate, int? offsetCell, List<ICalendarEvent> eventList)
        {

        }
    }
}
