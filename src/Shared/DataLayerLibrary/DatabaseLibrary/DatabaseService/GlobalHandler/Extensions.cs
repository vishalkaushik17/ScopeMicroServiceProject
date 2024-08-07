namespace DataBaseServices.GlobalHandler;

public static class Extensions
{

    public static string GetExceptionMessages(this Exception e, string msgs = "")
    {
        if (e == null) return string.Empty;
        if (msgs == "") msgs = e.Message;
        if (e.InnerException != null)
            msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
        return msgs;
    }
    public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
    {
        if (ex == null)
        {
            throw new ArgumentNullException("ex");
        }

        var innerException = ex;
        do
        {
            yield return innerException;
            innerException = innerException.InnerException;
        }
        while (innerException != null);
    }
}