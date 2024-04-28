using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class ForbiddenGuestException : Xeption
	{
        public ForbiddenGuestException()
            : base(message: "You don't have permission to access this recourse")
        { }
    }
}
