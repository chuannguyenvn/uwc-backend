using MockApplication.Base;

namespace MockApplication.McpFilling;

public class McpFillingMock : BaseHostedService
{
    protected override async Task Main()
    {
        while (true)
        {
            // Do something here...

            await Task.Delay(1000);
        }
    }
}