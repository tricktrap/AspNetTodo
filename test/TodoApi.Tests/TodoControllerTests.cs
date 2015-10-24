using Xunit;
using TodoApi.Controllers;
using TodoApi.Models;
using Microsoft.AspNet.Mvc;

namespace TodoApi.Tests
{	
	public class TodoControllerTests
	{
		[Fact]
		public  void GetAllGetsATodo()
		{
			var controller = new TodoController();
			
			// this would normally have to be mocked or something, but 
			// it's just a dictionary anyway. We can use it directly.
			// Under normal circumstances this is populated via Service injection
			controller.TodoItems = new TodoRepository();
			
			var result = controller.GetAll();
			
			int counter = 0;
			foreach (var item in result) {
				counter++;
				Assert.NotNull(item.Key);         // key is *something*
				Assert.False(item.IsComplete);    // not complete
				Assert.Equal("Item1", item.Name);  // Default item 
			}
			Assert.Equal(1, counter);
		}
		
		[Fact]
		public void TestGet() 
		{
			var controller = new TodoController();
			controller.TodoItems = new TodoRepository();
			
			var result = controller.GetAll();
			TodoItem item = result.GetEnumerator().Current;
			
			Assert.NotNull(item);
			//var result = controller.GetById(item.Key);
			
		}
	}
}