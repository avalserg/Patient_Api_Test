using PatientLibrary.Models;
using System.Globalization;

namespace Patient_Api_Test.Helpers
{
    /// <summary>
    /// Store logic for filtering request
    /// </summary>
    public static class FiltrationHelpers
    {

        public static string? ValidateSearchTemplateByBirthDate(string? searchTemplateByBirthDate)
        {

            //check for searchTemplateByBirthDate, it should be have at least {eq|ne|gt|lt|ge|le|ap}{year} data
            var dateValue =
                (searchTemplateByBirthDate != null && searchTemplateByBirthDate.Length >= 6)
                    ? searchTemplateByBirthDate.Substring(2)
                    : null;

            return dateValue;
        }
        public static DateTime? ParseBirthDate(string? dateValue)
        {
            // hours, minutes and seconds does not support!!!
            string[] formats = { "yyyy-MM-dd", "yyyy", "yyyy-MM" };

            if (!DateTime.TryParseExact(dateValue, formats, CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out DateTime parsedDate))
            {
                return null;
            }

            return parsedDate;
        }

        /// <summary>
        /// Main filtration after get correct date  and prefix
        /// </summary>
        /// <param name="prefixFilter"></param>
        /// <param name="parsedDate"></param>
        /// <param name="filteredPatients"></param>
        /// <returns></returns>
        public static IQueryable<Patient> FiltrationByPrefixAndDate(string prefixFilter, DateTime? parsedDate, IQueryable<Patient> filteredPatients)
        {
            var universalDate = parsedDate.Value.ToUniversalTime().Date;

            switch (prefixFilter)
            {
                case "gt":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date > universalDate);
                    break;
                case "lt":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date < universalDate);
                    break;
                case "ge":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date >= universalDate);
                    break;
                case "le":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date <= universalDate);
                    break;
                case "ne":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date != universalDate);
                    break;
                case "sa":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date <= universalDate);
                    break;
                case "ap":
                    // +- include 10 Days at search date
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date >= universalDate.Subtract(new TimeSpan(10)) && p.BirthDate.Value.Date <= universalDate.AddDays(10));
                    break;
                case "eq":
                    filteredPatients = filteredPatients.Where(p => p.BirthDate.Value.Date == universalDate);
                    break;
            }

            return filteredPatients;
        }
    }
}
