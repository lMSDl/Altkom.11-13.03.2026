var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseOwin(pipe => pipe(dic => OwinHelloWorld));

app.Run();



async Task OwinHelloWorld(IDictionary<string, object> dictionary)
{
    string responseText = "Hello, World!";

    var body = dictionary["owin.ResponseBody"] as Stream;
    var headers = dictionary["owin.ResponseHeaders"] as IDictionary<string, string[]>;

    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(responseText);

    headers["Content-Type"] = new[] { "text/plain" };
    headers["Content-Length"] = new[] { bytes.Length.ToString() };

    await body.WriteAsync(bytes, 0, bytes.Length);

}
