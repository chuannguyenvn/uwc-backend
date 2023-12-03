namespace Services.Status;

public interface IOnlineStatusService
{
    public bool IsAccountOnline(int accountId);
}