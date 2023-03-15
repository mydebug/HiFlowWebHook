using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FlowWebhook.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache();
            var app = builder.Build();
            var cache = app.Services.GetService<IMemoryCache>();
            // �������ݵ�webhook 
            app.MapPost("/{id}", async ([FromRoute] string id, [FromBody] object o) =>
            {
                await SendDataAsync(id, o);
                return Results.Ok();
            });

            // ��ӻ�������
            app.MapPost("cache/{name}", ([FromRoute] string name, [FromBody] object o) =>
            {
                cache.Set(name, o);
                return Results.Ok();
            });

            // ��ȡ��������
            app.MapGet("cache/{name}", ([FromRoute] string name) =>
            {
                return cache.Get(name);
            });
            // ɾ����������
            app.MapDelete("cache/{name}", ([FromRoute] string name) =>
            {
                cache.Remove(name);
                return Results.Ok();
            });

            app.MapGet("/", () => { return "hello,chendaye"; });
            app.Run();

        }
        private static async Task SendDataAsync(string id, object o)
        {
            var webHookUrl = "https://api.hiflow.tencent.com/engine/webhook/31/" + id;
            HttpClient client = new HttpClient();
            var result = await client.PostAsJsonAsync(webHookUrl, o);
        }
    }
}