using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KASCFlightLog.Data;
using KASCFlightLog.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KASCFlightLog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure Cookie Policy
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Initialize Roles and Admin User
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    // Create roles
                    string[] roleNames = { "Admin", "Staff", "User" };
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                            if (!result.Succeeded)
                            {
                                logger.LogError("Failed to create role {Role}. Errors: {Errors}",
                                    roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                            }
                        }
                    }

                    // Create admin user
                    var adminEmail = "admin@kascflight.com";
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            EmailConfirmed = true,
                            Name = "System Administrator",
                            IDNumber = "ADMIN001"
                        };

                        var result = await userManager.CreateAsync(adminUser, "Admin@123456");

                        if (result.Succeeded)
                        {
                            logger.LogInformation("Admin user created successfully");
                            var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

                            if (!roleResult.Succeeded)
                            {
                                logger.LogError("Failed to add admin user to Admin role. Errors: {Errors}",
                                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                            }
                        }
                        else
                        {
                            logger.LogError("Failed to create admin user. Errors: {Errors}",
                                string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding roles and users");
                }
            }

            await app.RunAsync();
        }
    }
}