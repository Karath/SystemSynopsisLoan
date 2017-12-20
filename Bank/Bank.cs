using EasyNetQ;
using Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Bank
{
    class Bank
    {
        private int interestRate;
        private string bankName;
        private int maxLoanTerm;
        private IBus bus = null;


        public Bank(string bankName, int interestRate, int maxLoanTerm)
        {
            this.bankName = bankName;
            this.interestRate = interestRate;
            this.maxLoanTerm = maxLoanTerm;
        }

        public void Start()
        {
            using (bus = RabbitHutch.CreateBus("host=localhost"))
            {
                // Listen for order request messages published to this warehouse only
                // using Topic Based Routing

                //bus.Subscribe<OrderRequestMessageToLocalWarehouse>("warehouse" + id,
                //    HandleOrderEvent, x => x.WithTopic(country));

                // Listen for order request messages published by the retailer
                // to all warehouses. Notice that this subscriber subscribes to
                // another type of message (OrderBroadcastRequestMessage) than
                // the subscriber above (OrderRequestMessageToLocalWarehouse).
                // If they subscribed to the same message type, a pair of
                // subscribers with the same warehouse id would listen on the
                // same queue. 
                bus.Subscribe<OrderBroadcastRequestMessage>("Bank" + bankName,
                    HandleOrderEvent);

                SynchronizedWriteLine("Bank " + bankName + " Listening for order requests\n");

                lock (this)
                {
                    // Block this thread so that the Warehouse program will not exit.
                    Monitor.Wait(this);
                }
            }
        }

        private void HandleOrderEvent(BankQuoteRequestMessage message)
        {
            SynchronizedWriteLine("Order request received:\n" +
                "customer history lengh: " + message.HistoryLength + "\n" +
                "customer SSN: " + message.SSN + "\n" +
                "loan amount: " + message.LoanAmount + "\n" +
                "loan duration: " + message.LoanTerm
                );

            if (maxLoanTerm > message.LoanTerm)
            {
                BankReplyMessage replyMessage = new BankReplyMessage
                {
                    QuoteID = "gfdgdf",
                    InterestRate = 4
                };

                // Send the reply message to the broker
                bus.Send(message.ReplyTo, replyMessage);
                SynchronizedWriteLine("Reply sent back to retailer");
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

