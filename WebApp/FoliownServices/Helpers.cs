using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FoliownServices
{
    public static class Helpers
    {
        public static T Merge<T>(T target, T source)
        {
            typeof(T)
                .GetRuntimeProperties()
                .Select((PropertyInfo x) => new KeyValuePair<PropertyInfo, object>(x, x.GetValue(source, null)))
                .Where((KeyValuePair<PropertyInfo, object> x) => x.Value != null).ToList()
                .ForEach((KeyValuePair<PropertyInfo, object> x) => x.Key.SetValue(target, x.Value, null));

            //return the modified copy of Target
            return target;
        }
    }
}