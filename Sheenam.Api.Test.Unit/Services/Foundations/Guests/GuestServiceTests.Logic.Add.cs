using FluentAssertions;
using Force.DeepCloner;
using Moq;
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
		public async Task ShouldAddGuestAsync()
		{
			// given
			Guest randomGuest = CreateRandomGuest();
			Guest inputGuest = randomGuest;
			Guest storageGuest = inputGuest;
			Guest expectedGuest = storageGuest.DeepClone();

			this.storageBrokerMock.Setup(broker =>
			  broker.InsertGuestAsync(inputGuest))
				.ReturnsAsync(expectedGuest);

			// when 
			Guest actualGuest =
				await this.guestService.AddGuestAsync(inputGuest);

			// then
			actualGuest.Should().BeEquivalentTo(expectedGuest);

			this.storageBrokerMock.Verify(broker =>
			  broker.InsertGuestAsync(It.IsAny<Guest>()), Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
