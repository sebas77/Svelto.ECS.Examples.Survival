using System;
using System.Text;

class CoroutineException : Exception
{
    public CoroutineException(string message, Exception innerException):base(message, innerException) {}

    public override string StackTrace
    {
        get
        {
            return "Stack trace" + Environment.NewLine + Environment.NewLine + base.StackTrace;
        }
    }

    public override string ToString()
    {
        return Message + Environment.NewLine + Environment.NewLine + StackTrace;
    }
}
