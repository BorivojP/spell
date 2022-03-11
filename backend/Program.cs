var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseResponseCompression();

    app.UseRouting();
    
    app.UseResponseCaching();
    
    env.WebRootFileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "UI/build"));

    app.UseDefaultFiles();
    
    app.UseStaticFiles(new StaticFileOptions()
    {
        HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
        OnPrepareResponse = (context) =>
        {
            var headers = context.Context.Response.GetTypedHeaders();
            headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(1)
            };
        }
    });

    app.UseSpaStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "UI/build")),
        RequestPath = "/Static",
        HttpsCompression = Microsoft.AspNetCore.Http.Features.HttpsCompressionMode.Compress,
        OnPrepareResponse = (context) =>
        {
            var headers = context.Context.Response.GetTypedHeaders();
            headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(1)
            };
        }
    });
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "UI";

        if (env.IsDevelopment())
        {
            spa.UseReactDevelopmentServer(npmScript: "start");
        }
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
