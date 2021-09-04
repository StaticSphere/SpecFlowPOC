using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestSharp;
using TechTalk.SpecFlow;
using ToDoApi.ViewModels;

namespace ToDoApi.Integration.Steps
{
    [Binding]
    public class ToDoItemsSteps
    {
        private static readonly Lazy<IConfiguration> Configuration = new(() => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false)
            .Build());

        private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private RestClient? _client;
        private RestRequest? _request;
        private IRestResponse? _restResponse;
        private string? _jsonResult;

        [BeforeTestRun]
        public async static Task BeforeTestRun()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync(GetEmbeddedResource("ResetForTodoItemTests"));
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            _client = new RestClient(Configuration.Value["BaseUrl"]);

            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync(GetEmbeddedResource("SeedForTodoItemTests"));
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync(GetEmbeddedResource("ResetForTodoItemTests"));
        }

        [Given(@"that I want to request all incomplete todo items")]
        public void GivenThatIWantToRequestAllIncompleteTodoItems()
        {
            //ScenarioContext.Current.Pending();
            _request = new RestRequest("todoitems");
        }

        [Given(@"that I want to request all todo items")]
        public void GivenThatIWantToRequestAllTodoItems()
        {
            _request = new RestRequest("todoitems");
            _request.AddQueryParameter("includeCompleted", true.ToString());
        }

        [Given(@"that I want to request todo items with incomplete = (.*)")]
        public void GivenThatIWantToRequestTodoItemsWithIncompleteFalse(string incompleteString)
        {
            if (!bool.TryParse(incompleteString, out var incomplete))
                throw new SpecFlowException("incomplete value must be 'true' or 'false'");

            if (incomplete)
                GivenThatIWantToRequestAllTodoItems();
            else
                GivenThatIWantToRequestAllIncompleteTodoItems();
        }

        [Given(@"I want those todo items with the tag (.*)")]
        public void GivenIWantThoseTodoItemsWithTheTagChore(string tag)
        {
            _request!.AddQueryParameter("tags", tag);
        }

        [Given(@"that I want to request a todo item")]
        public void GivenThatIWantToRequestATodoItem()
        {
            _request = new RestRequest("todoitems/1");
        }

        [Given(@"that I want to request a non-existent todo item")]
        public void GivenThatIWantToRequestANon_ExistentTodoItem()
        {
            _request = new RestRequest("todoitems/999");
        }

        [Given(@"that I want to create a new todo item")]
        public void GivenThatIWantToCreateANewTodoItem()
        {
            var todoItem = new TodoItemViewModel
            {
                Title = "It's a new item!"
            };
            _request = new RestRequest("todoitems", Method.POST);
            _request.AddJsonBody(todoItem);
        }

        [Given(@"that I want to edit an existing todo item")]
        public void GivenThatIWantToEditAnExistingTodoItem()
        {
            var todoItem = new TodoItemViewModel
            {
                Id = 3,
                Title = "It's an updated item!"
            };
            _request = new RestRequest("todoitems", Method.PUT);
            _request.AddJsonBody(todoItem);
        }

        [Given(@"that I want to delete an existing todo item")]
        public void GivenThatIWantToDeleteAnExistingTodoItem()
        {
            _request = new RestRequest("todoitems/3", Method.DELETE);
        }

        [When(@"I make the request")]
        public async Task GivenThatIRequestAllIncompleteTodoItems()
        {
            _restResponse = await _client!.ExecuteAsync(_request);
            _jsonResult = _restResponse.Content;
        }

        [Then(@"I should receive only the incomplete todo items")]
        public void ThenIShouldReceiveOnlyTheIncompleteTodoItems()
        {
            var todoItems = JsonSerializer.Deserialize<IEnumerable<TodoItemViewModel>>(_jsonResult!, _serializerOptions);
            todoItems!.Count().Should().Be(5);
        }

        [Then(@"I should receive all todo items")]
        public void ThenIShouldReceiveAllTodoItems()
        {
            var todoItems = JsonSerializer.Deserialize<IEnumerable<TodoItemViewModel>>(_jsonResult!, _serializerOptions);
            todoItems!.Count().Should().Be(8);
        }

        [Then(@"I should have (.*) todo items")]
        public void ThenIShouldHaveTodoItems(int itemCount)
        {
            var todoItems = JsonSerializer.Deserialize<IEnumerable<TodoItemViewModel>>(_jsonResult!, _serializerOptions);
            todoItems!.Count().Should().Be(itemCount);
        }

        [Then(@"they should all have the (.*) tag")]
        public void ThenTheyShouldAllHaveTheChoreTag(string tag)
        {
            var allHaveTag = true;
            var todoItems = JsonSerializer.Deserialize<IEnumerable<TodoItemViewModel>>(_jsonResult!, _serializerOptions);
            foreach (var item in todoItems)
            {
                if (!item.Tags.Any(t => t.Title == tag))
                    allHaveTag = false;
            }

            allHaveTag.Should().BeTrue();
        }

        [Then(@"I should get back the expected todo item")]
        public void ThenIShouldGetBackTheExpectedTodoItem()
        {
            var todoItem = JsonSerializer.Deserialize<TodoItemViewModel>(_jsonResult!, _serializerOptions);
            todoItem?.Title.Should().Be("Get milk");
        }

        [Then(@"I should get back a (.*) response")]
        public void ThenIShouldGetBackAResponse(int responseCode)
        {
            _restResponse!.StatusCode.Should().Be((HttpStatusCode)responseCode);
        }

        [Then(@"the todo item should have been added to the database")]
        public async Task ThenTheTodoItemShouldHaveBeenAddedToTheDatabase()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            var count = await conn.QueryFirstAsync<int>("SELECT COUNT(1) FROM todo_item WHERE title = 'It''s a new item!';");

            count.Should().Be(1);
        }

        [Then(@"the todo item should have been updated in the database")]
        public async Task ThenTheTodoItemShouldHaveBeenUpdatedInTheDatabase()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            var count = await conn.QueryFirstAsync<int>("SELECT COUNT(1) FROM todo_item WHERE id = 3 AND title = 'It''s an updated item!';");

            count.Should().Be(1);
        }

        [Then(@"the todo item should have been removed from the database")]
        public async Task ThenTheTodoItemShouldHaveBeenRemovedFromTheDatabase()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            var count = await conn.QueryFirstAsync<int>("SELECT COUNT(1) FROM todo_item WHERE id = 3;");

            count.Should().Be(0);
        }

        private static string GetEmbeddedResource(string fileName)
        {
            using var stream = typeof(ToDoItemsSteps).GetTypeInfo().Assembly.GetManifestResourceStream($"ToDoApi.Integration.Resources.{fileName}.sql");
            using var memStream = new StreamReader(stream!);

            return memStream.ReadToEnd();
        }
    }
}
