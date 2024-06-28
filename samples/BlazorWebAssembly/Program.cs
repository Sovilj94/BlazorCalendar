using BlazorCalendar.FactoryClasses;
using BlazorCalendar.FactoryClasses.CalculatePosition;
using BlazorCalendar.Models.Interfaces;
using BlazorCalendar.Models.MonthViewModels;
using BlazorCalendar.Services;
using BlazorWebAssembly;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITaskDraggedService, TaskDraggedService>();
builder.Services.AddScoped<TasksService, TasksService>();

builder.Services.AddTransient<MonthCalendarViewFactory, MonthCalendarViewFactory>();
builder.Services.AddTransient<WeekCalendarViewFactory, WeekCalendarViewFactory>();
builder.Services.AddTransient<DayCalendarViewFactory, DayCalendarViewFactory>();

builder.Services.AddTransient<CalendarViewFactoryProvider>();

builder.Services.AddScoped<ICalendarPositionCalculator<MonthCalendarViewModel>, MonthPositionCalculator>();

await builder.Build().RunAsync();
