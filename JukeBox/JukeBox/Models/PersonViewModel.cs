using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JukeBox.Models
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public PersonViewModel()
        {
            //Create mock data
            People = new List<Person>(); 
            People.Add(new Person
            {
                Code = "hhmmss",
                Name = "Mr. A",
                Address = "Singapore",
                Image = "IMG_0078.JPG"
            });
      
            People.Add(new Person
            {
                Code = "hhmmss",
                Name = "Mr. B",
                Address = "Singapore",
                Image = "IMG_0308.JPG"
            });
            People.Add(new Person
            {
                Code = "hhmmss",
                Name = "Mr. C",
                Address = "Singapore",
                Image = "IMG_0437.JPG"
            });
            People.Add(new Person
            {
                Code = "hhmmss",
                Name = "Mr. D",
                Address = "Singapore",
                Image = "IMG_0074.JPG"
            });
            //Add an empty data
        }

        public List<Person> People { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}