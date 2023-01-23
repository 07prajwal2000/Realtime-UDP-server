namespace RealtimeApp.Server.Initializers;

internal class MethodExistsException : Exception
{
    private readonly string methodName;

    public MethodExistsException(string methodName) : base($"Method with {methodName} already exists")
    {
        this.methodName = methodName;
    }

    public override string ToString()
    {
        return $"Method with {methodName} already exists";
    }
}
