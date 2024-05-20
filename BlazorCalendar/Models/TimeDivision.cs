using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCalendar.Models
{
    public class TimeDivision
    {
        public int Minutes { get; set; }
        public int NumberOfCells { get; set; }

        public TimeDivision(TimeDivisionEnum timeDivision)
        {
            switch (timeDivision)
            {
                case TimeDivisionEnum.Hour:
                    Minutes = 60;
                    NumberOfCells = 24;
                    break;
                case TimeDivisionEnum.FifteenMinutes:
                    Minutes = 15;
                    NumberOfCells = 96;
                    break;
                case TimeDivisionEnum.ThirtyMinutes:
                    Minutes = 30;
                    NumberOfCells = 48;
                    break;
                case TimeDivisionEnum.TwoHours:
                    Minutes = 120;
                    NumberOfCells = 12; // Adjust if you need another multiplier value
                    break;
                default:
                    Minutes = 60;
                    NumberOfCells = 24;
                    break;
            }
        }
    }
}
