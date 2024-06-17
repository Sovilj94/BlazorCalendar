﻿using BlazorCalendar.Models;
using BlazorCalendar.Models.DayViewModels;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.ViewModel;
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
                    return CreateDayCalendarView(firstDate, timeDivision);
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }

        private ICalendarView CreateWeekCalendarView(DateTime firstDate, TimeDivisionEnum timeDivision)
        {
            // Prvi i zadnji dan u sedmici
            var firstDayOfWeek = _culture.DateTimeFormat.FirstDayOfWeek;
            var firstDateWeek = firstDate.AddDays(-(firstDate.DayOfWeek - firstDayOfWeek));
            var lastDateOfWeek = firstDateWeek.AddDays(6);

            // Inicijalizacija view modela
            WeekCalendarViewModel weekCalendarViewModel = new WeekCalendarViewModel();
            weekCalendarViewModel.DayHeader = new List<DayHeaderViewModel>();
            weekCalendarViewModel.AllDay = new AllDayViewModel();
            weekCalendarViewModel.TimeSideBar = new TimeSideBarViewModel();
            weekCalendarViewModel.DayCalendar = new List<DayViewModel>();
            weekCalendarViewModel.Tasks = _tasksService.GetTasksForWeekViewModel(firstDateWeek, lastDateOfWeek, _tasksService.GetAllTasks());

            // Popunjavanje AllDayViewModela
            weekCalendarViewModel.AllDay.Events = _tasksService.GetTasksForAllDayViewModel((List<ICalendarEvent>)_tasksService.GetAllTasks());
            weekCalendarViewModel.AllDay.FirstDateWeek = firstDateWeek;
            weekCalendarViewModel.AllDay.TimeCells = new List<TimeCellViewModel>();
            weekCalendarViewModel.AllDay.GridItems = new List<GridItemViewModel>();
            weekCalendarViewModel.AllDay.GridItems = _tasksService.GetGridItemsForAllDayComponent(weekCalendarViewModel.AllDay.Events, firstDateWeek);

            // TimeCells
            for (int column = 0; column < 7; column++)
            {
                TimeCellViewModel timeCell = new TimeCellViewModel();
                timeCell.IsAllDayTimesCell = true;
                timeCell.Time = weekCalendarViewModel.AllDay.FirstDateWeek.AddDays(column);
                timeCell.Column = column + 2;
                timeCell.CSSGridPosition = $"grid-row:1 / span 5; grid-column-start:{column + 2}; height:100px; border-right:1px solid #ccc";

                weekCalendarViewModel.AllDay.TimeCells.Add(timeCell);
            }

            // TimeSideBar
            weekCalendarViewModel.TimeSideBar.TimeDivision = new TimeDivision(timeDivision);

            // Popunjavanje DayHeaderViewModel i DayCalendarViewModel
            for (int i = 0; i < 7; i++)
            {
                int d = Dates.GetNumOfDay(i);
                DateTime day = firstDateWeek.AddDays(i);
                var dayHeader = new DayHeaderViewModel
                {
                    DayName = _dayNames[d],
                    DayDate = day.ToString("dd.MM")
                };

                var dayCalendar = new DayViewModel
                {
                    Day = day,
                    TimeDivision = new TimeDivision(timeDivision),
                    GridItems = _tasksService.GetGridItemsForDayComponent(_tasksService.GetTasksForDayViewModel(day, weekCalendarViewModel.Tasks), new TimeDivision(timeDivision).Minutes,day),
                    TimeCells = new List<TimeCellViewModel>()
                };

                if (dayCalendar.GridItems != null && dayCalendar.GridItems.Count != 0)
                {
                    dayCalendar.MaxNumberOfColumns = dayCalendar.GridItems.Max(x => x.ColumnStart);
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

                    timeCell.Row = row;
                    timeCell.ColumnsSpan = dayCalendar.MaxNumberOfColumns;
                    timeCell.CSSGridPosition = $"grid-row:{timeCell.Row}; grid-column:1 / span {timeCell.ColumnsSpan};";
                    timeCell.Time = time;
                    timeCell.CSSbackground = GetBackground(dayCalendar.Day, dayCalendar);

                    dayCalendar.TimeCells.Add(timeCell);
                }

                weekCalendarViewModel.DayHeader.Add(dayHeader);
                weekCalendarViewModel.DayCalendar.Add(dayCalendar);
            }

            return weekCalendarViewModel;
        }


        private ICalendarView CreateDayCalendarView(DateTime date, TimeDivisionEnum timeDivision)
        {
            // Inicijalizacija view modela
            DayCalendarViewModel dayCalendarViewModel = new DayCalendarViewModel();
            dayCalendarViewModel.DayHeaderViewModel = new DDayHeaderViewModel
            {
                DayName = _dayNames[(int)date.DayOfWeek],
                DayDate = date.ToString("dd.MM")
            };

            dayCalendarViewModel.AllDayViewModel = new DAllDayViewModel
            {
                Events = _tasksService.GetTasksForAllDayViewModel(_tasksService.GetAllTasks()),
                FirstDateWeek = date.Date,
                TimeCellViewModel = new DTimeCellViewModel(),
                GridItemsViewModel = new List<DGridItemViewModel>()
            };
            dayCalendarViewModel.AllDayViewModel.GridItemsViewModel = _tasksService.GetGridItemsForDAllDayComponent(dayCalendarViewModel.AllDayViewModel.Events, date.Date);

            // TimeCells for AllDayViewModel
            DTimeCellViewModel allDayTimeCell = new DTimeCellViewModel
            {
                IsAllDayTimesCell = true,
                Time = date,
                Column = 1,
                CSSGridPosition = $";grid-column-start:1; height:100px; border-right:1px solid #ccc"
            };
            dayCalendarViewModel.AllDayViewModel.TimeCellViewModel = allDayTimeCell;

            // TimeSideBar
            dayCalendarViewModel.TimeSideBarViewModel = new DTimeSideBarViewModel
            {
                TimeDivision = new TimeDivision(timeDivision)
            };

            // Main Day View
            dayCalendarViewModel.DayViewModel = new DDayViewModel
            {
                Day = date,
                TimeDivision = new TimeDivision(timeDivision),
                GridItemsViewModel = _tasksService.GetGridItemsForDDayComponent(_tasksService.GetTasksForDayViewModel(date,_tasksService.GetAllTasks()), new TimeDivision(timeDivision).Minutes, date),
                TimeCellsViewModel = new List<DTimeCellViewModel>()
            };

            if (dayCalendarViewModel.DayViewModel.GridItemsViewModel != null && dayCalendarViewModel.DayViewModel.GridItemsViewModel.Count != 0)
            {
                dayCalendarViewModel.DayViewModel.MaxNumberOfColumns = dayCalendarViewModel.DayViewModel.GridItemsViewModel.Max(x => x.ColumnStart);
            }
            else
            {
                dayCalendarViewModel.DayViewModel.MaxNumberOfColumns = 1;
            }

            for (int dayTime = 0; dayTime < dayCalendarViewModel.DayViewModel.TimeDivision.NumberOfCells; dayTime++)
            {
                DateTime time = dayCalendarViewModel.DayViewModel.Day.AddMinutes(dayTime * dayCalendarViewModel.DayViewModel.TimeDivision.Minutes);
                int row = dayTime + 1;

                DTimeCellViewModel timeCell = new DTimeCellViewModel
                {
                    Row = row,
                    ColumnsSpan = dayCalendarViewModel.DayViewModel.MaxNumberOfColumns,
                    CSSGridPosition = $"grid-row:{row}; grid-column:1 / span {dayCalendarViewModel.DayViewModel.MaxNumberOfColumns};",
                    Time = time,
                    CSSbackground = null //GetBackground(dayCalendarViewModel.DayViewModel.Day, dayCalendarViewModel.DayViewModel)
                };

                dayCalendarViewModel.DayViewModel.TimeCellsViewModel.Add(timeCell);
            }

            dayCalendarViewModel.Events = _tasksService.GetAllTasks().Where(e => e.DateStart.Date == date.Date).ToList();

            return dayCalendarViewModel;
        }

        private string GetBackground(DateTime day, DayViewModel dayViewModel)
        {
            int d = (int)day.DayOfWeek;

            if (d == 6)
            {
                return $"background:{dayViewModel.SaturdayColor}";
            }
            else if (d == 0)
            {
                return $"background:{dayViewModel.SundayColor}";
            }

            return $"background:{dayViewModel.WeekDaysColor}";
        }
    }
}
