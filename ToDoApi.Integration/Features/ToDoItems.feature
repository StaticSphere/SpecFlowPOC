Feature: ToDo Items
	This feature is the main feature of this API. It will need to be able
	to properly retrieve, create, update, and delete todo items.

@SeedData
Scenario: API can retrieve all incomplete todo items
	Given that I request all incomplete todo items
	Then I should receive only the incomplete todo items
