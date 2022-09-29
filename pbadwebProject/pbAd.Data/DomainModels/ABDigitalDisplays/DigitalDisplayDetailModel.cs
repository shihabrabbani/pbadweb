using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.ABDigitalDisplays
{
    public class DigitalDisplayDetailModel
    {
        public int DigitalPageId { get; set; }
        public string DigitalPageName { get; set; }

        public int DigitalPagePositionId { get; set; }
        public string PagePositionName { get; set; }
        public int DigitalAdUnitTypeId { get; set; }
        public string UnitName { get; set; }

        public int AdQty { get; set; }

        public int PerUnitRate { get; set; }       
        public string PublishDateStart { get; set; }       
        public string PublishTimeStart { get; set; }       
        public string PublishDateEnd { get; set; }      
        public string PublishTimeEnd { get; set; }
    }
}
