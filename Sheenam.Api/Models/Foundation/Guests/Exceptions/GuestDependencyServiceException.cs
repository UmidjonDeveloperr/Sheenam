using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class GuestDependencyServiceException : Xeption
	{
		public GuestDependencyServiceException(Xeption innerExpection)
			: base(message: "Unexpected service error occured. Contact support",
				  innerExpection)
		{ }
	}
}
