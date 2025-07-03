using Gestion_personal.Components;
using Gestion_personal.Services;
using GestionPersonnel.Services;
using GestionPersonnel.Services.EquipeServices;
using GestionPersonnel.Storages.AvancesStorages;
using GestionPersonnel.Storages.DettesStorages;
using GestionPersonnel.Storages.EmployeeEquipeStorages;
using GestionPersonnel.Storages.EquipeStorages;
using GestionPersonnel.Storages.FonctionsStorages;
using GestionPersonnel.Storages.PointagesStorages;
using GestionPersonnel.Storages.SalairesBaseStorages;
using GestionPersonnel.Storages.SalairesStorages;
using GestionPersonnel.Storages.Storages.PostesStorages;
using GestionPersonnel.Storages.TypeDePaimentStorages;
using Implementation.Services.Dashboard;
using Implementation.Services.ReadUSB;
using Implementation.Services.SalaireBase;
using Infrastructures.Storages.DashboardStorages;
using Infrastructures.Storages.EmployeStorages;
using Infrastructures.Storages.ReadUSB;
using Infrastructures.Storages.RecordStorages;
using Infrastructures.Storages.TransferData;
using Infrastructures.Storages.UserStorages;
using Radzen;
using Services;
using Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

string connectionString =builder.Configuration.GetConnectionString("DBConnection");
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
builder.Services.AddRazorComponents();
builder.Services.AddSingleton<IConfiguration>(provider =>
	new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<EmployeStorage>();
builder.Services.AddScoped<FonctionStorage>();
builder.Services.AddScoped<PointageStorage>();
builder.Services.AddScoped<SalaireStorage>();
builder.Services.AddScoped<TypeDePaiementStorage>();
builder.Services.AddScoped<SalaireBaseStorage>();
builder.Services.AddScoped<EquipeStorage>();
builder.Services.AddScoped<EmployeeEquipeStorage> ();
builder.Services.AddScoped<AvanceStorage>();
builder.Services.AddScoped<DetteStorage>();
builder.Services.AddScoped<PosteStorage>();
builder.Services.AddScoped<DashboardStorage>();
builder.Services.AddScoped<DetteRestantStorage>();
builder.Services.AddScoped<IUserStorage,UserStorage>();
builder.Services.AddScoped<ITransferDataStorage, TransferDataStorage>();
builder.Services.AddScoped<ICheckInOutStorage, CheckInOutStorage>();

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITypeDePaiementService, TypeDePaiementService>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IFonctionService, FonctionService>();
builder.Services.AddScoped<IPointageService, PointageService>();
builder.Services.AddScoped<ISalaireService, SalaireService>();
builder.Services.AddScoped<ISalaireBaseService, SalaireBaseService>();
builder.Services.AddScoped<IPDFService, PDFService>();
builder.Services.AddScoped<IEquipeService, EquipeService>();

builder.Services.AddScoped<IEmployeeEquipeService, EmployeeEquipeService>();
builder.Services.AddScoped<IPosteService,PosteService>();
builder.Services.AddScoped<IAvanceService, AvanceService>();
builder.Services.AddScoped<IDetteService, DetteService>();
builder.Services.AddScoped<IPdfService,PdfService>();
builder.Services.AddScoped<IDetteRestantService, DetteRestantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostGeneratePDF,PostGeneratePDF>();
builder.Services.AddSingleton<UserSessionStateService>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddRadzenComponents();
builder.Services.AddAuthentication("Cookies")
	.AddCookie("Cookies", options => {
		options.LoginPath = "/";
		options.AccessDeniedPath = "/";
		options.ExpireTimeSpan=TimeSpan.FromHours(20);
		options.Cookie.Name = "Fabelec";
	});
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
var app = builder.Build();
app.UseSession(); 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
