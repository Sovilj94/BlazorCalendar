using BlazorCalendar.Models;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.MonthViewModels;
using System.Globalization;

namespace BlazorCalendar.FactoryClasses
{
    public class MonthCalendarViewFactory : ICalendarViewFactory
    {
        private readonly string[] _dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
        public string WeekDaysColor { get; set; } = "#FFF";
        public string SaturdayColor { get; set; } = "#ECF4FD";
        public string SundayColor { get; set; } = "#DBE7F8";
        public bool HighlightToday { get; set; } = true;

        private readonly ICalendarPositionCalculator<MonthCalendarViewModel> CalendarPositionCalculator;

        public MonthCalendarViewFactory(ICalendarPositionCalculator<MonthCalendarViewModel> calendarPositionCalculator)
        {
            CalendarPositionCalculator = calendarPositionCalculator;
        }

        public ICalendarView CreateCalendarView(DateTime date, TimeDivisionEnum timeDivision, List<ICalendarEvent> calendarEvents)
        {

            DateTime today = DateTime.Today;
            DateTime firstDate = new DateTime(date.Year, date.Month, 1);
            int firstDayWeek = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek;
            DateTime dayCounter = new DateTime(date.Year, date.Month, 1);
            DateTime lastDay = dayCounter.AddMonths(1).AddDays(-1);
            int OffsetCell = 0;

            MonthCalendarViewModel MonthViewModel = new MonthCalendarViewModel();
            MonthViewModel.MonthHeaderViewModels = new List<MonthDayHeaderViewModel>();
            MonthViewModel.MonthTimeCellViewModels = new List<MonthTimeCellViewModel>();
            MonthViewModel.MonthGridItemViewModels = new List<MonthGridItemViewModel>();

            MonthViewModel.CalendarEvents = calendarEvents;
            MonthViewModel.Date = date;

            for (var i = 0; i < 7; i++)
            {
                var d = Dates.GetNumOfDay(i);

                var monthDayHeader = new MonthDayHeaderViewModel
                {
                    DayName = _dayNames[d]
                };

                MonthViewModel.MonthHeaderViewModels.Add(monthDayHeader);
            }

            StateCase state = StateCase.Before;

            for (int i = 0; i < 42; i++)
            {
                MonthTimeCellViewModel cell = new MonthTimeCellViewModel();

                if (state == StateCase.Before)
                {
                    if (firstDayWeek == Dates.GetNumOfDay(i))
                    {
                        state = StateCase.InMonth;
                        cell.DayCounter = dayCounter;
                        cell.CSSbackground = GetBackground(dayCounter);
                        cell.CSSClass = "fade-in monthly-day noselect";
                        MonthViewModel.MonthTimeCellViewModels.Add(cell);
                        dayCounter = dayCounter.AddDays(1);
                        OffsetCell = i;
                    }
                    else
                    {
                        cell.CSSClass = "monthly-day monthly-day--disabled cursor-top";
                        MonthViewModel.MonthTimeCellViewModels.Add(cell);
                    }
                }
                else if (state == StateCase.InMonth)
                {
                    if (dayCounter > lastDay)
                    {
                        state = StateCase.After;
                        cell.CSSClass = "monthly-day monthly-day--disabled cursor-bottom";
                        MonthViewModel.MonthTimeCellViewModels.Add(cell);
                    }
                    else
                    {
                        cell.DayCounter = dayCounter;
                        cell.CSSbackground = GetBackground(dayCounter);
                        cell.CSSClass = "fade-in monthly-day noselect";
                        if (HighlightToday && dayCounter == today)
                        {
                            cell.CSSClass += " monthly-today";
                        }
                        MonthViewModel.MonthTimeCellViewModels.Add(cell);
                        dayCounter = dayCounter.AddDays(1);
                    }
                }
                else if (state == StateCase.After)
                {
                    cell.CSSClass = "monthly-day monthly-day--disabled cursor-bottom";
                    MonthViewModel.MonthTimeCellViewModels.Add(cell);
                }
            }

            // Populate MonthGridItemViewModels

            CalendarPositionCalculator.CalculatePositions(MonthViewModel, firstDate, OffsetCell, calendarEvents);

            return MonthViewModel;
        }
        private string GetBackground(DateTime day)
        {
            int d = (int)day.DayOfWeek;

            if (d == 6)
            {
                return $"background:{SaturdayColor}";
            }
            else if (d == 0)
            {
                return $"background:{SundayColor}";
            }

            return $"background:{WeekDaysColor}";
        }
    }


}
