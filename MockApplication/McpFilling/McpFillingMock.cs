using Commons.Extensions;
using MockApplication.Base;

namespace MockApplication.McpFilling;

public class McpFillingMock : BaseMock
{
    private List<int> _mcpIds;
    
    protected override async Task Main()
    {
         _mcpIds = await GetMcpIds();
        
        while (true)
        {
            var randomMcpId = _mcpIds.GetRandom()[0];
            var fillLevel = await GetMcpFillLevel(randomMcpId);
            if (fillLevel < 0.9f)
            {
                SetMcpFillLevel(randomMcpId, fillLevel + 0.1f);
            }
            
            await Task.Delay(5000);
        }
    }
}