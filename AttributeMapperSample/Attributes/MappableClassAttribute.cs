using System;

namespace AttributeMapperSample.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MappableClassAttribute : Attribute
    {
        public Type TargetType { get; set; }
        public Type SourceType { get; set; }

        public MappableClassAttribute(Type sourceAndTargetType)
        {
            TargetType = sourceAndTargetType;
            SourceType = sourceAndTargetType;
        }

        public MappableClassAttribute() { }

    }
}
