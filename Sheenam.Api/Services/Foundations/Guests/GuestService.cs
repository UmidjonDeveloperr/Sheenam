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

		public async ValueTask<Guest> AddGuestAsync(Guest guest)
		{
			return await this.storageBroker.InsertGuestAsync(guest);
		}

		public IQueryable<Guest> RetrieveAllGuests()
		{
			return this.storageBroker.SelectAllGuests();
		}

		public async ValueTask<Guest> RetrieveGuestByIdAsync(Guid id)
		{
			return await this.storageBroker.SelectGuestByIdAsync(id);
		}

		public async ValueTask<Guest> ModifyGuestAsync(Guest guest)
		{
			return await this.storageBroker.UpdateGuestAsync(guest);
		}

		public async ValueTask<Guest> RemoveGuestAsync(Guid id)
		{
			Guest getGuest = await this.storageBroker.SelectGuestByIdAsync(id);

			return await this.storageBroker.DeleteGuestAsync(getGuest);
		}

	}
}
