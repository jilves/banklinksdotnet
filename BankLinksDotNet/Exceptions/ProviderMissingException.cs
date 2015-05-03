namespace BanklinksDotNet.Exceptions
{
    public class ProviderMissingException : BanklinksDotNetException
    {
        public override string Message
        {
            // TODO: All banklinkDotNet exceptions should provide more information about the error than a simple error message.
            get { return "Unable to find a provider that can handle the specified request or response parameters"; }
        }
    }
}
