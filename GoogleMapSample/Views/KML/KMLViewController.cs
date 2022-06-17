using System;
using CoreGraphics;
using Foundation;
using Google.Maps;
using Google.Maps.Utility;
using UIKit;

namespace GoogleMapSample.Views.KML
{
    public partial class KMLViewController : UIViewController
    {
        private double cameraLatitude = 37.4220;
        private double cameraLongitude = -122.0841;

        private MapView mapView;

        public KMLViewController() : base("KMLViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetKML();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void LoadView()
        {
            base.LoadView();
            SetMapView();
        }

        private void SetMapView()
        {            
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 17);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
        }

        private void SetKML()
        {
            var bundle = NSBundle.MainBundle;
            var path = bundle.PathForResource("KML_Sample", "kml");
            var url = NSUrl.CreateFileUrl(path, null);
            var parser = new KMLParser(url);
            parser.Parse();
            var renderer = new GeometryRenderer(mapView, parser.Placemarks, parser.Styles);
            renderer.Render();
        }
    }
}