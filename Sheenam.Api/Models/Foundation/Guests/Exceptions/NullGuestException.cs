using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class NullGuestException : Xeption
	{
        public NullGuestException()
            : base(message: "Guest is null")
        { }
    }
}
