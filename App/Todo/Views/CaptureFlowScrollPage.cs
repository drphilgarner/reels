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
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness (5.0d)
                
            };
            lookupButton.Clicked += LookupButton_Clicked;

            this.Content = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,

                Content = new StackLayout
                {
                    Children =
                    {
                        new Entry
                        {
                            Placeholder = "Your number plate",
                            VerticalOptions = LayoutOptions.Start,
                            BindingContext = vehicleCapture.VRM
                        },
                        lookupButton
                    }
                }


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
