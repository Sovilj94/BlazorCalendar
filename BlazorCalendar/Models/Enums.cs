namespace BlazorCalendar;

using System.ComponentModel;

public enum PriorityLabel
{
    Code = 0,
    Caption = 1
}

public enum DisplayedView
{
    Annual = 0,
    Monthly = 1,
    Weekly = 2,
    Daily = 3
}

public enum FillStyleEnum
{
    [Description("fill")]
    Fill = 0,
    [Description("backwardDiagonal")]
    BackwardDiagonal = 1,
    [Description("zigZag")]
    ZigZag = 2,
    [Description("triangles")]
    Triangles = 3,
    [Description("crossDots")]
    CrossDots = 4
}

public enum TimeDivision
{
    TwoHours = 0,
    Hour = 1,
    ThirtyMinutes = 2,
    FifteenMinutes = 3,
}