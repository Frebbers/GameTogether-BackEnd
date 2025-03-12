﻿using GameTogetherAPI.Test.Drivers;
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
        APIDriver driver = new APIDriver(TestHooks.Context.Client);
        var response = await driver.SendPostRequest($"/api/auth/register", new Credential(
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
}