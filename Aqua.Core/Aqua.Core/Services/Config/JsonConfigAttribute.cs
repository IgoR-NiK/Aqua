using System;

namespace Aqua.Core.Services
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JsonConfigAttribute : Attribute
    {
        public string Namespace { get; }

        public JsonConfigAttribute()
        {
        }
        
        public JsonConfigAttribute(string @namespace)
        {
            Namespace = @namespace;
        }
    }
}