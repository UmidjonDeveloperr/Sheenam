﻿using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
using Sheenam.Api.Models.Foundation.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Sheenam.Api.Test.Unit.Services.Foundations.Guests
{
	public partial class GuestServiceTests
	{
		[Fact]
		public async Task ShouldThrowExceptionOnRetrieveByIdIfSqlErrorOccurredAndLogItAsync()
		{
			// given
			Guid randomId = Guid.NewGuid();
			SqlException sqlException = GetSqlError();

			var failedGuestStorageException =
				new FailedGuestStorageException(sqlException);

			var guestDependencyException =
				new GuestDependencyException(failedGuestStorageException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

			// when
			ValueTask<Guest> RetrieveGuestByIdTask =
				this.guestService.RetrieveGuestByIdAsync(randomId);

			// then
			var actualGuestDependencyException =
				await Assert.ThrowsAsync<GuestDependencyException>(RetrieveGuestByIdTask.AsTask);

			actualGuestDependencyException.Should().BeEquivalentTo(guestDependencyException);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(guestDependencyException))),
				Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
