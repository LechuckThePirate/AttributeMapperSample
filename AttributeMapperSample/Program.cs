using System;
using AttributeMapperSample.Classes;

namespace AttributeMapperSample
{
    public class Program
    {

        private static void RunTest()
        {

            Console.WriteLine("{0}.IsMappableWith({1})         : {2}",
                typeof(TestViewModel).Name, 
                typeof(TestDto).Name, 
                typeof(TestViewModel).IsMappableWith(typeof(TestDto)));

            var dto = new TestDto()
            {
                DtoId = 20,
                BirthDate = DateTime.Today,
                Name = "TestSubject"
            };
            Console.WriteLine("Sample object               : {0} / {1} / {2}", dto.DtoId, dto.Name, dto.BirthDate);

            var vm = dto.MapTo<TestViewModel>();
            Console.WriteLine("TestDTo > TestViewModel     : {0} / {1} / {2}",vm.Id, vm.Name,vm.BirthDate);

            vm.Name += "(Modified)";
            Console.WriteLine("Changed name to '{0}'.", vm.Name);
            var newDto = vm.MapTo<TestDto>();
            Console.WriteLine("TestViewModel > new TestDto : {0} / {1} / {2}",newDto.DtoId,newDto.Name,newDto.BirthDate);
        }

        static void Main(string[] args)
        {
            RunTest();
            Console.WriteLine();
            Console.WriteLine("** End of test ** Press any key **A smap");
            Console.Read();
        }
    }
}
