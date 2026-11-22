using System.Text;

namespace Demo.Infrastructure.Logging;

public sealed class HttpLoggingMiddleware
{
    private const int MaximumBodyBytes = 64 * 1024;

    private readonly RequestDelegate _next;
    private readonly ILogger<HttpLoggingMiddleware> _logger;

    public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var requestBody = await ReadRequestBody(context.Request, context.RequestAborted);
        var requestHeaders = FormatHeaders(context.Request.Headers);
        var originalResponseBody = context.Response.Body;

        await using var responseBuffer = new MemoryStream();
        context.Response.Body = responseBuffer;

        try
        {
            await _next(context);
            await responseBuffer.FlushAsync(context.RequestAborted);

            var responseBody = await ReadResponseBody(context.Response, responseBuffer, context.RequestAborted);
            var responseHeaders = FormatHeaders(context.Response.Headers);

            _logger.LogInformation(
                "HTTP request: {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString}; " +
                "Protocol={Protocol}; RemoteIp={RemoteIp}; RequestContentType={RequestContentType}; " +
                "RequestContentLength={RequestContentLength}; RequestHeaders={RequestHeaders}; RequestBody={RequestBody}; " +
                "ResponseStatusCode={StatusCode}; ResponseContentType={ResponseContentType}; " +
                "ResponseContentLength={ResponseContentLength}; ResponseHeaders={ResponseHeaders}; ResponseBody={ResponseBody}",
                context.Request.Method,
                context.Request.Scheme,
                context.Request.Host,
                context.Request.PathBase,
                context.Request.Path,
                context.Request.QueryString,
                context.Request.Protocol,
                context.Connection.RemoteIpAddress,
                context.Request.ContentType,
                context.Request.ContentLength,
                requestHeaders,
                requestBody,
                context.Response.StatusCode,
                context.Response.ContentType,
                context.Response.ContentLength,
                responseHeaders,
                responseBody);

            await responseBuffer.CopyToAsync(originalResponseBody, context.RequestAborted);
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }

    private static async Task<string> ReadRequestBody(HttpRequest request, CancellationToken ct)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        try
        {
            var bytes = await ReadLimited(request.Body, ct);
            return FormatBody(bytes, request.ContentType);
        }
        finally
        {
            request.Body.Position = 0;
        }
    }

    private static async Task<string> ReadResponseBody(
        HttpResponse response,
        MemoryStream responseBuffer,
        CancellationToken ct)
    {
        responseBuffer.Position = 0;
        var bytes = await ReadLimited(responseBuffer, ct);
        responseBuffer.Position = 0;
        return FormatBody(bytes, response.ContentType);
    }

    private static async Task<byte[]> ReadLimited(Stream stream, CancellationToken ct)
    {
        var buffer = new byte[MaximumBodyBytes + 1];
        var totalRead = 0;

        while (totalRead < buffer.Length)
        {
            var read = await stream.ReadAsync(
                buffer.AsMemory(totalRead, buffer.Length - totalRead),
                ct);
            if (read == 0)
            {
                break;
            }

            totalRead += read;
        }

        if (totalRead > MaximumBodyBytes)
        {
            Array.Resize(ref buffer, MaximumBodyBytes);
        }
        else
        {
            Array.Resize(ref buffer, totalRead);
        }

        return buffer;
    }

    private static string FormatBody(byte[] bytes, string? contentType)
    {
        if (bytes.Length == 0)
        {
            return "<empty>";
        }

        if (IsTextContent(contentType))
        {
            return DecodeTextBody(bytes, contentType);
        }

        var body = Convert.ToBase64String(bytes);
        return bytes.Length == MaximumBodyBytes ? body + "… <truncated at 64 KiB>" : $"base64:{body}";
    }

    private static string DecodeTextBody(byte[] bytes, string? contentType)
    {
        var charset = Microsoft.Net.Http.Headers.MediaTypeHeaderValue.TryParse(contentType, out var parsed)
            ? parsed.Charset.Value
            : null;
        Encoding encoding;

        try
        {
            encoding = string.IsNullOrWhiteSpace(charset) ? Encoding.UTF8 : Encoding.GetEncoding(charset);
        }
        catch (ArgumentException)
        {
            encoding = Encoding.UTF8;
        }

        var body = encoding.GetString(bytes);
        return bytes.Length == MaximumBodyBytes ? body + "… <truncated at 64 KiB>" : body;
    }

    private static bool IsTextContent(string? contentType)
    {
        return contentType is not null &&
            (contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("json", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("xml", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("form-urlencoded", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("javascript", StringComparison.OrdinalIgnoreCase));
    }

    private static string FormatHeaders(IHeaderDictionary headers)
    {
        return string.Join("; ", headers.Select(header =>
        {
            var value = IsSensitiveHeader(header.Key) ? "[REDACTED]" : header.Value.ToString();
            return $"{header.Key}={value}";
        }));
    }

    private static bool IsSensitiveHeader(string name)
    {
        return name.Equals("Authorization", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("Proxy-Authorization", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("Cookie", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("X-API-Key", StringComparison.OrdinalIgnoreCase);
    }
}