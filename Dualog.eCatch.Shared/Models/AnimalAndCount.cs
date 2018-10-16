using System;

namespace Dualog.eCatch.Shared.Models
{
    public class AnimalAndCount : IComparable<AnimalAndCount>
    {
        public int Count { get; set; }
        public string Code { get; private set; }

        public AnimalAndCount(string code, int count)
        {
            Code = code;
            Count = count;
        }
        
        public int CompareTo(AnimalAndCount other) => StringComparer.OrdinalIgnoreCase.Compare(Code, other.Code);

        public override string ToString() => $"{Code} {Count}";
    }
}
