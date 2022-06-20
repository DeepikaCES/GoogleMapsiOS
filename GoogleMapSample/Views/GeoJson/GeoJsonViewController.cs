using System;
using CoreGraphics;
using Foundation;
using Google.Maps;
using Google.Maps.Utility;
using UIKit;

namespace GoogleMapSample.Views.GeoJson
{
    public partial class GeoJsonViewController : UIViewController
    {
        private double cameraLatitude = -33.8;
        private double cameraLongitude = 151.2;

        private MapView mapView;

        public GeoJsonViewController() : base("GeoJsonViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var path = NSBundle.PathForResourceAbsolute("GeoJSON_Sample", "geojson", NibBundle.BundlePath);
            var url = NSUrl.CreateFileUrl(path, null);
            var jsonParser = new GMUGeoJSONParser(url);
            jsonParser.Parse();
            var renderer = new GeometryRenderer(mapView, jsonParser.Features);
            renderer.Render();
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
        }

        private void SetMapView()
        {
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 1);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;

            mapView.OverlayTapped += MapView_OverlayTapped;
            mapView.InfoTapped += MapView_InfoTapped;
            mapView.MyLocationTapped += MapView_MyLocationTapped;
            mapView.CoordinateTapped += MapView_CoordinateTapped;
            mapView.PoiWithPlaceIdTapped += MapView_PoiWithPlaceIdTapped;
            mapView.DraggingMarker += MapView_DraggingMarker;
        }

        private void MapView_DraggingMarker(object sender, GMSMarkerEventEventArgs e)
        {

        }

        private void MapView_PoiWithPlaceIdTapped(object sender, GMSPoiWithPlaceIdEventEventArgs e)
        {

        }

        private void MapView_CoordinateTapped(object sender, GMSCoordEventArgs e)
        {

        }

        private void MapView_MyLocationTapped(object sender, GMSMyLocationTappedEventArgs e)
        {

        }

        private void MapView_InfoTapped(object sender, GMSMarkerEventEventArgs e)
        {

        }

        private void MapView_OverlayTapped(object sender, GMSOverlayEventEventArgs e)
        {

        }
    }
}
