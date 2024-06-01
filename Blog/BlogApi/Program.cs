using System.Reflection;
using BlogApi.Context;
using BlogApi.Repositories;
using BlogApi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

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

// Add services to the container.
void ConfigureServices(IServiceCollection services)
{
    // Controller support
    services.AddControllers().ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; });
    services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    // Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        // Add header documentation in swagger
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Amblog (Blog API)",
            Description = "A very powerful API to use for creating blog applications.",
        });

        // Feed generated xml api docs to swagger
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    // Configure Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Our services, interface, or DB Contexts that we want to inject
    services.AddTransient<DapperContext>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IPostRepository, PostRepository>();
    services.AddScoped<IPostService, PostService>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<ICommentRepository, CommentRepository>();
    services.AddScoped<ICommentService, CommentService>();
}