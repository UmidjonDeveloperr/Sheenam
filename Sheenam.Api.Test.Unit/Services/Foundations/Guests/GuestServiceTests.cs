﻿using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Test.Unit.Services.Foundations.Guests
{
	public partial class GuestServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly IGuestService guestService;

		public GuestServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.guestService =
				new GuestService(storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object);
		}

		private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
			actualException => actualException.SameExceptionAs(expectedException);

		private static int GetRandomNumber() =>
			new IntRange(min: 0, max: 9).GetValue();

		private static T GetInvalidEnum<T>()
		{
			int randomNumber = GetRandomNumber();

			while (Enum.IsDefined(typeof(T), randomNumber) is true)
			{
				randomNumber = GetRandomNumber();
			}

			return (T)(object)randomNumber;
		}

		private static SqlException GetSqlError() =>
			(SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

		private static string GetRandomString() =>
			new MnemonicString().GetValue().ToString();

		private static IQueryable<Guest> CreateRandomGuests() =>
			CreateGuestFiller(date: GetRandomDateTimeOffset)
			.Create(count: GetRandomNumber()).AsQueryable<Guest>();

		private static Guest CreateRandomGuest() =>
			CreateGuestFiller(date: GetRandomDateTimeOffset).Create();

		private static DateTimeOffset GetRandomDateTimeOffset =>
			new DateTimeRange(earliestDate: new DateTime()).GetValue();

		private static Filler<Guest> CreateGuestFiller(DateTimeOffset date)
		{
			var filler = new Filler<Guest>();
			filler.Setup()
				.OnType<DateTimeOffset>().Use(date);

			return filler;
		}
	}
}
