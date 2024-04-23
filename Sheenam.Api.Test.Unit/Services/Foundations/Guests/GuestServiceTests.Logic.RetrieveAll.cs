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
		public void ShouldRetrieveAllGuests()
		{
			// given
			IQueryable<Guest> randomGuests = CreateRandomGuests();
			IQueryable<Guest> storageGuests = CreateRandomGuests();
			IQueryable<Guest> expectedGuests = storageGuests.DeepClone();

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllGuests()).Returns(storageGuests);

			// when
			IQueryable<Guest> ActualGuests =
				this.guestService.RetrieveAllGuests();

			// then 
			ActualGuests.Should().BeEquivalentTo(expectedGuests);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllGuests(), Times.Once());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
