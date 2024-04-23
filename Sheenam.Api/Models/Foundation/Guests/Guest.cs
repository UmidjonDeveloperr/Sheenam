using System;

namespace Sheenam.Api.Models.Foundation.Guests
{
    public class Guest
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public GenderType Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
