using Commons.Extensions;
using MockApplication.Base;

namespace MockApplication.McpFilling;

public class McpFillingMock : BaseMock
{
    protected override async Task Main()
    {
        while (true)
        {
            var mcpIds = await GetMcpIds();
            var randomMcpIds = mcpIds.GetRandom(5);

            foreach (var randomMcpId in randomMcpIds)
            {
                var fillLevel = await GetMcpFillLevel(randomMcpId);
                SetMcpFillLevel(randomMcpId, fillLevel + Random.Shared.NextSingle() * 0.5f);
            }

            await Task.Delay(5000);
        }
    }
}