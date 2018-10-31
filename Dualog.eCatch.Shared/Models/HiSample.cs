using System;
using System.Collections.Generic;
using System.Text;

namespace Dualog.eCatch.Shared.Models 
{
    public class HiSample
    {
        public int SequenceNumber { get; set; }
        public int RecordNumber { get; set; }
        public string RadioCallSignal { get; set; }
        public HiSample(string radioCallSignal, int recordNumber, int sequenceNumber)
        {
            RadioCallSignal = radioCallSignal;
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
        }
        public override string ToString() => $"{RadioCallSignal}-{RecordNumber}-{SequenceNumber}";

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
