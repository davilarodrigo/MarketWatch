namespace ProductCatalogAPI.Common;

public record Result<T>
{
    private Result(bool Success, T? Data, string? Error)
    {
        //private constructor to avoid creating invalid result records
        this.Success = Success;
        this.Data = Data!;
        this.Error = Error;
    }

    public bool Success { get; }
    public T Data { get; }
    public string? Error { get; }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(true, data, null);
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T>(false, default, message);
    }
}

public record Result
{
    private Result(bool Success, string? Error)
    {
        this.Success = Success;
        this.Error = Error;
    }

    public bool Success { get; }
    public string? Error { get; }

    public static Result Ok()
    {
        return new Result(true, null);
    }

    public static Result Fail(string message)
    {
        return new Result(false, message);
    }
}