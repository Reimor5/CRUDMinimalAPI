using CRUDMinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<Superhero>> GetAllHeroes(DataContext context) => 
    await context.Superheroes.ToListAsync();

app.MapGet("/", () => "Welcome to my CRUD with minimal API using superheroes DB");

app.MapGet("/superhero", async (DataContext context) => 
    await context.Superheroes.ToListAsync());

app.MapGet("/superhero/{id}", async (DataContext context, int id) =>
    await context.Superheroes.FindAsync(id) is Superhero hero ?
        Results.Ok(hero) :
        Results.NotFound("Sorry, this hero doesn't exist!"));

app.MapPost("/superhero", async (DataContext context, Superhero hero) =>
{
    context.Superheroes.Add(hero);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllHeroes(context));
});

app.MapPut("/superhero/{id}", async(DataContext context, Superhero hero, int id) =>
{
    var dbHero = await context.Superheroes.FindAsync(id);
    if (dbHero == null) return Results.NotFound("Hero cannot be traced!");

    dbHero.Firstname = hero.Firstname;
    dbHero.Lastname = hero.Lastname;
    dbHero.Heroname = hero.Heroname;
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllHeroes(context));
});

app.MapDelete("/superhero/{id}", async (DataContext context, int id) =>
{
    var dbHero = await context.Superheroes.FindAsync(id);
    if (dbHero == null) return Results.NotFound("Sorry, WHO???");

    context.Superheroes.Remove(dbHero);
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllHeroes(context));
});

app.Run();
