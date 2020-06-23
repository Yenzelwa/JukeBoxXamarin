using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JukeBox.Models.Profile
{
  public  class SongShuffleRepeat
    {
        [PrimaryKey , AutoIncrement]
        public int ShuffleRepeatId { get; set; }
        public bool Shuffle { get; set; }
        public bool Repeat { get; set; }
    }
}
