using Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broker
{
    class MessageTransformers
    {
        public static CustomerOrderRequestMessage FilterCustomerOrderRequestMessage(CustomerOrderRequestMessage message)
        {
            return new CustomerOrderRequestMessage
            {
                SSN = message.SSN
            };
        }

        public static OrderBroadcastRequestMessage EnrichOrderRequestMessage( int creditScore, int historyLength, int loanAmount, int loanterm, string replyTo)
        {
            return new OrderBroadcastRequestMessage
            {
                CreditScore = creditScore,
                HistoryLength = historyLength,
                LoanAmount = loanAmount,
                LoanTerm = loanterm,
                ReplyTo = replyTo

            };
        }
    }
}