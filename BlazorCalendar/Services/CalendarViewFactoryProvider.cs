using BlazorCalendar.FactoryClasses;
using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Services
{
    public class CalendarViewFactoryProvider
    {
        public ICalendarView CreateCalendarView(DisplayedView viewType, DateTime firstDate, TimeDivisionEnum timeDivision)
        {
            switch (viewType)
            {
                case DisplayedView.Weekly:
                    return new WeekCalendarViewFactory().CreateCalendarView(firstDate, timeDivision);
                case DisplayedView.Daily:
                    return new DayCalendarViewFactory().CreateCalendarView(firstDate, timeDivision);
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }

    }
}
