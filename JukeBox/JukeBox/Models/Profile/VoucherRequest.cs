using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Models.Profile
{
   public class VoucherRequest
    {
        public string VoucherPin { get; set; }
        public long CustomerId { get; set; }
    }
}
