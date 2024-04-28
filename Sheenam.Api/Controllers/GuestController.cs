﻿using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundation.Guests.Exceptions;
using Sheenam.Api.Models.Foundation.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Linq;

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

		private string GetCurrentGuest()
		{
			var identity = HttpContext.User.Identity as ClaimsIdentity;

			if (identity != null)
			{
				var userClaims = identity.Claims;

				string Id = userClaims.FirstOrDefault(x => x.Type ==
					ClaimTypes.NameIdentifier)?.Value;

				return Id;
			}

			throw new UnauthorizedAccessException();
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
		
		[HttpGet("{id}")]
		public async ValueTask<ActionResult<Guest>> GetGuestByIdAsync([FromRoute] Guid id)
		{
			try
			{
				var identity = HttpContext.User.Identity as ClaimsIdentity;
				var userClaims = identity.Claims;

				string? authorizedGuestId = userClaims.FirstOrDefault(x => x.Type ==
					ClaimTypes.NameIdentifier)?.Value;

				

				Guest currentGuest = await this.guestService.RetrieveGuestByIdAsync(id);
				return Ok(currentGuest);
			}
			catch (GuestValidationException guestValidationException)
			{
				return BadRequest(guestValidationException.InnerException);
			}
			catch (GuestDependencyException guestDependencyException)
			{
				return InternalServerError(guestDependencyException.InnerException);
			}
			catch (GuestDependencyServiceException guestDependencyServiceException)
			{
				return InternalServerError(guestDependencyServiceException.InnerException);
			}
		}
	}
}
