using System;

namespace API.Extensions
{
    public static class DateOnlyExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.CompareTo(today.AddYears(-age)) > 0)
                age--;
            return age;
        }
    }
}
