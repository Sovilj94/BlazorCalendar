namespace BlazorCalendar.Models.Interfaces
{
    public interface ICalendarPositionCalculator<TViewModel> where TViewModel : ICalendarView
    {
        void CalculatePositions(TViewModel viewModel, DateTime firstDate, int? offsetCell, List<ICalendarEvent> eventList);
    }
}
