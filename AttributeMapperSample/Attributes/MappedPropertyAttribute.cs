using System;

namespace AttributeMapperSample.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappedPropertyAttribute : Attribute
    {
        public string MapTo { get; set; }
        public Type MapperClass { get; set; }
    }
}
