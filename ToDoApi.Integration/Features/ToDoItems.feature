Feature: Query Todo Items
	This feature is describes the various ways that todo items
    can be requested from the API.

Scenario: API can retrieve all incomplete todo items
	Given that I want to request all incomplete todo items
    When I make the request
	Then I should receive only the incomplete todo items

Scenario: API can retrieve all todo items
    Given that I want to request all todo items
    When I make the request
    Then I should receive all todo items

Scenario Outline: API can retrieve todo items based on tags
    Given that I want to request todo items with incomplete = <incomplete>
    And I want those todo items with the tag <tag>
    When I make the request
    Then I should have <count> todo items
    And they should all have the <tag> tag

    Examples:
    | incomplete | tag   | count |
    | false      | Chore | 3     |
    | true       | Chore | 4     |
    | false      | Auto  | 1     |
    | true       | Auto  | 2     |
    | false      | Work  | 2     |
    | true       | Work  | 3     |
