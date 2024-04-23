using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System;
using Xeptions;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
	{
		private delegate ValueTask<Guest> ReturningGuestFunction();
		private delegate IQueryable<Guest> ReturningGuestsFunction();

		private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
		{
			try
			{
				return await returningGuestFunction();
			}
			catch (NullGuestException nullGuestException)
			{
				throw CreateAndLogValidationException(nullGuestException);
			}
		}

		private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
		{
			return returningGuestsFunction();
		}

		private GuestValidationException CreateAndLogValidationException(Xeption exception)
		{
			var guestValidationException = new GuestValidationException(exception);
			this.loggingBroker.LogError(guestValidationException);
			return guestValidationException;
		}

	}
}
