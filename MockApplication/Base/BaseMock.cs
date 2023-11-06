using Commons.Communications.Mcps;
using Commons.Endpoints;
using Commons.Helpers;
using Commons.Types;

namespace MockApplication.Base;

public abstract class BaseMock : IHostedService
{
    protected abstract Task Main();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Main();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
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

    protected async void EmptyMcp(int mcpId)
    {
        await RequestHelper.Post(Endpoints.McpFillLevel.Empty, new EmptyMcpRequest()
        {
            McpId = mcpId
        });
    }

    protected async Task<Coordinate> GetMcpCoordinate(int mcpId)
    {
        var result = await RequestHelper.Post<GetMcpDataResponse>(Endpoints.McpData.Get, new McpDataQueryParameters
        {
        });

        return result.Results.First(mcpData => mcpData.Id == mcpId).Coordinate;
    }
}