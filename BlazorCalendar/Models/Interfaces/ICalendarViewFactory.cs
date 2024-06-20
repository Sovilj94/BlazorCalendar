using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCalendar.Models.Interfaces
{
    public interface ICalendarViewFactory
    {
        ICalendarView CreateCalendarView(DateTime firstDate, TimeDivisionEnum timeDivision);
    }
}
