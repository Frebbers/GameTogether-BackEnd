Feature: UserManagement
This feature file describes the process of logging in and managing one's user account.

    Background:
#Given the API is initialized

    Scenario: Create account
        Given I send a create account request
        Then I assert that the account is created

#    Scenario: Group assignment
#        Given I am logged in
#        When I try to join a group
#        And Send a Join request
#        Then The request is accepted
#        And I join a group
#
#    Scenario: Tag assignment for my account
#        Given I am logged in
#        And I assign a tag {string} to my account
#        Then the tag {string} should be attatched to my account
#
#    Scenario: Tag assignment for my group
#        Given I am logged in 
#        And I assign a tag {string} to my group
#        Then the tag {string} is found in the group
#
#    Scenario: Join request
#        Given I am logged in 
#        When I recieve a request to join a group
#        And The request to join is accepted
#        Then I have 1 more member in my group
#
#    Scenario: Log off
#        Given I am logged in
#        When I click the log off button
#        Then I am no longer logged in 
#    
#    Scenario: Joining a Group
#        Given I am logged in
#        When I request to join a group
#        And The request to join is accepted 
