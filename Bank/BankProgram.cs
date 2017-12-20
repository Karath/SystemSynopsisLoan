using System;
using System.Threading.Tasks;

namespace Bank
{
    class BankProgram
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() => new Bank("Western Union", 3, 6));
            Task.Factory.StartNew(() => new Bank("Dansk", 6, 5));
            Task.Factory.StartNew(() => new Bank("Bank of Wales", 2, 3));

            Console.ReadLine();
        }
    }
}
