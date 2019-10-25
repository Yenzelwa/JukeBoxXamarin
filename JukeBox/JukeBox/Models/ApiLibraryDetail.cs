using JukeBox.ViewModels;
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
        public decimal Price { get; set; }
        public bool SongDownload { get; set; }
        public DateTime DateCreated { get; set; }

    
        public string SinglePurchase
        {
            get
            {
                if (SongDownload == true)
                {
                    return "Download";
                }
                else
                {
                    decimal price = Math.Round(Price, 2);
                    return  "R " + price.ToString();
                }
            }
        }
    }
}
