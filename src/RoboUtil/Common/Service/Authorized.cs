namespace RoboUtil.Common.Service

{

    public class Authorized : System.Attribute
    {
        private string[] authorizations;

        public Authorized(params string[] authorizations)
        {
            this.authorizations = authorizations;
        }

        public string[] Authorizations
        {
            get
            {
                return this.authorizations;
            }
        }

    }


}
