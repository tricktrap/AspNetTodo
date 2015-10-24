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
		
		[HttpGet]
		public IEnumerable<TodoItem> GetAll()
		{
			return TodoItems.GetAll();
		}
		
		[HttpGet("{id}", Name = "GetTodo")]
		public IActionResult GetById(string id)
		{
			var item = TodoItems.Find(id);
			
			if (item == null)
			{
				return HttpNotFound();
			}
			return new ObjectResult(item);
		}
	}
	
}