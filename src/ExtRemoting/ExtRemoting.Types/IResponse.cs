namespace ExtRemoting.Types;

public interface IResponse
{
    Envelope? Envelope { get; }
    Envelope? Errors { get; }
}