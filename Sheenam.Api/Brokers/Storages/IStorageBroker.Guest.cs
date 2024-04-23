using Sheenam.Api.Models.Foundation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sheenam.Api.Brokers.Storages
{
	public partial interface IStorageBroker
	{
		ValueTask<Guest> InsertGuestAsync(Guest guest);
		IQueryable<Guest> SelectAllGuests();
		ValueTask<Guest> SelectGuestByIdAsync(Guid Id);
		ValueTask<Guest> UpdateGuestAsync(Guest guest);
		ValueTask<Guest> DeleteGuestAsync(Guest guest);
	}
}
