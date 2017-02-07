using System.Collections.Generic;

namespace Todo.Models
{
    public class VehicleCapture
    {
        public string VRM { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int OdometerReading { get; set; }

        public OdoType OdoType { get; set; }

        public bool DataValidatedFromLookupService { get; set; }

        public List<VideoClip> VideoClips { get; set; }
    }

    public enum OdoType
    {
        Miles,
        Kilometers
    }
}