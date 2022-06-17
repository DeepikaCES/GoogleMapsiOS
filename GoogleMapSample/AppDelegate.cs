using System;
using Foundation;
using Google.Maps;
using GoogleMapSample.Views.Home;
using UIKit;

namespace GoogleMapSample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        [Export("window")]
       
        public override UIWindow Window
        {
            get;
            set;
        }

        private string GoogleMapsApiKey = "AIzaSyBXn8fmjRpUutYm2OmF13uxNHpAbVfiVe0";

        public UINavigationController navigationController { get; set; }

        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            if (string.IsNullOrEmpty(GoogleMapsApiKey))
                throw new Exception("Please provide your own Google Maps Api Key");


            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            MapServices.ProvideApiKey(GoogleMapsApiKey);
            navigationController = new UINavigationController(new HomeViewController());
            Window.RootViewController = navigationController;
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}

