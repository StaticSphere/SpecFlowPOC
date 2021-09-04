Feature: Persist Todo Items
    This feature describes the various ways that todo items
    can be persisted to the API.

Scenario: API can add a new todo item
    Given that I want to create a new todo item
    When I make the request
    Then the todo item should have been added to the database
    And I should get back a 201 response

Scenario: API can update existing todo item
    Given that I want to edit an existing todo item
    When I make the request
    Then the todo item should have been updated in the database
    And I should get back a 200 response

Scenario: API can delete an existing todo item
    Given that I want to delete an existing todo item
    When I make the request
    Then the todo item should have been removed from the database
    And I should get back a 204 response
