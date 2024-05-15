using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorCalendar
{
    partial class DailyView : CalendarBase
    {
        [CascadingParameter(Name = "TimeDivision")]
        public TimeDivision TimeDivision { get; set; }

        [CascadingParameter(Name = "SelectedView")]
        public DisplayedView DisplayedView { get; set; } = DisplayedView.Daily;

        private DateTime _firstDate;

        [CascadingParameter(Name = "FirstDate")]
        public DateTime FirstDate
        {
            get
            {
                if (_firstDate == DateTime.MinValue) _firstDate = DateTime.Today;
                return _firstDate.Date;
            }
            set
            {
                _firstDate = value;
            }
        }

        [CascadingParameter(Name = "TasksList")]
        public Tasks[]? TasksList { get; set; }

        [Parameter]
        public PriorityLabel PriorityDisplay { get; set; } = PriorityLabel.Code;

        [Parameter]
        public bool HighlightToday { get; set; } = false;

        [Parameter]
        public EventCallback<int> OutsideCurrentMonthClick { get; set; }

        [Parameter]
        public EventCallback<ClickEmptyDayParameter> DayClick { get; set; }

        [Parameter]
        public EventCallback<ClickEmptyDayParameter> EmptyDayClick { get; set; }

        [Parameter]
        public EventCallback<ClickTaskParameter> TaskClick { get; set; }

        [Parameter]
        public EventCallback<DragDropParameter> DragStart { get; set; }

        [Parameter]
        public EventCallback<DragDropParameter> DropTask { get; set; }

        private Tasks? TaskDragged;

        private async Task HandleDragStart(int taskID)
        {
            TaskDragged = new Tasks()
            {
                ID = taskID
            };

            DragDropParameter dragDropParameter = new()
            {
                taskID = TaskDragged.ID
            };

            await DragStart.InvokeAsync(dragDropParameter);
        }

        private async Task HandleDayOnDrop(DateTime day)
        {
            if (!Draggable)
                return;

            if (TaskDragged is null)
                return;

            DragDropParameter dragDropParameter = new()
            {
                Day = day,
                taskID = TaskDragged.ID
            };

            await DropTask.InvokeAsync(dragDropParameter);

            TaskDragged = null;
        }

        private async Task ClickDayInternal(MouseEventArgs e, DateTime day)
        {
            if (!DayClick.HasDelegate)
                return;

            ClickEmptyDayParameter clickEmptyDayParameter = new()
            {
                Day = day,
                X = e.ClientX,


                Y = e.ClientY
            };

            await DayClick.InvokeAsync(clickEmptyDayParameter);
        }

        private async Task ClickTaskInternal(MouseEventArgs e, int taskID, DateTime day)
        {
            if (!TaskClick.HasDelegate)
                return;

            List<int> listID = new()
            {
                taskID
            };

            ClickTaskParameter clickTaskParameter = new()
            {
                IDList = listID,
                X = e.ClientX,
                Y = e.ClientY,
                Day = day
            };

            await TaskClick.InvokeAsync(clickTaskParameter);
        }

        public List<Tasks> GetTasksWithPosition(Tasks[] tasks, DateTime date)
        {
            List<Tasks> tasksForDate = tasks.Where(task => task.DateStart.Date == date.Date || task.DateEnd.Date == date.Date).OrderBy(x => x.DateStart).ThenBy(x => x.DateEnd).ToList();

            // Iterate over each task
            for (int i = 0; i < tasksForDate.Count; i++)
            {
                // Get the current task
                Tasks currentTask = tasksForDate[i];

                if (i == 0)
                {
                    currentTask.ColumnStart = 2;
                }

                // Iterate over the remaining tasks to compare with the current task
                for (int j = i + 1; j < tasksForDate.Count; j++)
                {
                    Tasks nextTask = tasksForDate[j];
                        
                    nextTask.ColumnStart = currentTask.ColumnStart + 1;
                }
            }

            return tasksForDate;
        }

        public bool IsTaskTakingWholeDay(DateTime date, Tasks task)
        {
            // Check if task starts at 00:00 and ends at 00:00 the next day
            if (task.DateStart.Hour == 0 && task.DateStart.Minute == 0 &&
                task.DateEnd.Hour == 0 && task.DateEnd.Minute == 0 &&
                task.DateStart.Date == date.Date &&
                task.DateEnd.Date == date.Date.AddDays(1))
            {
                return true;
            }

            return false;
        }

    }
}
