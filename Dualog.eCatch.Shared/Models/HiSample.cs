using System;
using System.Collections.Generic;
using System.Text;

namespace Dualog.eCatch.Shared.Models 
{
    public class HISample : IComparable<HISample>
    {
        public int SequenceNumber { get; set; }
        public int RecordNumber { get; set; }
        public int RadioCallSignal { get; set; }
        public HISample(int radioCallSignal, int recordNumber, int sequenceNumber)
        {
            RadioCallSignal = radioCallSignal;
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
        }
        public int CompareTo(HISample other) => SequenceNumber.CompareTo(other.SequenceNumber);

        public override string ToString() => $"{RadioCallSignal}-{RecordNumber}-{SequenceNumber}";

    }
}
