using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class InvalidGuestException : Xeption
	{
		public InvalidGuestException()
			: base(message: "Guest is invalid")
		{ }
	}
}
