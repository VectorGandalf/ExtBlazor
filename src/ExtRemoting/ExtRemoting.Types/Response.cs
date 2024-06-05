namespace ExtRemoting.Types;
public class Response<T, TE>(T? data, TE? errors) : IResponse
{
    public Envelope? Envelope { get; } = data != null ? new(data) : null;
    public Envelope? Errors { get; } = errors != null ? new(errors) : null;
}
