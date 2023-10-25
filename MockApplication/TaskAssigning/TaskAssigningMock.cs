using MockApplication.Base;

namespace MockApplication.TaskAssigning;

public class TaskAssigningMock : BaseMock
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