using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFramework.dataPath = "../../../../../Test/Scripts/";
            using (var test = new TestFramework())
            {
                test.Run();


                //test.Luna.DoFile("test_class.luna");
            }
        }
    }
}
