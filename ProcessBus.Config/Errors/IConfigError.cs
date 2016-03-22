namespace ProcessBus.Config.Errors
{
    public interface IConfigError
    {
        string Name { get; }
        string Message { get; }
    }
}
