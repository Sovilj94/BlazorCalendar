namespace BlazorCalendar;

using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

partial class WeekViewNew : CalendarBase
{
    public int Minutes { get; set; }
    public decimal Multiplyer { get; set; }
    public int NumberOfDays { get; set; } = 7;

    public int DayNumber { get; set; } = 0;

    [CascadingParameter(Name = "TimeDivisionEnum")]
    public TimeDivisionEnum TimeDivisionEnum { get; set; }

    [CascadingParameter(Name = "SelectedView")]
    public DisplayedView DisplayedView { get; set; } = DisplayedView.Weekly;

    private DateTime _firstdate;
    [CascadingParameter(Name = "FirstDate")]
    public DateTime FirstDate
    {
        get
        {
            if (_firstdate == DateTime.MinValue) _firstdate = DateTime.Today;
            return _firstdate.Date;
        }
        set
        {
            _firstdate = value;
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
    private async Task HandleDragStart(DragDropParameter dragDropParameter)
    {
        TaskDragged = new Tasks()
        {
            ID = dragDropParameter.taskID
        };

        await DragStart.InvokeAsync(dragDropParameter);
    }

    private async Task HandleDayOnDrop(DragDropParameter dragDropParameter)
    {
        if (!Draggable) 
            return;

        if (TaskDragged is null) 
            return;

        await DropTask.InvokeAsync(dragDropParameter);

        TaskDragged = null;
    }
    private async Task ClickDayInternal(ClickEmptyDayParameter clickEmptyDayParameter)
    {
        if (!DayClick.HasDelegate)
            return;

        await DayClick.InvokeAsync(clickEmptyDayParameter);
    }

    private async Task ClickTaskInternal(ClickTaskParameter clickTaskParameter)
    {
        if (!TaskClick.HasDelegate)
            return;

        await TaskClick.InvokeAsync(clickTaskParameter);
    }
}
