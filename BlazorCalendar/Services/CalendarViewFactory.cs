using BlazorCalendar.CalendarComponenets;
using BlazorCalendar.Models;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.ViewModel;
using System.Data.Common;
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
                    DayTasks = _tasksService.GetTasksForDayViewModel(day, weekCalendarViewModel.Tasks, timeDivision),
                    TimeCells = new List<TimeCellViewModel>()
                };

                if (dayCalendar.DayTasks != null && dayCalendar.DayTasks.Count != 0)
                {
                    dayCalendar.MaxNumberOfColumns = dayCalendar.DayTasks.Max(x => x.ColumnStart);
                }
                else
                {
                    dayCalendar.MaxNumberOfColumns = 1;
                }
                for (int dayTime = 0; dayTime < dayCalendar.TimeDivision.NumberOfCells; dayTime++)
                {
                    DateTime time = dayCalendar.Day.AddMinutes(dayTime * dayCalendar.TimeDivision.Minutes);
                    int row = dayTime + 1;

                    TimeCellViewModel timeCell = new TimeCellViewModel();

                    timeCell = new TimeCellViewModel();
                    timeCell.CSSGridPosition = $"grid-row:{row}; grid-column:1 / span {dayCalendar.MaxNumberOfColumns};";
                    timeCell.Time = time;
                    timeCell.CSSbackground = GetBackground(dayCalendar.Day, dayCalendar);

                    dayCalendar.TimeCells.Add(timeCell);
                }


                weekCalendarViewModel.DayHeader.Add(dayHeader);
                weekCalendarViewModel.DayCalendar.Add(dayCalendar);
            }

            weekCalendarViewModel.AllDay.Tasks = _tasksService.GetAllDayTaskPositionForDayGrid(_tasksService.GetAllTasks(), firstDateWeek);
            weekCalendarViewModel.AllDay.FirstDateWeek = firstDateWeek;
            weekCalendarViewModel.AllDay.TimeCells = new List<TimeCellViewModel>();
            weekCalendarViewModel.AllDay.GridItems = new List<GridItemViewModel>();
            weekCalendarViewModel.AllDay.GridItems = _tasksService.GetGridItemsForAllDayComponent(weekCalendarViewModel.AllDay.Tasks, firstDateWeek);

            for (int column = 0; column < 7; column++)
            {
                TimeCellViewModel timeCell = new TimeCellViewModel();
                timeCell.IsAllDayTimesCell = true;
                timeCell.Time = weekCalendarViewModel.AllDay.FirstDateWeek.AddDays(column);
                timeCell.CSSGridPosition = $"grid-row:1 / span 5; grid-column-start:{column + 2}; height:100px; border-right:1px solid #ccc";

                weekCalendarViewModel.AllDay.TimeCells.Add(timeCell);
            }

            weekCalendarViewModel.TimeSideBar.TimeDivision = new TimeDivision(timeDivision);

            return weekCalendarViewModel;
        }
        private string GetBackground(DateTime day, DayCalendarViewModel dayCalendarViewModel)
        {
            int d = (int)day.DayOfWeek;

            if (d == 6)
            {
                return $"background:{dayCalendarViewModel.SaturdayColor}";
            }
            else if (d == 0)
            {
                return $"background:{dayCalendarViewModel.SundayColor}";
            }

            return $"background:{dayCalendarViewModel.WeekDaysColor}";
        }
    }
}
