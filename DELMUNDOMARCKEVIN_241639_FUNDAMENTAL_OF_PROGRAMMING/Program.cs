using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Insert(0, "/Views/{1}/DELMUNDOMARCKEVIN_241639_{0}.cshtml");
    options.ViewLocationFormats.Insert(1, "/Views/Shared/DELMUNDOMARCKEVIN_241639_{0}.cshtml");
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Singleton in-memory CRUD store
builder.Services.AddSingleton(new TypedCrudStore<DELMUNDOMARCKEVIN_241639_WishlistItem>(CrudSeedData.WishlistItems));
builder.Services.AddSingleton(new TypedCrudStore<DELMUNDOMARCKEVIN_241639_Game>(CrudSeedData.Games));
builder.Services.AddSingleton(new TypedCrudStore<DELMUNDOMARCKEVIN_241639_AttendanceRecord>(CrudSeedData.AttendanceRecords));
builder.Services.AddSingleton(new TypedCrudStore<DELMUNDOMARCKEVIN_241639_LostFoundItem>(CrudSeedData.LostFoundItems));
builder.Services.AddSingleton<LibraryStore>();
builder.Services.AddSingleton<ProgramCatalog>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/DELMUNDOMARCKEVIN_241639_Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DELMUNDOMARCKEVIN_241639_Home}/{action=Index}/{id?}");

app.Run();
