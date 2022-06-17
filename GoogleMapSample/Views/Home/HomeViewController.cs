using System;
using System.Collections.Generic;
using GoogleMapSample.Models;
using GoogleMapSample.Views.Basic;
using GoogleMapSample.Views.CustomMarker;
using GoogleMapSample.Views.GeoJson;
using GoogleMapSample.Views.HeatMap;
using GoogleMapSample.Views.Home.Cell;
using GoogleMapSample.Views.Home.Source;
using GoogleMapSample.Views.KML;
using UIKit;

namespace GoogleMapSample.Views.Home
{
    public partial class HomeViewController : UIViewController
    {
        private List<Pages> pages;
        private HomeSource homeSource;


        public HomeViewController() : base("HomeViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTableProperties();
            NavigationItem.Title = "Demos";
            View.AddSubview(tableView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            homeSource.ItemSelected += HomeSource_ItemSelected;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            homeSource.ItemSelected -= HomeSource_ItemSelected;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void SetTableProperties()
        {
            PopulatePages();
            homeSource = new HomeSource(pages);
            tableView.RegisterNibForCellReuse(HomeViewCell.Nib, HomeViewCell.Key);
            tableView.Source = homeSource;
        }

        private void PopulatePages()
        {
            pages = new List<Pages>()
            {
                new Pages("Basic","",new BasicViewController()),
                new Pages("Custom Marker","",new CustomMarkerViewController()),
                new Pages("Geo Json import","",new GeoJsonViewController()),
                new Pages("Heat Map import","",new HeatMapViewController()),
                new Pages("Kml Import","",new KMLViewController())
            };
        }

        void HomeSource_ItemSelected(int row)
        {
           NavigationController?.PushViewController(pages[row].Page,true);
        }
    }
}