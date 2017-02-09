using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Todo.Models;
using Todo.ServiceClient;
using Xamarin.Forms;

namespace Todo.Views
{
    public class CaptureFlowScrollPage : ContentPage
    {
        private StackLayout _stackLayout;
        private VehicleCapture _vehicleCapture;
        private RestVehicleServices _restVehicleServices;

        public List<string> Manufacturers { get; set; }
        private Picker _manufacturerPicker;


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Manufacturers = await _restVehicleServices.GetManufacturers();

            foreach (var m in Manufacturers)
            {
                _manufacturerPicker.Items.Add(m);
            }
        }

        public CaptureFlowScrollPage()
        {
            Manufacturers = new List<string>();

            _vehicleCapture = new VehicleCapture();

            _restVehicleServices = new RestVehicleServices();

            _manufacturerPicker = new Picker
            {
                VerticalOptions = LayoutOptions.Start,
                Title = Strings.Manufactuer

            };
            


            var lookupButton = new Button
            {
                Text = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_LOOKUP,
                BackgroundColor = Color.FromHex("#2196F3"),
                TextColor = Color.White,
                BorderColor = Color.Gray,
                BorderRadius = 2,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness (5.0d)
                
            };
            lookupButton.Clicked += LookupButton_Clicked;

            _stackLayout = new StackLayout
            {
                Children =
                {
                    _manufacturerPicker,   
                    new Entry
                    {
                        Placeholder = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_Your_number_plate,
                        VerticalOptions = LayoutOptions.Start,
                        BindingContext = _vehicleCapture.VRM,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry))
                    },
                    lookupButton
                }
            };
            this.Content = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,

                Content = _stackLayout


            };

        
        }

        private void LookupButton_Clicked(object sender, EventArgs e)
        {
            if (_vehicleCapture.VRM != String.Empty)
            {
                new RestVehicleServices().LookupVRM(_vehicleCapture.VRM);
            }
        }


        public Page BuildForm()
        {
            throw new NotImplementedException();
        }
    }
}
