namespace RoboUtil.Common.Service
{
    public class DatabaseConnection : System.Attribute
    {
        public bool IsEnabled { get; set; }

        public DatabaseConnection()
        {
            IsEnabled = true;
        }

        public DatabaseConnection(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}