using System;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utility;
using GoogleMapSample.Models;
using UIKit;

namespace GoogleMapSample.Views.Basic
{
    public partial class BasicViewController : UIViewController
    {
        private int clusterItemCount = 10000;
        private double cameraLatitude = -33.8;
        private double cameraLongitude = 151.2;
        double extent = 0.2;

        private MapView mapView;
        private MapDelegate mapDelegate;
        private ClusterManager clusterManager;

        public BasicViewController() : base("BasicViewController", null)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            SetMapView();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetRemoveButton();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetClusterManager();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void SetRemoveButton()
        {
            UIBarButtonItem removeButton = new UIBarButtonItem()
            {
                Target = this,
                Title = "Remove",
                Style = UIBarButtonItemStyle.Plain
            };
            removeButton.Clicked -= RemoveButton_Clicked;
            removeButton.Clicked += RemoveButton_Clicked;
            NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { removeButton };
        }

        private void SetClusterManager()
        {
            mapDelegate = new MapDelegate(mapView);
            var iconGenerator = IconGeneratorWithImages();
            var algorithm = new NonHierarchicalDistanceBasedAlgorithm();
            var renderer = new DefaultClusterRenderer(mapView, iconGenerator) { WeakDelegate = mapDelegate };
            clusterManager = new ClusterManager(mapView, algorithm, renderer);
            clusterManager.SetDelegate(mapDelegate, mapDelegate);
            GenerateClusterItem();
            clusterManager.Cluster();
        }

        void RemoveButton_Clicked(object sender, EventArgs e)
        {
            clusterManager = null;
        }

        private void SetMapView()
        {
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 10);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
        }

        private void GenerateClusterItem()
        {
            for (int index = 1; index <= clusterItemCount; index++)
            {
                double lat = cameraLatitude + extent * GetRandomNumber(-1.0, 1.0);
                double lng = cameraLongitude + extent * GetRandomNumber(-1.0, 1.0);
                var item = new ClusterMarker("Item " + index, new CLLocationCoordinate2D(lat, lng));
                clusterManager.AddItem(item);
            }
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private DefaultClusterIconGenerator DefaultIconGenerator()
        {
            return new DefaultClusterIconGenerator();
        }

        private DefaultClusterIconGenerator IconGeneratorWithImages()
        {
            return new DefaultClusterIconGenerator(new NSNumber[] { 50, 100, 250, 500, 1000 }, new UIImage[] { UIImage.FromBundle("m1"), UIImage.FromBundle("m2"), UIImage.FromBundle("m3"), UIImage.FromBundle("m4"), UIImage.FromBundle("m5") });
        }

        internal class MapDelegate : MapViewDelegate, IClusterManagerDelegate, IClusterRendererDelegate
        {
            private MapView mapView;

            public MapDelegate(MapView mapView)
            {
                this.mapView = mapView;
            }
            public override void DidTapAtCoordinate(MapView mapView, CLLocationCoordinate2D coordinate)
            {
                Console.WriteLine(string.Format("Tapped at location: ({0}, {1})", coordinate.Latitude, coordinate.Longitude));
            }

            public override bool TappedMarker(MapView mapView, Marker marker)
            {
                if (marker.UserData is ClusterMarker)
                {
                    Console.WriteLine("Did tap marker for cluster item " + ((ClusterMarker)marker.UserData).Title);
                }
                else
                {
                    Console.WriteLine("Did tap a normal marker");
                }
                return false;
            }

            [Export("clusterManager:didTapCluster:")]
            public bool DidTapCluster(ClusterManager clusterManager, ICluster cluster)
            {
                mapView.MoveCamera(CameraUpdate.SetTarget(cluster.Position, mapView.Camera.Zoom + 1));
                return true;
            }

            [Export("renderer:willRenderMarker:")]
            public void WillRenderMarker(IClusterRenderer renderer, Marker marker)
            {
                if (marker.UserData is ClusterMarker)
                {
                    marker.Title = ((ClusterMarker)marker.UserData).Title;
                }
            }
        }
    }
}
