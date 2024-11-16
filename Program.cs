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
            // Add this after app builder configuration
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    // Ensure database is created
                    context.Database.EnsureCreated();

                    // Seed Roles
                    string[] roleNames = { "Admin", "Staff", "User" };
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                        }
                    }

                    // Seed Admin User
                    var adminUser = await userManager.FindByEmailAsync("admin@kasc.com");
                    if (adminUser == null)
                    {
                        var admin = new ApplicationUser
                        {
                            UserName = "admin@kasc.com",
                            Email = "admin@kasc.com",
                            FirstName = "Admin",
                            LastName = "User",
                            IdNumber = "ADMIN001",
                            EmailConfirmed = true,
                            IsActive = true
                        };

                        var result = await userManager.CreateAsync(admin, "Admin@123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(admin, "Admin");
                        }
                    }

                    // Seed Staff User
                    var staffUser = await userManager.FindByEmailAsync("staff@kasc.com");
                    if (staffUser == null)
                    {
                        var staff = new ApplicationUser
                        {
                            UserName = "staff@kasc.com",
                            Email = "staff@kasc.com",
                            FirstName = "Staff",
                            LastName = "User",
                            IdNumber = "STAFF001",
                            EmailConfirmed = true,
                            IsActive = true
                        };

                        var result = await userManager.CreateAsync(staff, "Staff@123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(staff, "Staff");
                        }
                    }

                    // Seed Regular User
                    var regularUser = await userManager.FindByEmailAsync("user@kasc.com");
                    if (regularUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = "user@kasc.com",
                            Email = "user@kasc.com",
                            FirstName = "Regular",
                            LastName = "User",
                            IdNumber = "USER001",
                            EmailConfirmed = true,
                            IsActive = true
                        };

                        var result = await userManager.CreateAsync(user, "User@123");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Continue with the rest of your Program.cs configuration

            await app.RunAsync();
        }
    }
}