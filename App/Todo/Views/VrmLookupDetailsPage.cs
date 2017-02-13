using System;
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
        private Entry _odometerEntry;
        private Picker _milesOrKilometersPicker;
        private Picker _fuelTypePicker;
        private Picker _engineSizePicker;


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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BindingContext = _vehicleCapture.VRM,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry)),
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromHex(Variables.VrmEntryColourFromHex),
                Margin = thickness,
                
                
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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = thickness,
                IsEnabled = _isLookupSuccessful,
                WidthRequest = 200
            };

            
            _modelYearPicker = new Picker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = thickness,
                IsEnabled = _isLookupSuccessful,

            };

            int modelYearsAgo = 100;
            foreach (var m in Enumerable.Range(DateTime.Now.Year - modelYearsAgo, modelYearsAgo))
            {
                _modelYearPicker.Items.Add(m.ToString());
            }

            _colorEntry = new Entry
            {
                Placeholder = "COLOUR",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = thickness,
                
                IsEnabled = _isLookupSuccessful
            };

            _odometerEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = Strings.VrmLookupDetailsPage_VrmLookupDetailsPage_ODOMETER,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = thickness,
                IsEnabled = _isLookupSuccessful
            };

            _milesOrKilometersPicker = new Picker
            {
                IsEnabled = _isLookupSuccessful,
                Margin = thickness,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _milesOrKilometersPicker.Items.Add("MILES");
            _milesOrKilometersPicker.Items.Add("KILOMETERS");
            _milesOrKilometersPicker.SelectedIndex = 0;

            _fuelTypePicker = new Picker
            {
                IsEnabled = _isLookupSuccessful,
                Margin = thickness,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var fuelTypes = new[] {"Petrol", "Diesel", "Other", "Hybrid Electric", "Electric" };
            foreach (var f in fuelTypes)
                _fuelTypePicker.Items.Add(f);

            _engineSizePicker = new Picker
            {
                IsEnabled = _isLookupSuccessful,
                Margin = thickness,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            double smallestEngine = 0d;
            double biggestEngine = 9d;
            double increment = 0.1d;

            for (double i = smallestEngine; i < biggestEngine; i+= increment)
                _engineSizePicker.Items.Add(i.ToString("#.#"));
            

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
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            _odometerEntry,
                            _milesOrKilometersPicker
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children=
                        {
                            _fuelTypePicker,
                            _engineSizePicker
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
                        _odometerEntry.IsEnabled = true;
                        _milesOrKilometersPicker.IsEnabled = true;
                        _fuelTypePicker.IsEnabled = true;
                        _engineSizePicker.IsEnabled = true;


                        _modelEntry.Text = result["model"].ToString();
                        _colorEntry.Text = result["vehicleColour"].ToString();
                        _modelYearPicker.SelectedIndex = _modelYearPicker.Items.IndexOf(result["yearOfManufactureDate"].ToString());

                        var fuelType = result["fuelType"].ToString();

                        switch (fuelType)
                        {
                            case "Electric":
                            case "Electricity":
                                _fuelTypePicker.SelectedIndex = _fuelTypePicker.Items.IndexOf("ELECTRICITY");
                                break;
                            default:
                                _fuelTypePicker.SelectedIndex = _fuelTypePicker.Items.IndexOf(fuelType);
                                break;
                        }

                        //set engine size
                        double simpleEngineSize = 0;
                        string justNumericEngine =
                            new string(result["cylinderCapacity"].ToString().Cast<char>().Where(char.IsDigit).ToArray());

                        if (double.TryParse(justNumericEngine, out simpleEngineSize))
                        {
                            var round = Math.Round(simpleEngineSize/1000,2);
                            _engineSizePicker.SelectedIndex = _engineSizePicker.Items.IndexOf(round.ToString("#.#"));
                        }

                        //TODO: implement estimated current odo

                    }

                }
            }
        }

    }
}
