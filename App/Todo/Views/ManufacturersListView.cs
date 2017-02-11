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
        private readonly CaptureFlowScrollPage _senderPage;

        private readonly RestVehicleServices _restVehicleServices;
        private ListView _listView;
        

        public ManufacturersListView(CaptureFlowScrollPage senderPage)
        {
            _senderPage = senderPage;
            _restVehicleServices = new RestVehicleServices();
        }

        private void BuildLayout()
        {
            _listView = new ListView
            {
                ItemsSource = Manufacturers,
                RowHeight = 100,
                //SeparatorVisibility = SeparatorVisibility.None,
                BackgroundColor = Color.White,
                
                //VerticalOptions = LayoutOptions.FillAndExpand,

                ItemTemplate = new DataTemplate(() =>
                {
                    Image logoImage = new Image { Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                    Label nameLabel = new Label {VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand};
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    logoImage.SetBinding(Image.SourceProperty, "ImageSource");

                    return new ViewCell
                    {
                        
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Vertical,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                logoImage,
                                nameLabel
                            }
                        }
                    };
                })
            };

            _listView.ItemSelected += _listView_ItemSelected;

            Content = new StackLayout
            {
                Children =
                {
                    _listView
                }
            };
        }

        private async void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            MessagingCenter.Send(_senderPage, "SelectedManufacturer", ((Manufacturer)_listView.SelectedItem).Name);


            await Navigation.PopAsync();
        }

        public List<Manufacturer> Manufacturers { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Manufacturers = await _restVehicleServices.GetMajorManufacturers();

            

            BuildLayout();

        }
    }
}
