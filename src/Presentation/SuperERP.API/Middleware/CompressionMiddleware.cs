using System.IO.Compression;

namespace SuperERP.API.Middleware;

public static class CompressionMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseCompression(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString();
            
            if (acceptEncoding.Contains("gzip"))
            {
                var originalBody = context.Response.Body;
                using var gzipStream = new GZipStream(originalBody, CompressionMode.Compress);
                context.Response.Body = gzipStream;
                context.Response.Headers.Add("Content-Encoding", "gzip");
                
                await next();
                
                context.Response.Body = originalBody;
            }
            else
            {
                await next();
            }
        });
    }
}
