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
	}
}
