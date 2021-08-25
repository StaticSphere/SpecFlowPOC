using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestSharp;
using TechTalk.SpecFlow;

namespace ToDoApi.Integration.Steps
{
    [Binding]
    public class ToDoItemsSteps
    {
        private static readonly Lazy<IConfiguration> Configuration = new(() => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false)
            .Build());

        private RestClient _client;
        private RestRequest _request;
        private JsonDocument _result;

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
            _request.AddQueryParameter("tags", tag);
        }

        [When(@"I make the request")]
        public async Task GivenThatIRequestAllIncompleteTodoItems()
        {
            //ScenarioContext.Current.Pending();            
            var result = await _client.ExecuteAsync(_request);

            if (!result.IsSuccessful)
                throw new SpecFlowException("Couldn't get todo items.");

            _result = JsonDocument.Parse(result.Content);
        }

        [Then(@"I should receive only the incomplete todo items")]
        public void ThenIShouldReceiveOnlyTheIncompleteTodoItems()
        {
            _result.RootElement.GetArrayLength().Should().Be(5);
        }

        [Then(@"I should receive all todo items")]
        public void ThenIShouldReceiveAllTodoItems()
        {
            _result.RootElement.GetArrayLength().Should().Be(8);
        }

        [Then(@"I should have (.*) todo items")]
        public void ThenIShouldHaveTodoItems(int itemCount)
        {
            _result.RootElement.GetArrayLength().Should().Be(itemCount);
        }

        [Then(@"they should all have the (.*) tag")]
        public void ThenTheyShouldAllHaveTheChoreTag(string tag)
        {
            var allHaveTag = true;
            foreach(var item in _result.RootElement.EnumerateArray())
            {
                if (!item.GetProperty("tags").EnumerateArray().Any(t => t.GetProperty("title").GetString() == tag))
                    allHaveTag = false;
            }

            allHaveTag.Should().BeTrue();
        }


        private static string GetEmbeddedResource(string fileName)
        {
            using var stream = typeof(ToDoItemsSteps).GetTypeInfo().Assembly.GetManifestResourceStream($"ToDoApi.Integration.Resources.{fileName}.sql");
            using var memStream = new StreamReader(stream);

            return memStream.ReadToEnd();
        }
    }
}
