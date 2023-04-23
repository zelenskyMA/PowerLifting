namespace LoggerLib.Middleware;

/// <summary>
/// Метка для апи, которое не нужно логировать
/// </summary>
public class ExcludeLogItemAttribute : Attribute
{
    public ExcludeLogItemAttribute()
    {
    }
}
