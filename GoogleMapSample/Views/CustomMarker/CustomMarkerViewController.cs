using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utility;
using GoogleMapSample.Models;
using UIKit;

namespace GoogleMapSample.Views.CustomMarker
{
    public partial class CustomMarkerViewController : UIViewController, IClusterRendererDelegate
    {
        private double cameraLatitude = -33.8;
        private double cameraLongitude = 151.2;
        private int imageDimension = 30;
        private MapView mapView;
        private ClusterManager clusterManager;

        public CustomMarkerViewController() : base("CustomMarkerViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void LoadView()
        {
            base.LoadView();
            SetMapView();
            SetClusterManager();
        }

        private void SetMapView()
        {
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 10);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
        }

        private void SetClusterManager()
        {
            var iconGenerator = new DefaultClusterIconGenerator();
            var algorithm = new NonHierarchicalDistanceBasedAlgorithm();
            var renderer = new ClusterRenderer(mapView, iconGenerator) { Delegate = this };
            clusterManager = new ClusterManager(mapView, algorithm, renderer);
            RandomPeople();
            clusterManager.Cluster();
        }

        private void RandomPeople()
        {
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.8, 151.2), "https://c1.staticflickr.com/5/4125/5036248253_e405cc6961_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.82, 151.1), "https://c2.staticflickr.com/2/1350/4726917149_2a7e7c579e_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.9, 151.15), "https://c2.staticflickr.com/4/3101/3111525394_737eaf0dfd_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.91, 151.05), "https://c2.staticflickr.com/4/3288/2887433330_7e7ed360b1_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.7, 151.06), "https://c1.staticflickr.com/3/2405/2179915182_5a0ac98b49_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-33.5, 151.18), "https://c1.staticflickr.com/9/8035/7893552556_3351c8a168_s.jpg"));
            clusterManager.AddItem(new Person(new CLLocationCoordinate2D(-34.0, 151.18), "https://c1.staticflickr.com/5/4125/5036231225_549f804980_s.jpg"));
        }

        [Export("renderer:willRenderMarker:")]
        public void WillRenderMarker(IClusterRenderer renderer, Marker marker)
        {
            if (marker.UserData is Person)
            {
                marker.Title = ((Person)marker.UserData).Title;
                marker.Icon = ImageForItem(((Person)marker.UserData));
                marker.GroundAnchor = new CGPoint(0.5, 0.5);
            }
            else if (marker.UserData is StaticCluster)
            {
                marker.Icon = ImageForCluster(marker.UserData as StaticCluster);
            }
        }

        private UIImage ImageForItem(Person person)
        {
            if (person.cacheImage == null)
            {
                person.cacheImage = ImageWithContentsOfURL(person.imageUrl, new CGSize(imageDimension, imageDimension));
            }
            return person.cacheImage;
        }

        private UIImage ImageWithContentsOfURL(string url, CGSize size)
        {
            NSData data = NSData.FromUrl(new NSUrl(url));
            UIImage image = UIImage.LoadFromData(data);
            UIGraphics.BeginImageContextWithOptions(size, true, 0);
            image.Draw(new CGRect(0, 0, size.Width, size.Height));
            UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }

        private UIImage HalfOfImage(UIImage image)
        {
            nfloat scale = image.CurrentScale;
            nfloat width = image.Size.Width * scale;
            nfloat height = image.Size.Height * scale;
            CGRect rect = new CGRect(width / 4, 0, width / 2, height);
            CGImage imageRef = image.CGImage.WithImageInRect(rect);
            UIImage newImage = UIImage.FromImage(imageRef, scale, image.Orientation);
            return newImage;
        }

        private UIImage ImageFromImages(List<UIImage> images, CGSize size)
        {
            if (images.Count <= 1)
                return images.FirstOrDefault();
            UIGraphics.BeginImageContextWithOptions(size, true, 0);
            if (images.Count == 2 || images.Count == 3)
                images[0].Draw(new CGRect(-size.Width / 4, 0, size.Width, size.Height));
            if (images.Count == 2)
            {
                UIImage halfOfImage = HalfOfImage(images[1]);
                halfOfImage.Draw(new CGRect(size.Width / 2, 0, size.Width, size.Height));
            }
            else
            {
                images[1].Draw(new CGRect(size.Width / 2, 0, size.Width, size.Height));
                images[2].Draw(new CGRect(size.Width / 2, size.Height / 2, size.Width / 2, size.Height / 2));
            }
            if (images.Count >= 4)
            {
                images[0].Draw(new CGRect(0, 0, size.Width / 2, size.Height / 2));
                images[3].Draw(new CGRect(0, size.Height / 2, size.Width / 2, size.Height / 2));
            }
            UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }

        private UIImage ImageForCluster(StaticCluster cluster)
        {
            var items = cluster.Items;
            List<UIImage> images = new List<UIImage>();
            for (int i = 0; i < items.Count(); i++)
            {
                images.Add(ImageForItem(items[i] as Person));
                if (i >= 4)
                    break;
            }
            return ImageFromImages(images, new CGSize(imageDimension * 2, imageDimension * 2));
        }

        private class ClusterRenderer : DefaultClusterRenderer
        {
            public ClusterRenderer(MapView mapView, IClusterIconGenerator iconGenerator) : base(mapView, iconGenerator)
            {
            }

            public override bool ShouldRenderAsCluster(ICluster cluster, float zoom)
            {
                return cluster.Count >= 2;
            }
        }
    }
}
