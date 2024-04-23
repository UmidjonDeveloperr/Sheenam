using Sheenam.Api.Models.Foundation;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Sheenam.Api.Services.Foundations.Guests
{
	public interface IGuestService
	{
		ValueTask<Guest> AddGuestAsync(Guest guest);
		IQueryable<Guest> RetrieveAllGuests();
		ValueTask<Guest> RetrieveGuestByIdAsync(Guid id);
		ValueTask<Guest> ModifyGuestAsync(Guest guest);
		ValueTask<Guest> RemoveGuestAsync(Guid id);
	}
}
