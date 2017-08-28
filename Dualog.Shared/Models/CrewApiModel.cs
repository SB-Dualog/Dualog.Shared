using System;
using Dualog.Shared.Enums;

namespace Dualog.Shared.Models
{
    public class CrewApiModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeCountry { get; set; }
        public string PassportId { get; set; }
        public string VisaNo { get; set; }
        public Rank Rank { get; set; }
        public bool IsCaptain => Rank == Rank.Captain;
        public string PecNumber { get; set; }
        public string MaritimeTransportBookNumber { get; set; }

    }
}
