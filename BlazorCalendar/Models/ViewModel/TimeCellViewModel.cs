using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCalendar.Models.ViewModel
{
    public class TimeCellViewModel
    {
        public string CSSGridPosition { get; set; }
        public string CSSbackground { get; set; }
        public DateTime Time { get; set; }
    }
}
