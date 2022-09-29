
#region Using

using pbAd.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace pbAd.Web.Infrastructure.Helpers
{
    public static class DropdownListHelper
    {
        public static IEnumerable<SelectListItem> GetDropdownList(DropdownListTypes type, string selected = "")
        {
            var items = new List<ConstantDropdownItem>();

            switch (type)
            {
                case DropdownListTypes.PrivateAdTypes:
                    items = PrivateAdTypesConstants.Items.ToList();
                    break;
            }

            return items.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = !string.IsNullOrWhiteSpace(selected) ? selected == i.Value : i.Selected
                }).ToList();
        }

        public static IEnumerable<ConstantDropdownItem> GetCreditCardYears()
        {
            var years = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 15; i++)
            {
                var year = Convert.ToString(DateTime.Now.Year + i);
                years.Add(new ConstantDropdownItem
                {
                    Text = year,
                    Value = year,
                });
            }
            return years;
        }

        public static IEnumerable<ConstantDropdownItem> GetCreditCardMonths()
        {
            var months = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 12; i++)
            {
                var text = (i < 10)
                                  ? "0" + i.ToString(CultureInfo.InvariantCulture)
                                  : i.ToString(CultureInfo.InvariantCulture);

                months.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = i.ToString(CultureInfo.InvariantCulture),
                });
            }

            return months;
        }

        public static IEnumerable<ConstantDropdownItem> GetNumbersList()
        {
            var items = new List<ConstantDropdownItem>();

            for (var i = 1; i <= 31; i++)
            {
                var text = i.ToString(CultureInfo.InvariantCulture);

                items.Add(new ConstantDropdownItem
                {
                    Text = text,
                    Value = text,
                });
            }

            return items;
        }
    }
}