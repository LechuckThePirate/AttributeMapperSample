using System;

namespace AttributeMapperSample.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MappableClassAttribute : Attribute
    {
        private readonly Type _associatedType;
        public Type AssociatedType { get { return _associatedType; } }

        public MappableClassAttribute(Type sourceAndTargetType)
        {
            _associatedType = sourceAndTargetType;
        }
    }
}
