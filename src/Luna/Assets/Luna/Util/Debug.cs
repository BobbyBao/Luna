using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_2018_1_OR_NEWER

#else

namespace UnityEngine
{
}

namespace SharpLuna
{
    public class Debug
    {
        public static void Log(object message)
        {
            Console.WriteLine(message);
        }

        public static void LogWarning(object message)
        {
            Console.WriteLine(message);
        }

        public static void LogError(object message)
        {
            Console.WriteLine(message);
        }

        public static void Assert(bool condition, string message = "")
        {
            System.Diagnostics.Debug.Assert(condition, message);
        }
    }
}

#endif