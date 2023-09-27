namespace PartsWebApi.Utils.Exceptions
{
    public class ActionRequirementsNotFulfilledException : Exception
    {
        public ActionRequirementsNotFulfilledException(string message) : base(message)
        {
        }
    }
}
