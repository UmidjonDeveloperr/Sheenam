﻿using EFxceptions.Models.Exceptions;
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
			catch (DuplicateKeyException duplicateKeyException)
			{
				var alreadyExistGuestException =
					new AlreadyExistGuestException(duplicateKeyException);

				throw CreateAndLogDuplicateKeyException(alreadyExistGuestException);
			}
			catch (Exception exception)
			{
				var failedGuestServiceException =
					new FailedGuestServiceException(exception);

				throw CreateAndLogGuestDependencyServiceException(failedGuestServiceException);
			}
		}

		private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
		{
			try
			{
				return returningGuestsFunction();
			}
			catch (SqlException sqlException)
			{
				var failedGuestStorageException =
					new FailedGuestStorageException(sqlException);

				throw CreateAndLogCriticalException(failedGuestStorageException);
			}
			catch (Exception exception)
			{
				var failedGuestServiceException =
					new FailedGuestServiceException(exception);

				throw CreateAndLogGuestDependencyServiceException(failedGuestServiceException);
			}
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

		private GuestDependencyValidationException CreateAndLogDuplicateKeyException(Xeption exception)
		{
			var guestDependencyValidationException = new GuestDependencyValidationException(exception);
			this.loggingBroker.LogError(guestDependencyValidationException);
			return guestDependencyValidationException;
		}

		private GuestDependencyServiceException CreateAndLogGuestDependencyServiceException(Xeption exception)
		{
			var guestDependencyServiceException = new GuestDependencyServiceException(exception);
			this.loggingBroker.LogCritical(guestDependencyServiceException);
			return guestDependencyServiceException;
		}

	}
}
