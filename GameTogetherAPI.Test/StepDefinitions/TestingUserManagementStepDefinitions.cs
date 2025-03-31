﻿using GameTogetherAPI.Models;
using GameTogetherAPI.Test.Drivers;
using GameTogetherAPI.Test.Fixtures;
using GameTogetherAPI.Test.Hooks;
using GameTogetherAPI.Test.Util;

namespace GameTogetherAPI.Test.StepDefinitions;
using FluentAssertions;

[Binding]
public class TestingUserManagementStepDefinitions(ScenarioContext scenarioContext)
{

    [Given(@"I send a create account request")]
    public async Task GivenISendACreateAccountRequest()
    {
        var driver = new APIDriver(TestHooks.Context.Client);
        var response = await driver.SendPostRequest($"/api/auth/register", new RegisterModel(
            APIConstants.TestEmail, APIConstants.TestPassword)
        );
        scenarioContext.Add("response", response);
    }
    
    [Then(@"I assert that the account is created")]
    public void ThenIAssertThatTheAccountIsCreated()
    {
        var response = scenarioContext.Get<HttpResponseMessage>("response");
        var responseCode = response.StatusCode.ToString();
        responseCode.Should().BeEquivalentTo("OK");
    }
    [Given("I send a log in request")]
    public async Task GivenISendALogInRequest()
    {
        var driver = new APIDriver(TestHooks.Context.Client);
        LoginModel login = new LoginModel(APIConstants.TestEmail, APIConstants.TestPassword);
        var response = await driver.SendPostRequest($"/api/auth/login", login);
        scenarioContext.Add("response", response);
        Console.WriteLine(response);
    }
    
    [Then("I assert that the account is logged in")]
    public void ThenIAssertThatTheAccountIsLoggedIn()
    {
        var response = scenarioContext.Get<HttpResponseMessage>("response");
        var responseCode = response.StatusCode.ToString();
        responseCode.Should().BeEquivalentTo("OK");
            
    }

    [Given(@"I am logged in")]
    public async Task  GivenIAmLoggedIn()
    {
        var driver = new APIDriver(TestHooks.Context.Client);
        var response = await driver.SendGetRequest("/api/auth/login");
        var responseToken = response.ToString();
        var responseCode = response.StatusCode.ToString();
            
        responseCode.Should().BeEquivalentTo("OK");
    }


        [Given(@"I click the log off button")]
        public void GivenIClickTheLogOffButton()
        {
            ScenarioContext.StepIsPending();
        }
        
        [Given(@"TestUser is reset")]
        public async Task GivenTestUserIsReset()
        {
            var driver = new APIDriver(TestHooks.Context.Client); 
            var loginResponse = await driver.SendPostRequest("/api/auth/login", new { APIConstants.TestEmail, APIConstants.TestPassword });
            if (loginResponse.StatusCode.ToString() == ("OK")) { //if logged in, delete the user. Else do nothing
            loginResponse.Headers.TryGetValues("token", out var responseToken);
            //scenarioContext["token"] = responseToken;
            loginResponse.Should().NotBeNull();
            var deleteResponse = await driver.SendDeleteRequest("/api/auth/remove-user", responseToken);
            deleteResponse.StatusCode.ToString().Should().BeEquivalentTo("OK");
        }
            else
            {
                Console.WriteLine("User failed to log in");
            }
        //var response = scenarioContext.Get<HttpResponseMessage>("response");
    }
        
}