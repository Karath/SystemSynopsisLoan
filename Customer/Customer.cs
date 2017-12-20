using EasyNetQ;
using Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Customer
{
   public class Customer
    {
        private int customerID;
        private string quoteId;
        private int SSN;
        private int timeout = 10000;

        public Customer(int customerID, string quoteId, int SSN)
        {
            this.customerID = customerID;
            this.quoteId = quoteId;
            this.SSN = SSN;
        }

        public void Start()
        {
            SynchronizedWriteLine("Customer running. Waiting for a reply.\n");
            CustomerOrderRequestMessage request = new CustomerOrderRequestMessage
            {
                SSN = SSN,
                
            };

            using (IBus bus = RabbitHutch.CreateBus("host=localhost"))
            {
                // Listen to reply messages from the Broker
                bus.Subscribe<BrokerReplyMessage>("customer" + SSN,
                HandleOrderEvent, x => x.WithTopic(SSN.ToString()));

                // Send an order request message to the Broker
                bus.Send<CustomerOrderRequestMessage>("BrokerQueue", request);

                lock (this)
                {
                    // Block this thread so that the Customer program will not exit.
                    bool gotReply = Monitor.Wait(this, timeout);
                    if (!gotReply)
                        SynchronizedWriteLine(
                            "Timeout. The requested loan is not available!");
                }
            }
        }

        private void HandleOrderEvent(BrokerReplyMessage message)
        {
            StringBuilder reply = new StringBuilder();
            reply.Append("Order reply received by customer:" + SSN + "\n");
            reply.Append("Best Loan Rate: " + message.InterestRate + "\n");
            reply.Append("Order Id: " + message.QuoteID + "\n");
            SynchronizedWriteLine(reply.ToString());

            lock (this)
            {
                // Wake up the blocked Customer thread
                Monitor.Pulse(this);
            }
        }

        private void SynchronizedWriteLine(string s)
        {
            lock (this)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
                Console.ResetColor();
            }
        }

    }
}