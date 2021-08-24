using System;
using System.IO;
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
        private JsonDocument _result;

        [BeforeScenario]
        public void BeforeAllScenarios()
        {
            _client = new RestClient(Configuration.Value["BaseUrl"]);
        }

        [BeforeScenario("SeedData")]
        public async Task BeforeSeedData()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync(GetEmbeddedResource("SeedForTodoItemTests"));
        }

        [AfterScenario]
        public async Task AfterSeedData()
        {
            using var conn = new NpgsqlConnection(Configuration.Value.GetConnectionString("DefaultConnection"));
            await conn.ExecuteAsync(GetEmbeddedResource("ResetForTodoItemTests"));
        }

        [Given(@"that I request all incomplete todo items")]
        public async Task GivenThatIRequestAllIncompleteTodoItems()
        {
            //ScenarioContext.Current.Pending();
            var request = new RestRequest("todoitems");
            var result = await _client.ExecuteAsync(request);

            if (!result.IsSuccessful)
                throw new SpecFlowException("Couldn't get todo items.");

            _result = JsonDocument.Parse(result.Content);
        }
        
        [Then(@"I should receive only the incomplete todo items")]
        public void ThenIShouldReceiveOnlyTheIncompleteTodoItems()
        {
            //ScenarioContext.Current.Pending();
            _result.RootElement.GetArrayLength().Should().Be(2);
        }

        private static string GetEmbeddedResource(string fileName)
        {
            using var stream = typeof(ToDoItemsSteps).GetTypeInfo().Assembly.GetManifestResourceStream($"ToDoApi.Integration.Resources.{fileName}.sql");
            using var memStream = new StreamReader(stream);

            return memStream.ReadToEnd();
        }
    }
}
