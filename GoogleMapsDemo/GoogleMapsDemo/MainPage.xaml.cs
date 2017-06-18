using GoogleMapsDemo.Model;
using GoogleMapsDemo.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GoogleMapsDemo
{
    public partial class MainPage : ContentPage
    {
        string type;
        string MainLabel_Text = string.Empty;
        string MainLabel1_Text = string.Empty;
        string imgDisp_Source = string.Empty;
        string imgDesc_Text = string.Empty;
        int flag = 0;
        string actual_address;
        double Latitude, Longitude;

        public LocationHistoryModel LocationHistory { get; set; }

        public static string TimeAgo(DateTime startdate)
        {
            TimeSpan span = DateTime.Now - startdate;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("You have visited this place" + " about {0} {1} ago",years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("You have visited this place" + " about {0} {1} ago",months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("You have visited this place" + " about {0} {1} ago",span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("You have visited this place" + " about {0} {1} ago",span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("You have visited this place" + " about {0} {1} ago",span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("You have visited this place" + " about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "You have visited this place" + " just now";
            return string.Empty;
        }
        Geocoder geoCoder;
        
        public MainPage() 
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }
                     
        public void pinningonthemap(double lat, double longi, string label, string timestmp , string innertime, string type)
        {
            Pin pin = new Pin() { Position = new Position(lat, longi) };
            pin.Type = PinType.Place;
            
            DateTime startdate = GetDateTime(timestmp);
            pin.Label = startdate.ToString("dd/MM/yyyy");
            pin.Address = startdate.ToString("HH:mm:ss");
            
            MyMap.Pins.Add(pin);
            
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(lat, longi), Distance.FromMiles(1)));


            pin.Clicked += async (sender, args) =>
            {
                if (startdate.Hour < 18 && startdate.Hour > 06)
                {
                    card.BackgroundColor = Color.PaleGoldenrod;
                    addresslabel.BackgroundColor = Color.PaleGoldenrod;
                }
                else
                {
                    card.BackgroundColor = Color.LightGray;
                    addresslabel.BackgroundColor = Color.LightGray;
                }
                var pinTemp = sender as Pin;
                var position = pinTemp.Position;
                addresslabel.Text = string.Empty;
                var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                         

                foreach (var address in possibleAddresses)
                {
                    flag += 1;
                    if (flag == 1)
                    {
                        addresslabel.Text = address;
                    }
                    else
                        actual_address = address;    
                   
                }
                flag = 0;
             
                littlecard(innertime,type);
                MainLabel.Text = MainLabel_Text;
                MainLabel1.Text = MainLabel1_Text;
                imgDisp.Source = imgDisp_Source;
                imgDesc.Text = imgDesc_Text;
                
                };
        }

        public void littlecard(string timestmp,string type)
        {

           DateTime visiteddate = GetDateTime(timestmp);
           string timeago = TimeAgo(visiteddate);
           int hour = visiteddate.Hour % 12;
           MainLabel_Text = visiteddate.ToString("dd/MM/yyyy "+Convert.ToString(hour)+":mm:ss tt");
           MainLabel1_Text = timeago; 
            if (type == "WALKING" || type == "ON_FOOT")
            {
                imgDisp_Source = "walk.png";
                imgDesc_Text = "You were Walking";
                // card.BackgroundColor = Color.Green;
             }
            else if (type == "IN_VEHICLE")
             {
                imgDisp_Source = "vehicle.jpg";
                imgDesc_Text = "You were in a car";
             }
            else if (type == "ON_BICYCLE")
             {
                imgDisp_Source = "cycle.jpg";
                imgDesc_Text = "You were cycling";
             }
            else if (type == "STILL")
             {
                imgDisp_Source = "still.jpg";
                imgDesc_Text = "You were not moving";
             }
            else
             {
                imgDisp_Source = "unknown.gif";
                imgDesc_Text = "We are not sure";
             }
          }
       
        private async void Button_Clicked(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var fileToRead = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();
            var result = Encoding.UTF8.GetString(fileToRead.DataArray, 0, fileToRead.DataArray.Length);
            if (result != null)
            {
                LocationHistory = JsonConvert.DeserializeObject<LocationHistoryModel>(result);
            }

        }

        private void Button_Clicked2(object sender, EventArgs e)

        {
            MyMap.Pins.Clear();
            foreach (var item in LocationHistory.locations.Distinct())
            {

                var lat = Convert.ToDouble(item.latitudeE7).ToString().Insert(2, ".");
                double.TryParse(lat, out Latitude);
                var longitude = Convert.ToDouble(item.longitudeE7).ToString().Insert(2, ".");
                double.TryParse(longitude, out Longitude);

                if (item.activity != null)
                {
                    foreach (var variable in item.activity)
                    {
                        if (variable.activity != null)

                            type = variable.activity[0].type;
                        else
                            type = "UNKNOWN";

                    }
                    pinningonthemap(Latitude, Longitude, item.altitude.ToString(), item.timestampMs.ToString(), item.activity[0].timestampMs, type);

                }
            }
        }

        private void Filter_Clicked(object sender, EventArgs e)
        {
            MyMap.Pins.Clear();
            Filter();
            //foreach (var item in LocationHistory.locations.Distinct())
            //{
            //    DateTime compdate = GetDateTime(Convert.ToString(item.timestampMs));
            //    if ((compdate.Year >= Convert.ToInt32(FromYear.Text)) && (compdate.Year <= Convert.ToInt32(ToYear.Text)))
            //    {
            //        if ((compdate.Month >= Convert.ToInt32(FromMonth.Text)) && (compdate.Month <= Convert.ToInt32(ToMonth.Text)))
            //        {
                        
            //            if ((Convert.ToInt32(compdate.ToString("dd")) >= Convert.ToInt32(FromDate.Text)) && (Convert.ToInt32(compdate.ToString("dd")) <= Convert.ToInt32(ToDate.Text)))
            //            {
            //                var lat = Convert.ToDouble(item.latitudeE7).ToString().Insert(2, ".");
            //                double.TryParse(lat, out Latitude);
            //                var longitude = Convert.ToDouble(item.longitudeE7).ToString().Insert(2, ".");
            //                double.TryParse(longitude, out Longitude);
            //                if (item.activity != null)
            //                {
            //                    foreach (var variable in item.activity)
            //                    {
            //                        if (variable.activity != null)

            //                            type = variable.activity[0].type;
            //                        else
            //                            type = "UNKNOWN";

            //                    }
            //                    pinningonthemap(Latitude, Longitude, item.altitude.ToString(), item.timestampMs.ToString(), item.activity[0].timestampMs, type);

            //                }
            //            }
            //        }
            //    }
            //}

        }

        private void URL_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.google.co.in/webhp#q=" + actual_address));
        }

        private DateTime GetDateTime(string param)
        {
            double ticks = double.Parse(param);
            TimeSpan time = TimeSpan.FromMilliseconds(ticks);
            DateTime startdate = new DateTime(1970, 1, 1) + time;
            return startdate;
        }

        public void Filter ()
        {
            DateTime FromDates = DateTime.ParseExact(FromYear.Text+"-"+FromMonth.Text+"-"+FromDate.Text + " 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime ToDates = DateTime.ParseExact(ToYear.Text + "-" + ToMonth.Text + "-" + ToDate.Text + " 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime ReferenceDates = DateTime.ParseExact("1970-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            double fromdates_milliseconds = (FromDates - ReferenceDates).TotalMilliseconds;
            double todates_milliseconds = (ToDates - ReferenceDates).TotalMilliseconds;

            foreach (var item in LocationHistory.locations.Distinct())
            {
                if ((item.timestampMs > fromdates_milliseconds) && (item.timestampMs < todates_milliseconds))
                { 
                            var lat = Convert.ToDouble(item.latitudeE7).ToString().Insert(2, ".");
                            double.TryParse(lat, out Latitude);
                            var longitude = Convert.ToDouble(item.longitudeE7).ToString().Insert(2, ".");
                            double.TryParse(longitude, out Longitude);
                            if (item.activity != null)
                            {
                                foreach (var variable in item.activity)
                                {
                                    if (variable.activity != null)

                                        type = variable.activity[0].type;
                                    else
                                        type = "UNKNOWN";

                                }
                                pinningonthemap(Latitude, Longitude, item.altitude.ToString(), item.timestampMs.ToString(), item.activity[0].timestampMs, type);

                            }
                }
            }
        }

    }
}
