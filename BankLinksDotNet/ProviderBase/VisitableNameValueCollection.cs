using System.Collections.Specialized;

namespace BanklinksDotNet.ProviderBase
{
    public class VisitableNameValueCollection : NameValueCollection, IVisitable
    {
        public VisitableNameValueCollection(NameValueCollection nameValueCollection) : base(nameValueCollection)
        {
           
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
