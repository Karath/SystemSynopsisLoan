using System;
using EasyNetQ;
using Messages;
using System.Threading;

namespace Credit
{
    class BureauProgram
    {
        static void Main(string[] args)
        {
            new Bureau().Start();
        }
    }
}
