using BlazorCalendar.Models;
using System;

public interface ITaskDraggedService
{
    Tasks TaskDragged { get; set; }
    event Action TasksDraggedChanged;
}

public class TaskDraggedService : ITaskDraggedService
{
    private Tasks? _taskDragged;

    public Tasks TaskDragged
    {
        get => _taskDragged;
        set
        {
            _taskDragged = value;
            TasksDraggedChanged?.Invoke();
        }
    }

    public event Action? TasksDraggedChanged;
}