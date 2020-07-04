using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Android.Util;
using Android.Content;

namespace Test
{
    [Activity(Label = "Diabetes Detector", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public static string FinalResult= "hello";//tells if you have diabities or not
        public static EditText Pregnancies;
        public static EditText PlasmaGlucose;
        public static EditText BloodPressure;
        public static EditText TricepThickness;
        public static EditText SerumInsulin;
        public static EditText BMI;
        public static EditText DiabitiesPedigree;
        public static EditText Age;
        public String PredictionString;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
             Pregnancies = FindViewById<EditText>(Resource.Id.Pregnancies);
             PlasmaGlucose = FindViewById<EditText>(Resource.Id.PlasmaGlucose);
             BloodPressure = FindViewById<EditText>(Resource.Id.BloodPressure);
             TricepThickness = FindViewById<EditText>(Resource.Id.TricepThickness);
             SerumInsulin = FindViewById<EditText>(Resource.Id.SerumInsulin);
             BMI = FindViewById<EditText>(Resource.Id.BMI);
             DiabitiesPedigree =FindViewById<EditText>(Resource.Id.DiabitiesPedigree);
             Age = FindViewById<EditText>(Resource.Id.Age);
            Button submit = FindViewById<Button>(Resource.Id.Submit);
            TextView Result = FindViewById<TextView>(Resource.Id.Result);
            this.FindViewById<Button>(Resource.Id.Submit).Click += this.execute;



            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

        }
        public string prediction(string serverinput)
        {
            if (serverinput.Contains("0"))
            {
                return "No Diabetes";
            }

            if (serverinput.Contains("1"))
            {
                return "You Have Diabetes";
            }
            else return "process pending try again";
        }
        public void execute(object sender, EventArgs args)
        {
            Log.Info("error", "this is an info message");
            //Toast.MakeText(this, FinalResult, ToastLength.Long).Show();
            InvokeRequestResponseService().Wait();
            // this.FindViewById<TextView>(Resource.Id.Result).Text = prediction(FinalResult); // Predicts Whether you have diabtes or not
            PredictionString = prediction(FinalResult);
            Intent nextActivity = new Intent(this, typeof(HomePage));
            nextActivity.PutExtra("Prediction", PredictionString);
            StartActivity(nextActivity);

        }

        static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
                        {
                            "input1",
                            new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                            {
                                                "PatientID", "1354778"
                                            },
                                            {
                                                "Pregnancies", Pregnancies.Text
                                            },
                                            {
                                                "PlasmaGlucose", PlasmaGlucose.Text
                                            },
                                            {
                                                "DiastolicBloodPressure", BloodPressure.Text
                                            },
                                            {
                                                "TricepsThickness", TricepThickness.Text
                                            },
                                            {
                                                "SerumInsulin", SerumInsulin.Text
                                            },
                                            {
                                                "BMI", BMI.Text
                                            },
                                            {
                                                "DiabetesPedigree", DiabitiesPedigree.Text
                                            },
                                            {
                                                "Age", Age.Text
                                            },
                                }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "KOM6qoxLDuNUm7B1cFfQrfs+Y2Nxcqfu9uh/pvbYzNcUoEuE5L9BreQhVnjJNjJAcXn7EJj5ET+Y36WSpECgxg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/a32cc0f6e4f4453fabf21b2a94d9a430/services/e439236284bf4efba856e42e023b7978/execute?api-version=2.0&format=swagger");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    FinalResult = result;
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Console.WriteLine(responseContent);
                }
            }
        }



        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        //private void FabOnClick(object sender, EventArgs eventArgs)
        //{
        //    View view = (View)sender;
        //    Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
        //        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        //}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
