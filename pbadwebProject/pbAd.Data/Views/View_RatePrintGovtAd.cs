using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Views
{
    [Table("View_RatePrintGovtAd")]
    public class View_RatePrintGovtAd
    {

        [Key]

        public int AutoId { get; set; }
        public decimal CorpColorRate { get; set; }
        public decimal CorpBWRate { get; set; }
        public decimal PerColumnInchColorRate { get; set; }
        public decimal PerColumnInchBWRate { get; set; }

        public int EditionId { get; set; }
        public int EditionPageId { get; set; }
        public string EditionName { get; set; }
        public string EditionPageName { get; set; }
        public int EditionPageNo { get; set; }
    }
}
