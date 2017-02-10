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
    public class ManufacturersListView : ContentPage
    {
        private readonly RestVehicleServices _restVehicleServices;

        public ManufacturersListView()
        {
            _restVehicleServices = new RestVehicleServices();


            var lv = new ListView
            {
                ItemsSource = Manufacturers,

                ItemTemplate = new DataTemplate(() =>
                {
                    Image logoImage = new Image {Aspect = Aspect.AspectFit};
                    Label nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    logoImage.SetBinding(Image.SourceProperty, "ImageSource");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                logoImage,
                                name
                            }
                        }
                    };
                })
            };

            Content = new StackLayout
            {
                Children =
                {
                    lv
                }
            };


        }

        public List<Manufacturer> Manufacturers { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Manufacturers = await _restVehicleServices.GetMajorManufacturers();
            
        }
    }
}
