using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models
{
    public class ApiLibraryDetail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public decimal? Price { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
