using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public enum TestEnum
    {
        A,B,C,D
    }

    public struct TestStruct
    {
        public float x, y, z;

        public TestStruct(float x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
        }

        public TestStruct(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public TestStruct(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float mag = 1 / (float)Math.Sqrt(x * x + y * y + z * z);
            x *= mag;
            y *= mag;
            z *= mag;
        }
    }

    public class TestClass
    {
        public const int constVar = 5;
        public static string staticProp { get; set; } = "Static Prop";
        public static string staticVar = "Static var";

        public static void StaticFunc()
        {
            Console.WriteLine("StaticFunc");
        }

        public static void StaticFunc1(string text)
        {
            Console.WriteLine("StaticFunc1 : " + text);
        }

        public static void StaticFunc2(string text, int intParam)
        {
            Console.WriteLine("StaticFunc2 : " + text + ", " + intParam);
        }


        public string variable = "Default variable";
        public string name { get; set; } = "Default Name";

        public TestClass Child { get; set; }
        public event Action testEvent;

        Dictionary<int, string> testString = new Dictionary<int, string>();

        int[] intArray = new int[100];

        public TestClass()
        {
        }

        public void CreateChild()
        {
            Child = new TestClass();
        }

        public string this [int index]
        {
            get
            {
                return testString[index];
            }

            set
            {
                testString[index] = value;
            }
        }

        public int[] IntArray => intArray;


        public void Method()
        {
            Console.WriteLine("Method");
        }

        public void Method1(string text)
        {
            Console.WriteLine("Method1 : " + text);
        }

        public void Method2(string text, float floatParam)
        {
            Console.WriteLine("Method2 : " + text + ", " + floatParam);
        }


        public string Fun0()
        {
            Console.WriteLine("Fun0");
            return "Fun0";
        }

        public string Fun1(int param1)
        {
            Console.WriteLine("Fun1 : " + param1);
            return "Fun1";
        }

        public float Fun2(int param1)
        {
            Console.WriteLine("Fun2 : " + param1);
            return 1.5f * param1;
        }

        public override string ToString()
        {
            return "TestClass";
        }
    }
}
