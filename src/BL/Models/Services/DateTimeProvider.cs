using BL.Models.Services.Interfaces;
using System;

namespace BL.Models.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
