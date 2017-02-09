using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Todo.Models;
using Xamarin.Forms;

namespace Todo.Views
{
    public class CaptureFlowScrollPage : ContentPage
    {
        private StackLayout _stackLayout;

        public CaptureFlowScrollPage()
        {


            var vehicleCapture = new VehicleCapture();


            var lookupButton = new Button
            {
                Text = "LOOKUP",
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
                    new Entry
                    {
                        Placeholder = "Your number plate",
                        VerticalOptions = LayoutOptions.Start,
                        BindingContext = vehicleCapture.VRM,
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
            throw new NotImplementedException();
        }

        public Page BuildForm()
        {
            throw new NotImplementedException();
        }
    }
}
