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
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		public async Task ShouldThrowValidationExceptionOnAddIfGuestIsInvalidDataAndLogItAsync(string invalidData)
		{
			// given
			var invalidGuest = new Guest()
			{
				Firstname = invalidData
			};

			InvalidGuestException invalidGuestException = new();

			invalidGuestException.AddData(key: nameof(Guest.Id),
				values: "Id is required");

			invalidGuestException.AddData(key: nameof(Guest.Firstname),
				values: "Text is invalid");

			invalidGuestException.AddData(key: nameof(Guest.Lastname),
			   values: "Text is invalid");

			invalidGuestException.AddData(key: nameof(Guest.DateOfBirth),
			  values: "Date is invalid");

			invalidGuestException.AddData(key: nameof(Guest.Email),
			   values: "Text is invalid");

			invalidGuestException.AddData(key: nameof(Guest.Address),
				values: "Text is invalid");

			var expectedGuestValidationExpected =
				new GuestValidationException(invalidGuestException);

			// when
			ValueTask<Guest> addGuestTask =
			   this.guestService.AddGuestAsync(invalidGuest);

			// then
			await Assert.ThrowsAsync<GuestValidationException>(() => addGuestTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
			  broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationExpected))),
			  Times.Once());

			this.storageBrokerMock.Verify(broker =>
			  broker.InsertGuestAsync(It.IsAny<Guest>()),
			  Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
		[Fact]
		public async Task ShouldThrowExceptionOnAddIfGenderIsInvalidAndLogItAsync()
		{
			// given
			Guest randomGuest = CreateRandomGuest();
			Guest invalidGuest = randomGuest;

			invalidGuest.Gender = GetInvalidEnum<GenderType>();
			var invalidGuestException = new InvalidGuestException();

			invalidGuestException.AddData(key: nameof(Guest.Gender),
				values: "Value is invalid");

			var expectedGuestValidationException =
				new GuestValidationException(invalidGuestException);

			// when
			ValueTask<Guest> AddGuestTask =
				this.guestService.AddGuestAsync(invalidGuest);

			// then
			await Assert.ThrowsAsync<GuestValidationException>(() =>
			   AddGuestTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
			   broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
			   Times.Once());

			this.storageBrokerMock.Verify(broker =>
			   broker.InsertGuestAsync(It.IsAny<Guest>()),
			   Times.Never());

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
