# SpecFlow+LivingDoc Proof of Concept

This proof of concept demonstrates the usage of both the SpecFlow testing library, as
well as the LivingDoc addition that can be used to generate reports from the test results
that can then be read by non-technical folks.

## API Synopsis

The example API is a simple .NET 5 Web API based on a Todo application. It allows you to
created, edit, and delete todo items, and could be used with a fleshed out UI. A UI is not
included with this proof of concept since the point is to demonstrate using SpecFlow, not to
create a fully fleshed out application.

## Setup

You'll want to be using Visual Studio 2019 or greater to run this application. While SpecFlow
is technically supported in Visual Studio Code, the support for the Gherkin languate that
tests are written in not really "there" yet.

In Visual Studio, you'll want to add in the SpecFlow for Visual Studio 2019 extension, which you
can install through the Visual Studio extension manager. The extension works in all versions of
Visual Studio 2019, including Community edition. The caveat is that you'll need to create a free
account with SpecFlow.org, which you'll need to do the first time that you try to run the tests.
They'll initially fail to run, but you'll be asked to create the account. After the account is
created, you'll be able to run the tests.

You'll also want to install the Entity Framework EF tooling if you haven't already, along with
the LivingDoc tool, which will be used to generate the status report of test runs. You can do
so from a PowerShell shell with the following command:

```
dotnet tool install --global dotnet-ef
dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI
```

You'll also want to create a Postgres database to work with. Since this is only a POC, the connection
string that was used to develop and test the proof of concept is included. Feel free to change it
if you want. Otherwise, you'll want to create a database called `todo`, and create a user called
`todo_user` for the application to run with.

Once the database is created, you can use the EF tooling to populate the database with the tables
needed for the application to run like so (from the same folder as the ToDoApi project):

```
dotnet ef database update
```

## Project Architecture

### ToDoApi

The first project is `ToDoApi`, which is naturally the API itself. You can run it directly if you
want. However, to run the tests, you'll want to run the API WITHOUT debugging. You can do this by
either pressing CTRL+F5, or by clicking the `Debug > Start Without Debugging` menu. To terminate
the API, you can terminate the IIS Express process from the IIS Express system tray icon.

### ToDoApi.Integration

The second project is `ToDoApi.Integration` and has the SpecFlow integration tests. Once the API is
running, from the Visual Studio Text Explorer, click on the `Run All Tests In View` button. Note
that running the tests will automatically reset the database back to an empty state, deleting any
items that you have created.

#### Features

This folder is where you'll find the feature file that actually define the tests. These files contain
the Gherkin syntax that is meant to be readable by technical and non-technical people alike.

#### Resources

This folder contains embedded resource SQL scripts that are used for seeding the database before and
after tests.

#### Steps

The lone file in this folder defines the C# code that descries what each "sentence" of the Gherkin
feature code is supposed to do.

#### ViewModels

The files here are not actually IN the ToDoApi.Integration project, but are linked in from the ToDoApi
project itself. This is a good way of sharing files in a solution that should be compiled separately,
but can be maintaind in a single place. This allows our code to properly deserialize the JSON messages
being sent to and from the API.

## LivingDoc

To generate LivingDoc documentation from our test runs, you'll need to use the previously installed
LivingDoc CLI. When SpecFlow runs a test, it creates a file called TestExecution.json in the directory
with the binaries. As such, you can generate the LivingDoc documentation with the following command
(from the root solution folder):

```
livingdoc test-assembly .\ToDoApi.Integration\bin\Debug\net5.0\ToDoApi.Integration.dll -t .\ToDoApi.Integration\bin\Debug\net5.0\TestExecution.json
```
