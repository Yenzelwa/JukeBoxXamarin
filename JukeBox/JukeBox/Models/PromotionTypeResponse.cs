using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace JukeBox.Models
{
   public class PromotionTypeResponse
    {
        public ObservableCollection<PromotionType> ResponseObject { set; get; }
        public int ResponseType { set; get; }

        public string ResponseMessage { set; get; }
    }
}
