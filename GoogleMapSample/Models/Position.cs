using System;
using Newtonsoft.Json;

namespace GoogleMapSample.Models
{
    public class Position
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }
    }
}
