using System;
using SQLite;

namespace Todo
{
    public class VideoClip
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Path { get; set; }

        public DateTime CaptureTime { get; set; }

        public byte[] Thumbnail { get; set; }

        public override string ToString()
        {
            return Path ?? "undefined";
        }

        public ClipType ClipType { get; set; }
    }



    public enum ClipType
    {
        Walkround,
        WheelAndTyre,
        FrontSeats,
        RearSeats,
        EngineBay,
        FrontEnd,
        RearEnd,
        Dashboard,
        Trunk
    }
}