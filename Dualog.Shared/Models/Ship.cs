namespace Dualog.Shared.Models
{
    public class Ship
    {
        public string Name { get; set; }
        public string RadioCallSignal { get; set; }
        public string RegistrationNumber { get; set; }
        public int RecordNumberStart { get; set; }
        public int SequenceNumberXEU { get; set; }
        public int SequenceNumberFRO { get; set; }
        public int SequenceNumberRUS { get; set; }
        public int SequenceNumberXNE { get; set; }
        public int SequenceNumberISL { get; set; }
        public string DisplayName => $"{Name} ({RadioCallSignal})";
        public bool RecordNumberStartIsUnknown { get; set; }

        public Ship(string name, string radioCallSignal, string registrationNumber)
        {
            Name = name;
            RadioCallSignal = radioCallSignal;
            RegistrationNumber = registrationNumber;
        }
    }
}
