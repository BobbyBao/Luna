using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var test = new TestFramework())
            {
                test.Run();
            }
        }
    }
}
