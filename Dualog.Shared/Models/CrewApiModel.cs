using System;

namespace Dualog.Shared.Models
{
    public class CrewApiModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeCountry { get; set; }
        public string PassportId { get; set; }
        public string VisaNo { get; set; }
        public string Rank { get; set; }
    }
}
