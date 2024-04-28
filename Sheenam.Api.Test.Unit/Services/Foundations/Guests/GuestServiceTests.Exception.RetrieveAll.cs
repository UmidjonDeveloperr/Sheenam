using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
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
		public void ShouldThrowExceptionOnRetrieveAllIfSqlErrorOccured()
		{
			// given
			SqlException sqlException = GetSqlError();

			var failedGuestStorageException =
				new FailedGuestStorageException(sqlException);

			var guestDependencyException =
				new GuestDependencyException(failedGuestStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllGuests()).Throws(sqlException);

			// when 
			Action retrieveAllGuestsAction = () => this.guestService.RetrieveAllGuests();

			// then
			Assert.Throws<GuestDependencyException>(retrieveAllGuestsAction);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllGuests(), Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(guestDependencyException))),
					Times.Once());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public void ShouldThowExceptionOnRetrieveAllIfServiceErrorOccured()
		{
			// given
			var exception = new Exception();

			var failedGuestServiceException =
				new FailedGuestServiceException(exception);

			GuestDependencyServiceException guestDependencyServiceException =
				new GuestDependencyServiceException(failedGuestServiceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllGuests()).Throws(exception);

			// when & then
			Assert.Throws<GuestDependencyServiceException>(() =>
				this.guestService.RetrieveAllGuests());

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllGuests(), Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(guestDependencyServiceException))),
					Times.Once());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

	}
}
