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
        private string _name;

        public string Name
        {
            get { return _name.Replace("_"," "); }
            set { _name = value; }
        }

        public string LogoUri { get; set; }

        public ImageSource ImageSource => ImageSource.FromUri(new Uri(LogoUri));
    }
}
