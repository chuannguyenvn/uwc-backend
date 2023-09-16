using Commons.Types;

namespace Commons.Communications.Messages;

public class GetMessagesBetweenTwoUsersRequest
{
    public int SenderAccountID { get; set; }
    public int ReceiverAccountID { get; set; }
}