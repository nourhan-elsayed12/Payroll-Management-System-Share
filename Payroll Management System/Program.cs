using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Business.Interfaces;
using PayrollManagementSystem.Business.Services;
using PayrollManagementSystem.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));

// Services depend on the abstraction, resolved to the same scoped DbContext instance
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

// Business services
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IDepartmentIncentiveService, DepartmentIncentiveService>();
builder.Services.AddScoped<IServiceYearsIncentiveService, ServiceYearsIncentiveService>();
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<IAbsenceRuleService, AbsenceRuleService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
