using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System;
using Xeptions;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
	{
		private delegate ValueTask<Guest> ReturningGuestFunction();
		private delegate IQueryable<Guest> ReturningGuestsFunction();

		private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
		{
			return await returningGuestFunction();
		}

		private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
		{
			return returningGuestsFunction();
		}

	}
}
