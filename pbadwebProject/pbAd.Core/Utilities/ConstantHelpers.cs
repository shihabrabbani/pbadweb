using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pbAd.Core.Utilities
{
    public static class ConstantHelpers
    {
        public static string FindItemInList(this IEnumerable<ConstantDropdownItem> items, string searchTerm)
        {
            var item = items.FirstOrDefault(i => i.Value == searchTerm);
            return item == null ? "N/A" : item.Text;
        }

        /// <summary>
        /// Get random string from date time, lengh must be between 1 to 12
        /// otherwise it'll return empty string
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomNumber(int length = 12)
        {
            string number = string.Empty;

            var date = DateTime.UtcNow.AddHours(6);
            number = date.ToString("yy") + date.Month.ToString("d2") + date.Day.ToString("d2") + date.Hour.ToString("d2") + date.Minute.ToString("d2") + date.Second.ToString("d2");// + Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);
            if (number.Length > length)
            {
                number = number.Substring(0, length);
            }
            return number;
        } 
        
        public static string GetSixDigitRandomNumber()
        {
            string number = string.Empty;
            Random generator = new Random();
            number = generator.Next(0, 1000000).ToString("D6");
            return number;
        }
    }
}
