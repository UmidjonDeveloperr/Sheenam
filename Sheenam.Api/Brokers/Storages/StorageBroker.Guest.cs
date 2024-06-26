﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using Sheenam.Api.Models.Foundation.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
	{
		public DbSet<Guest> Guests { get; set; }

		public async ValueTask<Guest> InsertGuestAsync(Guest guest) =>
			await InsertAsync(guest);

		public IQueryable<Guest> SelectAllGuests() =>
			SelectAll<Guest>();

		public async ValueTask<Guest> SelectGuestByIdAsync(Guid id) =>
			await SelectAsync<Guest>(id);

		public async ValueTask<Guest> UpdateGuestAsync(Guest guest) =>
			await UpdateAsync(guest);

		public async ValueTask<Guest> DeleteGuestAsync(Guest guest) =>
			await DeleteAsync(guest);
	}
}
