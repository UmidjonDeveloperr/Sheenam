using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundation.Guests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
	{
		private readonly IStorageBroker storageBroker;
		private readonly ILoggingBroker loggingBroker;

		public GuestService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
		{
			this.storageBroker = storageBroker;
			this.loggingBroker = loggingBroker;
		}

		public ValueTask<Guest> AddGuestAsync(Guest guest) =>
			TryCatch(async () =>
			{
				ValidateGuestOnAdd(guest);
				guest.CreatedDate = DateTimeOffset.UtcNow;
				guest.UpdatedDate = DateTimeOffset.UtcNow;

				return await this.storageBroker.InsertGuestAsync(guest);
			});

		public IQueryable<Guest> RetrieveAllGuests() =>
			TryCatch(() => this.storageBroker.SelectAllGuests());

		public ValueTask<Guest> RetrieveGuestByIdAsync(Guid id) =>
			TryCatch(async () =>
			{
				ValidateGuestId(id);
				return await this.storageBroker.SelectGuestByIdAsync(id);
			});

		public ValueTask<Guest> ModifyGuestAsync(Guest guest) =>
			TryCatch(async () =>
			{
				ValidateGuestOnModify(guest);

				return await this.storageBroker.UpdateGuestAsync(guest);
			});

		public ValueTask<Guest> RemoveGuestAsync(Guid id) =>
			TryCatch(async () =>
			{
				ValidateGuestId(id);

				Guest gettingGuest =
					await this.storageBroker.SelectGuestByIdAsync(id);

				return await this.storageBroker.DeleteGuestAsync(gettingGuest);
			});

	}
}
