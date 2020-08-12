using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JukeBox.Models
{
    public class PromotionCategory
    {
        public int PromotionCategoryId { get; set; }
        public int PromotionTypeId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
    }
}