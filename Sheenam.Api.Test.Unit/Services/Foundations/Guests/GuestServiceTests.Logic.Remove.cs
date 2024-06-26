﻿using FluentAssertions;
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
		public async Task ShouldRemoveGuestAsync()
		{
			// given
			Guid randomGuestId = Guid.NewGuid();
			Guest randomGuest = CreateRandomGuest();
			Guest storageGuest = randomGuest.DeepClone();
			Guest deletedGuest = randomGuest.DeepClone();
			Guest expectedDeletedGuest = deletedGuest.DeepClone();

			this.storageBrokerMock.Setup(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(storageGuest);

			this.storageBrokerMock.Setup(broker =>
				broker.DeleteGuestAsync(storageGuest))
				.ReturnsAsync(deletedGuest);

			// when
			Guest actualDeletedGuest =
				await this.guestService.RemoveGuestAsync(randomGuestId);

			// then
			actualDeletedGuest.Should().BeEquivalentTo(expectedDeletedGuest);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once());

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteGuestAsync(It.IsAny<Guest>()), Times.Once());

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
