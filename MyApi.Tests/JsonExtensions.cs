namespace MyApi.Tests;

public static class JsonExtensions
{
    public static StringContent ToJsonContent(
         this object data,
         JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var json = JsonSerializer.Serialize(data, options);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}