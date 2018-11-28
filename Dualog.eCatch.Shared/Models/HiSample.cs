using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Dualog.eCatch.Shared.Models 
{
    public class HiSample
    {
        public int SequenceNumber { get; set; }
        public int RecordNumber { get; set; }
        public string RadioCallSignal { get; set; }

        public string Status { get; set; } = "N";

        public HiSample(string radioCallSignal, int recordNumber, int sequenceNumber)
        {
            RadioCallSignal = radioCallSignal;
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
        }

        public bool Delivered => Status.ToUpperInvariant().Equals("Y");
     
        public override string ToString() => $"{RadioCallSignal}-{RecordNumber}-{SequenceNumber} {Status}";

        public override int GetHashCode()
        {
            return new {RadioCallSignal, RecordNumber, SequenceNumber}.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var sample = obj as HiSample;
            if (sample == null) return false;
            return sample.GetHashCode() == GetHashCode();
        }
    }
}
