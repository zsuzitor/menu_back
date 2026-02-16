using System;

namespace BL.Models.Services.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDateTime();
        DateTime EnsureUtc(DateTime dateTime);
    }
}
