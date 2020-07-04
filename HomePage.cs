
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Test
{
    [Activity(Label = "Diabetes Detector")]
    public class HomePage : AppCompatActivity, IOnMapReadyCallback
    {
        public TextView Result;
        public TextView Recommendation;

        public String DiabetesPrediction;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomePage);
            DiabetesPrediction = Intent.GetStringExtra("Prediction" ?? "Not Got");
            Result = FindViewById<TextView>(Resource.Id.Result);
            Result.Text = DiabetesPrediction;
            Recommendation = FindViewById<TextView>(Resource.Id.Recommendation);
            ActionRecommendation();

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);



            // Create your application here
        }
        public void ActionRecommendation()
        {
            if(DiabetesPrediction== "You Have Diabetes")
            {
                Recommendation.Text = "Consult a Doctor, here are the highest rated options nearby";
            }
           if(DiabetesPrediction== "No Diabetes")
            {
                Recommendation.Text = "Please Remember to Still Schedule a Yearly Check Up";
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            CameraUpdate center =
        CameraUpdateFactory.NewLatLng(new LatLng(40.5187,
                                                 -74.4121));
            CameraUpdate zoom = CameraUpdateFactory.ZoomTo(12);
            googleMap.MoveCamera(center);
            googleMap.AnimateCamera(zoom);

            googleMap.MapType = GoogleMap.MapTypeNormal;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
           

            MarkerOptions Doctor1 = new MarkerOptions();
            Doctor1.SetPosition(new LatLng(40.597470, -74.359500));
            Doctor1.SetTitle("Janine Jamieson,D.O");
            Doctor1.SetSnippet("Number:(908) 834-8343");
            googleMap.AddMarker(Doctor1);

            MarkerOptions Doctor2 = new MarkerOptions();
            Doctor2.SetPosition(new LatLng(40.571550, -74.355140));
            Doctor2.SetTitle("Dr. Hitesh R. Patel, MD");
            Doctor2.SetSnippet("Number:(732) 744-0634");
            googleMap.AddMarker(Doctor2);

            MarkerOptions Doctor3 = new MarkerOptions();
            Doctor3.SetPosition(new LatLng(40.541870, -74.394900));
            Doctor3.SetTitle("Edison Wellness Medical Group: Hao Zhang, MD");
            Doctor3.SetSnippet("Number:(732) 201-6985");
            googleMap.AddMarker(Doctor3);

            
        }
    }
}
