using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JukeBox.Models
{
    public class PromotionType
    {
        public int PromotionTypeId { get; set; }
        public string PromotionTypeName { get; set; }
        public string PromotionImage { get; set; }
        public decimal? Amount { get; set; }
        public string AmountFormat => string.Format("Vote Costs R {0}", Math.Round(Amount ?? 0, 2));
    }
}