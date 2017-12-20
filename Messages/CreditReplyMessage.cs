using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class CreditReplyMessage
    {
        public int SSN { get; set; }
        public int CreditScore { get; set; }
        public int HistoryLength { get; set; }
    }
}
