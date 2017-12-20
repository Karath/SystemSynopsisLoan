using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
   public class CustomerOrderRequestMessage
    {
        public int SSN { get; set; }

    //public string Country { get; set; }
}

public class BankQuoteRequestMessage : CustomerOrderRequestMessage
    {
    public int CreditScore { get; set; }
    public int HistoryLength { get; set; }
    public int LoanAmount { get; set; }
    public int LoanTerm { get; set; }
    public string ReplyTo { get; set; }
}

public class CreditBureauRequestMessage : CustomerOrderRequestMessage
    {
    public int CreditScore { get; set; }
    public int HistoryLength { get; set; }
}


//public class OrderRequestMessageToLocalWarehouse : RetailerOrderRequestMessage
//{
//}

public class OrderBroadcastRequestMessage : BankQuoteRequestMessage
    {
}
}