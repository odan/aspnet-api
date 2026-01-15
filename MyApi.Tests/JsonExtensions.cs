namespace MyApi.Tests;

public static class JsonExtensions
{
    public static StringContent ToJsonContent(
         this object data,
         JsonSerializerOptions? options = null)
    {
        return new StringContent(ToJson(data, options), Encoding.UTF8, "application/json");
    }

    public static string ToJson(
       this object data,
       JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions(JsonSerializerDefaults.Web);

        return JsonSerializer.Serialize(data, options);
    }
}