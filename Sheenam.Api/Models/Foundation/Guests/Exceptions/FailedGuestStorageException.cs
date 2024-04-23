using System;
using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class FailedGuestStorageException : Xeption
	{
		public FailedGuestStorageException(Exception innerException)
			: base(message: "Failed guest storage error occured. Contact support",
				  innerException)
		{ }
	}
}
