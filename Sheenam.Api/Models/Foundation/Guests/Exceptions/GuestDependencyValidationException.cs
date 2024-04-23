using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class GuestDependencyValidationException : Xeption
	{
		public GuestDependencyValidationException(Xeption innerException)
			: base(message: "Guest dependency error occured. Fix errors and try again",
				  innerException)
		{ }
	}
}
