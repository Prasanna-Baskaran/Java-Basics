using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Hello!!! You have entered: ");
            if (args != null)
            {
                foreach (string str in args)
                {
                    Console.Write(str);
                    Console.Write(" ");
                }
            }
        }
    }
}
