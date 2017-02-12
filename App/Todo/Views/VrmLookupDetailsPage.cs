﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Todo.Models;
using Todo.ServiceClient;
using Xamarin.Forms;

namespace Todo.Views
{
    public class VrmLookupDetailsPage : ContentPage
    {
        private StackLayout _stackLayout;
        private VehicleCapture _vehicleCapture;
        private RestVehicleServices _restVehicleServices;

        public List<string> Manufacturers { get; set; }
        private Picker _manufacturerPicker;
        private Entry _vrmEntry;
        private Button _manufacturerBtn;
        private Button _lookupButton;
        private Regex _vrmRegex;
        private Label _lookupResultLabel;
        
        public string SelectedManufacturer { get; set; }

        private Image _thumbnailLogoImage;
        private Entry _modelEntry;
        private Picker _modelYearPicker;
        private bool _isVrmValid;
        private Entry _colorEntry;
        private bool _isLookupSuccessful;


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Manufacturers = await _restVehicleServices.GetManufacturers();

            foreach (var m in Manufacturers)
            {
                _manufacturerPicker.Items.Add(m);
            }


        }

        public VrmLookupDetailsPage()
        {
            BackgroundColor = Color.White;

            Manufacturers = new List<string>();
            var thickness = new Thickness(5.0d);

            _vehicleCapture = new VehicleCapture();
            _vrmRegex = new Regex(Strings.VrmEntry_Regex);

            _restVehicleServices = new RestVehicleServices();


            _manufacturerPicker = new Picker
            {
                //VerticalOptions = LayoutOptions.Start,
                Title = Strings.Manufacturer,
                HorizontalOptions = LayoutOptions.End
                
            };

            _manufacturerBtn = new Button
            {
                
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = thickness,
                Text = Strings.Manufacturer.ToUpper(), 
                VerticalOptions = LayoutOptions.Center, 
                BackgroundColor = Color.FromHex(Variables.StandardBtnColourHex),
                BorderRadius = 2,
                BorderWidth = 1,
                TextColor = Color.White

            };
            _manufacturerBtn.Clicked += ManufacturerBtnClicked;

            _thumbnailLogoImage = new Image
            {
                Margin = thickness,
                HorizontalOptions = LayoutOptions.End
                
            };

            _vrmEntry = new Entry
            {
                Placeholder = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_Your_number_plate,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BindingContext = _vehicleCapture.VRM,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry)),
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Yellow,
                Margin = thickness,
                WidthRequest = 150
                
            };
            _vrmEntry.TextChanged += VrmEntry_TextChanged;

            _lookupButton = new Button
            {
                Text = Strings.CaptureFlowScrollPage_CaptureFlowScrollPage_LOOKUP,

                BackgroundColor = Color.FromHex(Variables.StandardBtnColourHex),
                TextColor = Color.White,
                BorderColor = Color.Gray,
                BorderRadius = 2,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.End,
                //VerticalOptions = LayoutOptions.Start,
                Margin = thickness,
                IsEnabled = _isVrmValid,
                
            };
            _lookupButton.Clicked += LookupButton_Clicked;

            //model details etc
            _modelEntry = new Entry
            {
                Placeholder = Strings.VrmLookupDetailsPage_VrmLookupDetailsPage_MODEL,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = thickness,
                IsEnabled = _isLookupSuccessful,
                WidthRequest = 200
            };

            
            _modelYearPicker = new Picker
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Margin = thickness,
                IsEnabled = _isLookupSuccessful,
                WidthRequest = 200,

            };

            int modelYearsAgo = 75;
            foreach (var m in Enumerable.Range(DateTime.Now.Year - modelYearsAgo, modelYearsAgo))
            {
                _modelYearPicker.Items.Add(m.ToString());
            }

            _colorEntry = new Entry
            {
                Placeholder = "Colour",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = thickness,
                WidthRequest = 200,

                IsEnabled = _isLookupSuccessful
            };
            

            _stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new StackLayout
                    {
                      Orientation = StackOrientation.Horizontal,
                      HeightRequest = 70,
                      Children =
                      {
                          _manufacturerBtn,
                          _thumbnailLogoImage
                          
                      }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            _vrmEntry,
                            _lookupButton
                        }
                    },
                    _modelEntry,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            _modelYearPicker,
                            _colorEntry
                        }
                    }
                }
            };
            this.Content = new ScrollView
            {
                //VerticalOptions = LayoutOptions.FillAndExpand,
                Content = _stackLayout
            };

            MessagingCenter.Subscribe<VrmLookupDetailsPage, Manufacturer>(this, "SelectedManufacturer", (sender, selectedManufacturer) =>
            {
                if (selectedManufacturer != null)
                {
                    _manufacturerPicker.SelectedIndex = Manufacturers.IndexOf(selectedManufacturer.Name);
                    SelectedManufacturer = selectedManufacturer.Name;
                    _thumbnailLogoImage.Source = selectedManufacturer.ImageSource;
                }
            });


        }

        private async void ManufacturerBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManufacturersListView(this));
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
                _isVrmValid = _vrmRegex.IsMatch(vrmNoSpace);
                _lookupButton.IsEnabled = _isVrmValid;
            }

            _vrmEntry.TextChanged += VrmEntry_TextChanged;
        }


        private async void LookupButton_Clicked(object sender, EventArgs e)
        {
            if (_vehicleCapture.VRM != String.Empty)
            {
                var restVehicleServices = new RestVehicleServices();
                JObject result = await restVehicleServices.LookupVrmAsync(_vehicleCapture.VRM, SelectedManufacturer);

                if (result != null)
                {
                    _isLookupSuccessful = !bool.Parse(result["hasFailedLookup"].ToString());

                    if (_isLookupSuccessful)
                    {
                        _modelYearPicker.IsEnabled = true;
                        _colorEntry.IsEnabled = true;
                        _modelEntry.IsEnabled = true;


                        _modelEntry.Text = result["model"].ToString();
                        _colorEntry.Text = result["vehicleColour"].ToString();
                        _modelYearPicker.SelectedIndex =
                        _modelYearPicker.Items.IndexOf(result["yearOfManufactureDate"].ToString());
                    }

                }
            }
        }

    }
}