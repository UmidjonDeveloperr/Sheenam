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
			catch (InvalidGuestException invalidGuestException)
			{
				throw CreateAndLogValidationException(invalidGuestException);
			}
			catch (SqlException sqlException)
			{
				var failedGuestStorageException =
					new FailedGuestStorageException(sqlException);

				throw CreateAndLogCriticalException(failedGuestStorageException);
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

		private GuestDependencyException CreateAndLogCriticalException(Xeption exception)
		{
			var guestDependencyException = new GuestDependencyException(exception);
			this.loggingBroker.LogCritical(guestDependencyException);
			return guestDependencyException;
		}

	}
}
