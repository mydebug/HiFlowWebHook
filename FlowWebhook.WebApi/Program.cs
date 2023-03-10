using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FlowWebhook.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            // 发送数据到hiflow webhook
            app.MapPost("/{id}", async ([FromRoute] string id, [FromBody] object o) =>
            {
                var webHookUrl = "https://api.hiflow.tencent.com/engine/webhook/31/" + id;
                HttpClient client = new HttpClient();
                var result = await client.PostAsJsonAsync(webHookUrl, o);
                return Results.Ok();
            });
            app.MapGet("/test", () => { return "nihao"; });
            app.Run();
        }
    }
}