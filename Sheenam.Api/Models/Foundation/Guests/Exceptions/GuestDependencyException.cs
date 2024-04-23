using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class GuestDependencyException : Xeption
	{
		public GuestDependencyException(Xeption innerException)
			: base(message: "Guest dependency error occured. Contact support",
				  innerException)
		{ }
	}
}
