using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	[Route("api/[controller]")]
	public class TodoController : Controller
	{
		[FromServices]
		public ITodoRepository TodoItems {get; set;}
	}
	
}