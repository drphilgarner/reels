using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Todo.Models
{
    public class Manufacturer
    {
        public string Name { get; set; }

        public string LogoUri { get; set; }

        public ImageSource ImageSource => ImageSource.FromUri(new Uri(LogoUri));
    }
}
