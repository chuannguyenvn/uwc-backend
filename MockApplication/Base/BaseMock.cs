﻿using Commons.Communications.Map;
using Commons.Communications.Mcps;
using Commons.Communications.UserProfiles;
using Commons.Endpoints;
using Commons.Helpers;
using Commons.Types;

namespace MockApplication.Base;

public abstract class BaseMock : IHostedService
{
    protected abstract Task Main();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(Main, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected async Task<List<int>> GetMcpIds()
    {
        var result = await RequestHelper.Post<GetMcpDataResponse>(Endpoints.McpData.Get, new McpDataQueryParameters
        {
        });

        return result.Results.Select(mcpData => mcpData.Id).ToList();
    }

    protected async Task<float> GetMcpFillLevel(int mcpId)
    {
        var result = await RequestHelper.Post<GetFillLevelResponse>(Endpoints.McpFillLevel.Get, new GetFillLevelRequest()
        {
            McpIds = new List<int>() { mcpId }
        });

        return result.FillLevelsById[mcpId];
    }

    protected async void SetMcpFillLevel(int mcpId, float fillLevel)
    {
        await RequestHelper.Post(Endpoints.McpFillLevel.Set, new SetFillLevelRequest()
        {
            McpId = mcpId,
            FillLevel = fillLevel
        });
    }

    protected async void EmptyMcp(int mcpId, int workerId)
    {
        await RequestHelper.Post(Endpoints.McpFillLevel.Empty, new EmptyMcpRequest()
        {
            McpId = mcpId,
            WorkerId = workerId,
        });
    }

    protected async Task<Coordinate> GetMcpCoordinate(int mcpId)
    {
        var result = await RequestHelper.Post<GetMcpDataResponse>(Endpoints.McpData.Get, new McpDataQueryParameters
        {
        });

        return result.Results.First(mcpData => mcpData.Id == mcpId).Coordinate;
    }

    protected async Task<GetAllDriverProfilesResponse> GetAllDriverProfiles()
    {
        var result = await RequestHelper.Get<GetAllDriverProfilesResponse>(Endpoints.UserProfile.GetAllDriverProfiles);
        return result;
    }

    protected async Task<GetAllCleanerProfilesResponse> GetAllCleanerProfiles()
    {
        var result = await RequestHelper.Get<GetAllCleanerProfilesResponse>(Endpoints.UserProfile.GetAllCleanerProfiles);
        return result;
    }

    protected async Task<GetMcpDataResponse> GetAllMcpData()
    {
        var result = await RequestHelper.Post<GetMcpDataResponse>(Endpoints.McpData.Get, new McpDataQueryParameters
        {
        });
        return result;
    }
    
    protected async Task<Coordinate> GetLocation(int accountId)
    {
        var result = await RequestHelper.Post<GetLocationResponse>(Endpoints.Map.GetLocation, new GetLocationRequest()
        {
            AccountId = accountId
        });
        return result.Coordinate;
    }
    
    protected async void UpdateLocation(int accountId, Coordinate coordinate)
    {
        await RequestHelper.Post(Endpoints.Map.UpdateLocation, new LocationUpdateRequest()
        {
            AccountId = accountId,
            NewLocation = coordinate
        });
    }
    
    protected async Task<GetDirectionResponse> GetDirection(int accountId, Coordinate currentLocation, List<Coordinate> destinations)
    {
        var result = await RequestHelper.Post<GetDirectionResponse>(Endpoints.Map.GetDirection, new GetDirectionRequest()
        {
            AccountId = accountId,
            CurrentLocation = currentLocation,
            Destinations = destinations
        });
        return result;
    }
}