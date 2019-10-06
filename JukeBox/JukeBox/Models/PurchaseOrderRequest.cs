using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Models
{
  public  class PurchaseOrderRequest
    {

        public long LibraryId { get; set; }
        public long LibraryDetailId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
    }
}
