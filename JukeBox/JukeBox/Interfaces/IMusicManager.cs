using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JukeBox.Models;

namespace JukeBox.Interfaces
{
    public interface IMusicManager
    {
        void Init(Action<bool> IsPlaying, Action<double> GetSongPos, Action<int> GetQueuePos, Action<IList<Song>> GetQueue);

        Task SetQueue(IList<Song> songs);

        void StartQueue(IList<Song> songs, int pos);

        void Start(int pos);

        void Play();

        void Pause();

        void Next();

        void Prev();

        void Shuffle();

        void Seek(double position);

        void ClearQueue();

        void AddToEndOfQueue(IList<Song> songs);

        void PlayNext(Song song);
    }
}
