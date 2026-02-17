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

        public DateTime EnsureUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                // Предполагаем, что это местное время и конвертируем
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }

            // Уже UTC
            return dateTime;
        }
    }
}
