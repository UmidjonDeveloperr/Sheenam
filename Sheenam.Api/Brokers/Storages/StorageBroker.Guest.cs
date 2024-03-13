using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundation.Guests;

namespace Sheenam.Api.Brokers.Storages
{
	public partial class StorageBroker
	{
		public DbSet<Guest> Guests { get; set; }
	}
}
