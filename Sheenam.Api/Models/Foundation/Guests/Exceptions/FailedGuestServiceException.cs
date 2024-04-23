using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class FailedGuestServiceException : Xeption
	{
		public FailedGuestServiceException(Exception innerException)
			: base(message: "Unexpected error of guest service occured",
				  innerException)
		{ }
	}
}
