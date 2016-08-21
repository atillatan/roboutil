namespace RoboUtil.Common.Service
{
    public class Transactional:System.Attribute
    {
        public bool IsEnabled { get; set; }
        public Transactional()
        {
            IsEnabled = true;
        }

        public Transactional(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
