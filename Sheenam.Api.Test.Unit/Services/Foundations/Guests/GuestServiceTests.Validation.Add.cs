using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
using Moq;

namespace Sheenam.Api.Test.Unit.Services.Foundations.Guests
{
	public partial class GuestServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfGuestIsNullAndLogItAsync()
		{
			// given
			Guest nullGuest = null;
			NullGuestException nullGuestException = new();

			GuestValidationException expectedGuestValidationException =
			new(nullGuestException);

			// when
			ValueTask<Guest> addGuestTask =
				this.guestService.AddGuestAsync(nullGuest);

			// then
			await Assert.ThrowsAsync<GuestValidationException>(() =>
			  addGuestTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
				Times.Once());

			this.storageBrokerMock.Verify(broker =>
			 broker.InsertGuestAsync(It.IsAny<Guest>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
