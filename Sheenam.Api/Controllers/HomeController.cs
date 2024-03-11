//=================================================
//Copyright (c) Coalition of Good-Hearted Ingineers
//Free to Use Comfort and Peace
//=================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Sheenam.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HomeController : RESTFulController
	{
		[HttpGet]
		public ActionResult<string> Get() =>
			Ok("Hello Mario, The Princess is in another castle");
	}
}



