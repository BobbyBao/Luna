using SharpLuna;
using System;
using System.Collections.Generic;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFramework.dataPath = "../../../../../Test/Scripts/";
            
            using (var test = new TestFramework())
            {
                //test.Run();

                test.Luna.DoFile("test.luna");
                //test.Luna.DoFile("test_class.luna");
                //test.Luna.DoFile("test_perf.luna");
            }
        }
    }
}
