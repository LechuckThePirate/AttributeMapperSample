using System;
using AttributeMapperSample.Attributes;

namespace AttributeMapperSample.Classes
{
    [MappableClass(typeof(TestDto))]
    public class TestViewModel
    {
        [MappedProperty(MapTo = "DtoId")]
        public long Id { get; set; }
        [MappedProperty]
        public string Name { get; set; }
        [MappedProperty]
        public DateTime BirthDate { get; set; }
    }
}
