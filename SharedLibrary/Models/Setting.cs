using Commons.Types.SettingOptions;

namespace Commons.Models
{
    public class Setting : IndexedEntity
    {
        // Interface settings
        public ToggleOption DarkMode { get; set; }
        public ToggleOption ColorblindMode { get; set; }
        public ToggleOption ReducedMotionMode { get; set; }
        public LanguageOption Language { get; set; }

        // Notification settings
        public ToggleOption Messages { get; set; }
        public ToggleOption EmployeesLoggedIn { get; set; }
        public ToggleOption EmployeesLoggedOut { get; set; }
        public ToggleOption McpsAlmostFull { get; set; }
        public ToggleOption McpsFull { get; set; }
        public ToggleOption McpsEmptied { get; set; }
        public ToggleOption SoftwareUpdateAvailable { get; set; }

        // Account settings
        public OnlineStatusOption OnlineStatus { get; set; }
    }
}