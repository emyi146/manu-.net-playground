//https://www.youtube.com/watch?v=WfBsYo20D_I

using DesignPatterns.Common;
using System.Text;

var builder = new FormBodyBuilder();
ConstructionProcess(builder);
builder.Build().Dump();

void ConstructionProcess(IKeyValueCollectionBuilder builder)
{
    builder
        .Add("make", "lada")
        .Add("color", "red")
        .Add("year", 1990.ToString());
}

public interface IKeyValueCollectionBuilder
{
    IKeyValueCollectionBuilder Add(string key, string value);
}

public class FormBodyBuilder : IKeyValueCollectionBuilder
{
    private StringBuilder _stringBuilder = new StringBuilder();

    public IKeyValueCollectionBuilder Add(string key, string value)
    {
        _stringBuilder.Append(key);
        _stringBuilder.Append('=');
        _stringBuilder.Append(value);
        _stringBuilder.AppendLine();
        return this;
    }

    public string Build() => _stringBuilder.ToString();
}

public class HttpHeaderBuilder : IKeyValueCollectionBuilder
{
    private StringBuilder _stringBuilder = new StringBuilder();

    public IKeyValueCollectionBuilder Add(string key, string value)
    {
        _stringBuilder.Append(key);
        _stringBuilder.Append(": ");
        _stringBuilder.Append(value);
        _stringBuilder.AppendLine();
        return this;
    }

    public string Build() => _stringBuilder.ToString();
}

public class DictBuilder : IKeyValueCollectionBuilder
{
    private Dictionary<string, string> _dictionary = new();

    public IKeyValueCollectionBuilder Add(string key, string value)
    {
        _dictionary[key] = value;
        return this;
    }

    public string Build() => _stringBuilder.ToString();
}