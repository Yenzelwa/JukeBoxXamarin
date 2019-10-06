using System;

namespace JukeBox.Models
{
    public class Person
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }

        //    public Person()
        //    {
        //        this.Code = DateTime.Now.ToString("hhmmssffffff");
        //        this.Name = "Mr. A";
        //        this.Address = "Singapore";
        //        this.IsVisible = true;
        //    }
        //}

        //public class PeoplePair : Tuple<Person, Person>
        //{
        //    public PeoplePair(Person item1, Person item2)
        //        : base(item1, item2 ?? CreateEmptyModel()) { }

        //    private static Person CreateEmptyModel()
        //    {
        //        return new Person { IsVisible = true };
        //    }
        //}
    }
}