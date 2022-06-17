using System;
using CoreLocation;
using Google.Maps;
using Google.Maps.Utility;
using UIKit;

namespace GoogleMapSample.Models
{
    public class Person : Marker, IClusterItem
    {
        public string imageUrl { get; set; }
        public UIImage cacheImage { get; set; }

        public Person(CLLocationCoordinate2D Position, string imageUrl)
        {
            this.Position = Position;
            this.imageUrl = imageUrl;
        }
    }
}
