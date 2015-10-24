using Xunit;
using TodoApi.Controllers;
using TodoApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Xunit.Abstractions;

namespace TodoApi.Tests
{	
	public class TodoControllerTests
	{
		private readonly ITestOutputHelper output;
		
		public TodoControllerTests(ITestOutputHelper output)
		{
			this.output = output;
		}
		public class LocalTodoRepository : ITodoRepository
		{
			private Dictionary<string, TodoItem> _lookup;
			private int nextID;
			public LocalTodoRepository() 
			{
				 _lookup = new Dictionary<string, TodoItem>();
				 nextID = 1;
			}
			
			public IEnumerable<TodoItem> GetAll() 
			{
				return _lookup.Values;
			}
			
			public TodoItem Find(string key)
			{
				TodoItem item;
				_lookup.TryGetValue(key, out item);
				return item;
			}
			
			public void Add(TodoItem item)
			{
				item.Key = nextID.ToString();
				nextID++;
				_lookup.Add(item.Key, item);
			}
			
			public TodoItem Remove(string key)
			{
				TodoItem item;
				_lookup.TryGetValue(key, out item);
				if (item != null) {
					_lookup.Remove(key);
				}
				return item;
			}
			
			public void Update(TodoItem item)
			{
				_lookup[item.Key] = item;
			}
			
		}
		
		public TodoController GetController(ITodoRepository repo) {
			var controller = new TodoController();
			controller.TodoItems = repo;
			
			return controller;
		}
		
		
		[Fact]
		public  void GetAllFromEmptyReturnsNothing()
		{
			var controller = GetController(new LocalTodoRepository());
			var result = controller.GetAll() as ICollection<TodoItem>;
			
			Assert.Equal(0, result.Count);
		}
		
		[Fact]
		public  void GetAllShouldReturnEverything()
		{
			var repo = new LocalTodoRepository();
			
			repo.Add(new TodoItem() { Name = "Test", IsComplete=false });
			repo.Add(new TodoItem() { Name= "Code", IsComplete=true });
			
			var controller = GetController(repo);
			
			var result = controller.GetAll() as ICollection<TodoItem>;
			
			Assert.Equal(2, result.Count);
			
			bool testFound = false;
			bool codeFound = false;
			
			foreach (var item in result) {
				if (item.Name == "Test") {
					testFound = true;
					Assert.Equal(false, item.IsComplete);
					Assert.Equal("1", item.Key);
				} else if (item.Name == "Code" ) {
					codeFound = true;
					Assert.Equal(true, item.IsComplete);
					Assert.Equal("2", item.Key);
				}
			}
			
			Assert.True(testFound);
			Assert.True(codeFound);
		}
		

		// wake this code up in the future when this page has real content in it
		// https://docs.asp.net/projects/mvc/en/latest/controllers/testing.html
		// https://github.com/aspnet/Docs/issues/123
		#if false
		[Fact]
		public void TestGet() 
		{	
			var repo = new LocalTodoRepository();
			repo.Add(new TodoItem() { Name="FindMe", IsComplete=false});
			var controller = GetController(repo);
			
			// Make sure we DON'T get back the result.
		 	var result = controller.GetById("7") as ActionResult;
			result.ExecuteResult(new ActionContext());
			Assert.NotNull(result); 
			
			var goodResult = controller.GetById("1") as ObjectResult;
			goodResult.ExecuteResult(new ActionContext());
			Assert.Equal(StatusCodes.Status200OK, goodResult.StatusCode);
			TodoItem item = goodResult.Value as TodoItem;
			Assert.Equal("1", item.Key);
			Assert.Equal("FindMe", item.Name);
			Assert.False(item.IsComplete);	
		}
		#endif
	}
}