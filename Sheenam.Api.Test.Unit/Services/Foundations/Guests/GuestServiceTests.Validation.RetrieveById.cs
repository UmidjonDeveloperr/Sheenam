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
		public async Task ShouldThrowExceptionOnRetrieveByIdIfGuestIdIsInvalidAndLogItAsync()
		{
			// given
			Guid invalidGuestId = Guid.Empty;
			var invalidGuestException = new InvalidGuestException();
			invalidGuestException.AddData(key: nameof(Guest.Id), values: "Id is required");

			var guestValidationException =
				new GuestValidationException(invalidGuestException);

			// when
			ValueTask<Guest> RetrieveGuestByIdTask =
				this.guestService.RetrieveGuestByIdAsync(invalidGuestId);

			var actualGuestValidationException =
				await Assert.ThrowsAsync<GuestValidationException>(RetrieveGuestByIdTask.AsTask);

			// then 
			actualGuestValidationException.Should().BeEquivalentTo(guestValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(guestValidationException))),
				Times.Once());

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Never());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
