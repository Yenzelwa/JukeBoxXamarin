using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models
{
   public class PromotionResultRequest
    {
        public int PromotionTypeId { get; set; }
        public int ClientId { get; set; }
        public int Customer { get; set; }
    }
}
