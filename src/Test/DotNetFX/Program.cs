using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFramework.dataPath = "../../../../Test/Scripts/";
            /*
            using (var test = new TestFramework())
            {
                //test.Luna.DoFile("test_perf.luna");
                test.Luna.DoFile("test.luna");
            }*/

            Application.Run(new Test.TableTree());
        }
    }
}
