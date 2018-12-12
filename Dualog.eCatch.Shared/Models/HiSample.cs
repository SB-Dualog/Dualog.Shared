namespace Dualog.eCatch.Shared.Models 
{
    public class HiSample
    {
        public int SequenceNumber { get; }
        public int RecordNumber { get; }
        public string RadioCallSignal { get; }

        public string Status { get; private set; }

        public HiSample(string radioCallSignal, int recordNumber, int sequenceNumber, string status = "Y")
        {
            RadioCallSignal = radioCallSignal;
            RecordNumber = recordNumber;
            SequenceNumber = sequenceNumber;
            Status = status;
        }

        public bool Taken => Status.ToUpperInvariant().Equals("Y");
        public void SetNotTaken () => Status = "N";
        public void SetTaken () => Status = "Y";
     
        public override string ToString() => $"{RadioCallSignal}-{RecordNumber}-{SequenceNumber} {Status}";

        public override int GetHashCode() => new { RadioCallSignal, RecordNumber, SequenceNumber }.GetHashCode();

        public override bool Equals(object obj)
        {
            var sample = obj as HiSample;
            if (sample == null) return false;
            return sample.GetHashCode() == GetHashCode();
        }
    }
}
