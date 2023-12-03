namespace Services.OnlineStatus;

public interface IOnlineStatusService
{
    public bool IsAccountOnline(int accountId);
}