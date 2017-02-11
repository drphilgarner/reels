using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
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
        private Entry _vrmEntry;
        private Button _lookupButton;
        private Regex _vrmRegex;
        private Image _thumbnailImage;
        private Label _lookupResultLabel;

        public string SelectedManufacturer { get; set; }


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
            _vrmRegex = new Regex(Strings.VrmEntry_Regex);

            _restVehicleServices = new RestVehicleServices();

            _manufacturerPicker = new Picker
            {
                //VerticalOptions = LayoutOptions.Start,
                Title = Strings.Manufactuer,
                HorizontalOptions = LayoutOptions.End
                
            };
            


            _lookupButton = new Button
            {
                Text = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_LOOKUP,

                BackgroundColor = Color.FromHex(Variables.StandardBtnColourHex),
                TextColor = Color.White,
                BorderColor = Color.Gray,
                BorderRadius = 2,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness (5.0d),
                IsEnabled = false
                
                
            };
            _lookupButton.Clicked += LookupButton_Clicked;

            var thickness = new Thickness(5.0d);
            var chooseManufacturerButton = new Button
            {
                Text = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_CHOOSE_MAKE,
                BackgroundColor = Color.FromHex(Variables.StandardBtnColourHex),
                TextColor = Color.White,
                BorderColor = Color.Gray,
                BorderRadius = 2,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = thickness

            };

            chooseManufacturerButton.Clicked += ChooseManufacturerButton_Clicked;

            _vrmEntry = new Entry
            {
                Placeholder = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_Your_number_plate,
                VerticalOptions = LayoutOptions.Center,
                BindingContext = _vehicleCapture.VRM,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry)),
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Yellow,
                Margin = thickness
                
            };

            _vrmEntry.TextChanged += VrmEntry_TextChanged;
            _stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,

                        Children =
                        {
                            chooseManufacturerButton,
                            _manufacturerPicker
                        }
                    },
                    _vrmEntry,
                    _lookupButton
                }

       
            };
            this.Content = new ScrollView
            {
                //VerticalOptions = LayoutOptions.FillAndExpand,

                Content = _stackLayout


            };

            MessagingCenter.Subscribe<CaptureFlowScrollPage, string>(this, "SelectedManufacturer", (sender, selectedManufacturer) =>
            {
                if (selectedManufacturer != null)
                {
                    _manufacturerPicker.SelectedIndex = Manufacturers.IndexOf(selectedManufacturer);
                    SelectedManufacturer = selectedManufacturer;
                }
            });


        }

        private void VrmEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            _vrmEntry.TextChanged -= VrmEntry_TextChanged;
            _vrmEntry.Text = _vrmEntry.Text.ToUpper();

            
            var vrmNoSpace = _vrmEntry.Text.Replace(" ", "");
            _vehicleCapture.VRM = vrmNoSpace;

            //enable the lookup if the numberplate is valid
            if (SelectedManufacturer != null )
            {
                _lookupButton.IsEnabled = _vrmRegex.IsMatch(vrmNoSpace);
            }

            _vrmEntry.TextChanged += VrmEntry_TextChanged;
        }


        private async void ChooseManufacturerButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManufacturersListView(this));

            
        }

        private async void LookupButton_Clicked(object sender, EventArgs e)
        {
            if (_vehicleCapture.VRM != String.Empty)
            {
                var restVehicleServices = new RestVehicleServices();
                JObject result = await restVehicleServices.LookupVrmAsync(_vehicleCapture.VRM, SelectedManufacturer);

                if (result != null)
                {
                    var thumbnailString = await restVehicleServices.GetVehicleThumbnail(SelectedManufacturer,
                        result["model"].ToString(), result["vehicleColour"].ToString(), result["yearOfManufactureDate"].ToString());

                    if (_thumbnailImage != null)
                    {
                        _stackLayout.Children.Remove(_thumbnailImage);
                        _stackLayout.Children.Remove(_lookupResultLabel);
                    }

                    _lookupResultLabel = new Label
                    {
                        Text = $"{result["yearOfManufactureDate"]} {result["model"]}",
                        HorizontalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    _thumbnailImage = new Image
                    {
                        Source = ImageSource.FromUri(new Uri(thumbnailString)),
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    _stackLayout.Children.Add(_lookupResultLabel);
                    _stackLayout.Children.Add(_thumbnailImage);
                }
            }
        }

    }
}
