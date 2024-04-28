using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
using Sheenam.Api.Models.Foundation.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EFxceptions.Models.Exceptions;

namespace Sheenam.Api.Test.Unit.Services.Foundations.Guests
{
	public partial class GuestServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
		{
			// given
			Guest someGuest = CreateRandomGuest();
			SqlException sqlException = GetSqlError();
			FailedGuestStorageException failedGuestStorageException = new(sqlException);

			GuestDependencyException expectedGuestDependencyException =
				new(failedGuestStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertGuestAsync(someGuest))
				.ThrowsAsync(sqlException);

			// when
			ValueTask<Guest> AddGuestTask =
				this.guestService.AddGuestAsync(someGuest);

			// then
			await Assert.ThrowsAsync<GuestDependencyException>(() => AddGuestTask.AsTask());

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGuestAsync(someGuest),
				Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedGuestDependencyException))),
				Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
		[Fact]
		public async Task ShouldThrowExceptiononAddIfDuplicateKeyErrorOccurs()
		{
			// given
			Guest someGuest = CreateRandomGuest();
			string someString = GetRandomString();

			var duplicateKeyException = new DuplicateKeyException(someString);

			var alreadyExistGuestException =
				new AlreadyExistGuestException(duplicateKeyException);

			var guestDependencyValidationException =
				new GuestDependencyValidationException(alreadyExistGuestException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertGuestAsync(someGuest))
				.ThrowsAsync(duplicateKeyException);

			// when 
			ValueTask<Guest> AddGuestTask =
				this.guestService.AddGuestAsync(someGuest);

			// then
			await Assert.ThrowsAnyAsync<GuestDependencyValidationException>(() =>
			   AddGuestTask.AsTask());

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGuestAsync(someGuest),
				Times.Once());

			this.loggingBrokerMock.Verify(broker =>
			   broker.LogError(It.Is(SameExceptionAs(guestDependencyValidationException))),
			   Times.Once());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
		[Fact]
		public async Task ShouldThrowExceptionOnAddIfServiceErrorOccurs()
		{
			// given
			Guest someGuest = CreateRandomGuest();
			var exception = new Exception();

			var failedGuestServiceException =
				new FailedGuestServiceException(exception);

			var guestDependencyServiceException =
				new GuestDependencyServiceException(failedGuestServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertGuestAsync(someGuest))
				.ThrowsAsync(exception);

			// when
			ValueTask<Guest> AddGuestTask =
				this.guestService.AddGuestAsync(someGuest);

			// then
			await Assert.ThrowsAsync<GuestDependencyServiceException>(() =>
				AddGuestTask.AsTask());

			this.storageBrokerMock.Verify(broker =>
				broker.InsertGuestAsync(someGuest),
				Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(guestDependencyServiceException))),
				Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
