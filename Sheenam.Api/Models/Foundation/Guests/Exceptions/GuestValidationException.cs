﻿using Xeptions;

namespace Sheenam.Api.Models.Foundation.Guests.Exceptions
{
	public class GuestValidationException : Xeption
	{
		public GuestValidationException(Xeption innerException)
			: base(message: "Guest validation error occured, fix the errors and try again",
				  innerException)
		{ }
	}
}
