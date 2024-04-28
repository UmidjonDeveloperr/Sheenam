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
		public async Task ShouldRetrieveGuestById()
		{
			// given
			Guid randomId = Guid.NewGuid();
			Guest randomGuest = CreateRandomGuest();
			Guest storageGuest = randomGuest;
			Guest expectedGuest = storageGuest.DeepClone();

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGuestByIdAsync(randomId))
				.ReturnsAsync(storageGuest);

			// when
			Guest actualGuest =
				await this.guestService.RetrieveGuestByIdAsync(randomId);

			// then 
			actualGuest.Should().BeEquivalentTo(expectedGuest);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>()),
				Times.Once());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
