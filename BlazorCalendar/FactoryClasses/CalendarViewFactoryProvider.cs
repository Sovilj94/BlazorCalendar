using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.FactoryClasses
{
    public class CalendarViewFactoryProvider
    {
        private readonly MonthCalendarViewFactory _monthCalendarViewFactory;
        private readonly WeekCalendarViewFactory _weekCalendarViewFactory;
        private readonly DayCalendarViewFactory _dayCalendarViewFactory;

        public CalendarViewFactoryProvider(
            MonthCalendarViewFactory monthCalendarViewFactory,
            WeekCalendarViewFactory weekCalendarViewFactory,
            DayCalendarViewFactory dayCalendarViewFactory)
        {
            _monthCalendarViewFactory = monthCalendarViewFactory;
            _weekCalendarViewFactory = weekCalendarViewFactory;
            _dayCalendarViewFactory = dayCalendarViewFactory;
        }

        public ICalendarView CreateCalendarView(DisplayedView viewType, DateTime today, TimeDivisionEnum timeDivision, List<ICalendarEvent> calendarEvents)
        {
            return viewType switch
            {
                DisplayedView.Weekly => _weekCalendarViewFactory.CreateCalendarView(today, timeDivision, calendarEvents),
                DisplayedView.Daily => _dayCalendarViewFactory.CreateCalendarView(today, timeDivision, calendarEvents),
                DisplayedView.Monthly => _monthCalendarViewFactory.CreateCalendarView(today, timeDivision, calendarEvents),
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null),
            };
        }

    }
}
