using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JukeBox.Models
{
  public  class ApiLibraryTypeResponse
    {

        public ObservableCollection<ApiLibraryType> ResponseObject { set; get; }
        public int ResponseType { set; get; }

        public string ResponseMessage { set; get; }
    }
}
