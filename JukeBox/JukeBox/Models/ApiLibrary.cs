
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JukeBox.Models
{
     public  class ApiLibrary
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverFilePath { get; set; }
        public string FilePath { get; set; }
        public decimal? Price { get; set; }
        public string Type { get; set; }
        public string Artist { get; set; }
        public bool AlbumDownload { get; set; }
        public DateTime DateCreated { get; set; }
        public ImageSource AlbumPicture => ImageSource.FromUri(new Uri(CoverFilePath));

        public string Purchase
        {
            get
            {
                if (AlbumDownload == true)
                {
                    return "Download";
                }
                else
                {
                     return "Purchase";
                }
            }
        }
        public string PriceFormat => string.Format("R {0}", Math.Round(Price ?? 0, 2));
            
        
    }
}
