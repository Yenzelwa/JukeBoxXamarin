using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JukeBox.Models
{
    public class PromotionResult
    {
        public int PromoNumber { get; set; }
        public int? PromotionTypeId { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string ArtistImage { get; set; }
        public int? NumberOfVotes { get; set; }
        public int? Duration { get; set; }
        public double ProgressPosition
        {
            get
            {
                double ret = 0;
                if (Duration > 0)
                {
                    ret = (double)NumberOfVotes / (double)Duration;
                }

                return ret;
            }
        }
    }
}