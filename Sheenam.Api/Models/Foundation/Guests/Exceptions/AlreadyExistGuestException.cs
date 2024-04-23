using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class AlreadyExistGuestException : Xeption
	{
		public AlreadyExistGuestException(Exception innerException)
			: base(message: "Guest is already exist. Please try again",
				  innerException)
		{ }
	}
}
