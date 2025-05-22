using SmartSchool.BAL;
using SmartSchool.DAL;
using SmartSchool.Models.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartSchool.Repository;
using SmartSchool.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")), ServiceLifetime.Transient);

builder.Services.AddRazorPages();
builder.Services.AddCors();

double sessionTimeout = Convert.ToDouble(builder.Configuration["sessionTimeOut"]);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Registering Repositories and Services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IschoolServices, SchoolServices>();
builder.Services.AddScoped<ISchoolRepo, SchoolsRepo>();
builder.Services.AddScoped<ISchoolAddressRepo, SchoolAdressRepo>();
builder.Services.AddScoped<ISchoolAddressServices, SchoolAddressServices>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeachersRepo, TeachersRepo>();
builder.Services.AddScoped<ITeachersServices, TeachersServices>();
builder.Services.AddScoped<IClassesRepo, ClassesRepo>();
builder.Services.AddScoped<IClassesService, ClassesService>();
builder.Services.AddScoped<ISubjectRepo, SubjectRepo>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IAdmissionsRepo, AdmissionsRepo>();
builder.Services.AddScoped<IAdmissionsServices, AdmissionsServices>();
builder.Services.AddScoped<IFeeCategoriesRepo, FeeCatagoriesRepo>();
builder.Services.AddScoped<IFeeCategoriesService, FeeCategoriesService>();
builder.Services.AddScoped<IFeeItemsRepo, FeeItemsRepo>();
builder.Services.AddScoped<IFeeItemsService, FeeItemsService>();
builder.Services.AddScoped<ISubscriptionPaymentsRepo, SubscriptionPaymentsRepo>();
builder.Services.AddScoped<ISubscriptionPaymentsService, SubscriptionPaymentsService>();
builder.Services.AddScoped<ISubscriptionsTypeRepo, SubscriptionsTypeRepo>();
builder.Services.AddScoped<ISubscriptionsTypeServices, SubscriptionsTypeServices>();
builder.Services.AddScoped<IModulesRepo, ModulesRepo>();
builder.Services.AddScoped<IModulesService, ModulesService>();
builder.Services.AddScoped<IClassTimetableRepo, ClassTimetableRepo>();
builder.Services.AddScoped<IClassTimetableServices, ClassTimetableServices>();
builder.Services.AddScoped<IFeePaymentsRepo, FeePaymentsRepo>();
builder.Services.AddScoped<IFeePaymentsService, FeePaymentsService>();
builder.Services.AddScoped<IStudentAttendenceRepo, StudentAttendenceRepo>();
builder.Services.AddScoped<IStudentAttendenceServices, StudentAttendenceServices>();
builder.Services.AddScoped<IReportCardRepository, ReportCardRepository>();
builder.Services.AddScoped<IReportCardService, ReportCardService>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authenticate/Login";
        options.AccessDeniedPath = "/Authenticate/Login";
    });

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Super Admin", policy =>
        policy.RequireClaim("UserType", "Super Admin", "School Admin", "Teacher"));
    options.AddPolicy("School Admin", policy =>
        policy.RequireClaim("UserType", "School Admin", "Student", "Teacher"));
    options.AddPolicy("Teacher", policy =>
        policy.RequireClaim("UserType", "Teacher"));
    //options.AddPolicy("HRAccess", policy =>
    //    policy.RequireClaim("UserType", "HR"));
    //options.AddPolicy("EmployeeOrHRAccess", policy =>
    //   policy.RequireRole("EmployeeAccess", "HR"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSession();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authenticate}/{action=Login}/{id?}");

app.Run();
