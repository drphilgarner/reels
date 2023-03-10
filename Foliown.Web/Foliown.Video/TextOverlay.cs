namespace Foliown.Video
{
    public class TextOverlay
    {
        public string Text { get; set; }

        public double Timecode { get; set; }

        public double Duration { get; set; }

        public string FontPath { get; set; }

        public double XPos { get; set; }

        public double YPos { get; set; }

        public int FontSize { get; set; }

        public string FontColor { get; set; } //https://ffmpeg.org/ffmpeg-utils.html#Color
    }
}