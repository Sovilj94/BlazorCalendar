using BlazorCalendar.Models;
using BlazorCalendar.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCalendar.Services
{
    public class PopulateViewModelService
    {
        public List<DayHeaderViewModel> GetDayHeaders()
        {
            return new List<DayHeaderViewModel>();
        }

    }
}
