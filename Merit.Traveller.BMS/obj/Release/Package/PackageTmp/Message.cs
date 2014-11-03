using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Merit.Traveller.BMS
{
    public class Message
    {
        public string ID { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string received { get; set; }
        public string contents { get; set; }
        public string read { get; set; }

        public Message(string ID, string from, string subject, string received, string contents, string read)
        {
            this.ID = ID;
            this.from = from;
            this.subject = subject;
            this.received = received;
            this.contents = contents;
            this.read = read;
        }

    }
}