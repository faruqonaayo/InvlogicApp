using InvlogicServer;
using InvlogicServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// registering the database context
builder.Services.AddDbContext<ILSAppContext>();

// Load environment variables from .env file
DotNetEnv.Env.Load();

var app = builder.Build();

// General error handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
    }
});

// route to create a new project
app.MapPost("/newproject", async (ILSAppContext db, HttpContext httpContext) =>
{
    try
    {
        // getting the project data from the request body
        var project = await httpContext.Request.ReadFromJsonAsync<Project>();

        // checking if the project name is already taken
        var projectExists = await db.Projects.FirstOrDefaultAsync(p => p.ProjectName == project.ProjectName);
        if (projectExists != null)
        {
            return Results.BadRequest(new { Message = "Project name already exists", statusCode = 400 });
        }

        // adding the project to the database
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return Results.Created($"/project/{project.ProjectId}", new { Message = "Project created successfully", project, statusCode = 201 });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new { Message = "Invalid JSON data", statusCode = 400 });
    }

});

// route to get all projects
app.MapGet("/projects", async (ILSAppContext db) =>
{
    var projects = await db.Projects.ToListAsync();
    return Results.Ok(new { projects, statusCode = 200 });
});

// route to get a single project
app.MapGet("/project/{id:int}", async (ILSAppContext db, int id) =>
{
    try
    {
        var project = await db.Projects.FindAsync(id);
        if (project == null)
        {
            return Results.NotFound(new { Message = "Project not found", statusCode = 404 });
        }
        return Results.Ok(new { project, statusCode = 200 });
    }
    catch (Exception)
    {
        return Results.BadRequest(new { Message = "Invalid project ID", statusCode = 400 });
    }
});

// route to delete a project
app.MapDelete("/project/{id:int}", async (ILSAppContext db, int id) =>
{
    try
    {
        var project = await db.Projects.FindAsync(id);
        if (project == null)
        {
            return Results.NotFound(new { Message = "Project not found", statusCode = 404 });
        }
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return Results.Ok(new { Message = "Project deleted successfully", statusCode = 200 });
    }
    catch (Exception)
    {
        return Results.BadRequest(new { Message = "Invalid project ID", statusCode = 400 });
    }
});

app.Run();
