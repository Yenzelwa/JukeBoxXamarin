using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public int FK_SingerId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        public string ImageSource { get; set; }
        public Singer Singer { get; set; }


    }
    public class ImageList
    {
       public List<string> image { get; set; }
    }

    public class Singer
    {
        public int Id { get; set; }

        public string FirstName { get;set;}

        public string LastName { get; set; }


    }
}
