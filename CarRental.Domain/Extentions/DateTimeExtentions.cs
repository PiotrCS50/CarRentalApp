using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Extentions
{
    public static class DateTimeExtentions
    {
        public static bool Rented(this DateTime date)
        {
            DateTime now = DateTime.Today;
            if (now >= date)
                return true;
            return false;
        }
    }
}
