namespace Services.Status;

public class MockOnlineStatusService : IOnlineStatusService
{
    private List<int> _onlineAccountIds = new();

    public bool IsAccountOnline(int accountId)
    {
        return _onlineAccountIds.Contains(accountId);
    }

    public void SetAsOnline(int accountId)
    {
        if (_onlineAccountIds.Contains(accountId)) return;
        _onlineAccountIds.Add(accountId);
    }
}