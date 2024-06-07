using BlazorCalendar.Models.ViewModel.Interfaces;
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
builder.Services.AddScoped<ICalendarViewFactory, CalendarViewFactory>();

await builder.Build().RunAsync();
