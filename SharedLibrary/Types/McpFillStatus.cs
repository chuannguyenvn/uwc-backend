namespace Commons.Types
{
    public enum McpFillStatus
    {
        Full,
        AlmostFull,
        NotFull,
    }

    public static class McpFillStatusHelper
    {
        public static McpFillStatus GetStatus(float fillLevel)
        {
            if (fillLevel >= 0.9f) return McpFillStatus.Full;
            if (fillLevel >= 0.5f) return McpFillStatus.AlmostFull;
            return McpFillStatus.NotFull;
        }
    }
}