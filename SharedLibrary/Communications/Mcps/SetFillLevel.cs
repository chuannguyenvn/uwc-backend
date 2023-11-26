namespace Commons.Communications.Mcps
{
    public class SetFillLevelRequest
    {
        public int McpId { get; set; }
        // Fill level is a float between 0 and 1.
        public float FillLevel { get; set; }
    }
}