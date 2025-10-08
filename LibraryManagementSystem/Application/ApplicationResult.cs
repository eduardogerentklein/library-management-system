namespace Application;
public class ApplicationResult<T>
{
    public bool Success { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }

    private ApplicationResult(bool success, T? value, string? error)
    {
        Success = success;
        Value = value;
        Error = error;
    }

    public static ApplicationResult<T> Ok(T value) => new(true, value, null);

    public static ApplicationResult<T> Fail(string error) => new(false, default, error);
}