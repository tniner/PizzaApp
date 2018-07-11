using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process();

            process.LoadJson("http://files.olo.com/pizzas.json");

            Console.ReadLine();
            
        }
    }
}
