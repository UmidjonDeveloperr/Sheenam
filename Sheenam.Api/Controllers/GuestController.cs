using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using System.Threading.Tasks;

namespace Sheenam.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GuestController : RESTFulController
	{
		private readonly IGuestService guestService;

		public GuestController(IGuestService guestService)
		{
			this.guestService = guestService;
		}
		[HttpPost]
		public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
		{
			try
			{
				Guest postedGuest = await this.guestService.AddGuestAsync(guest);

				return Created(postedGuest);
			}
			catch (GuestValidationException guestValidationException)
			{

				return BadRequest(guestValidationException);
			}
			catch (GuestDependencyValidationException guestDependencyValidationException)
				when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
			{
				return Conflict(guestDependencyValidationException);
			}
			catch (GuestDependencyValidationException guestDependencyValidationException)
			{
				return BadRequest(guestDependencyValidationException.InnerException);
			}
			catch (GuestDependencyException guestDependencyException)
			{
				return InternalServerError(guestDependencyException.InnerException);
			}
			catch (GuestDependencyServiceException guestServiceException)
			{
				return InternalServerError(guestServiceException.InnerException);
			}
		}

	}
}
