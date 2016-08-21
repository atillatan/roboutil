namespace RoboUtil.Common.Service

{
   
    public class Authorized:System.Attribute
    {
        public bool IsEnabled { get; set; }
        public Authorized()
        {
            IsEnabled = true;
        }
        public Authorized(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }

  
}
