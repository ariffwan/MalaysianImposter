using JanganKantoi.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JanganKantoi
{
	public class Startup
	{
		// ✅ Keep ONLY this constructor (fix crash)
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			// ✅ Cookie configuration
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.CheckConsentNeeded = context => false;
			});

			// ✅ Sessions
			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.Cookie.IsEssential = true;
				options.IdleTimeout = TimeSpan.FromMinutes(180);
				options.Cookie.HttpOnly = true;
			});

			services.AddHttpContextAccessor();

			// ✅ FIXED DB CONNECTION
			services.AddDbContext<MyDbContext>(options =>
			{
				var connection = Configuration.GetConnectionString("mydb");

				Console.WriteLine("=== DB CONNECTION ===");
				Console.WriteLine(connection ?? "NULL");
				Console.WriteLine("=====================");

				if (string.IsNullOrEmpty(connection))
				{
					throw new Exception("Connection string 'mydb' is NULL!");
				}

				options.UseNpgsql(connection);
			});

			// ✅ MVC
			services.AddControllersWithViews();
			services.AddRazorPages();

			// ✅ Localization
			services.AddLocalization(option => option.ResourcesPath = "Resources");

			// ✅ CORS
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			// ✅ Large form handling
			services.Configure<FormOptions>(options =>
			{
				options.ValueCountLimit = int.MaxValue;
				options.ValueLengthLimit = int.MaxValue;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseStatusCodePagesWithReExecute("/errorhandler/{0}");
			}

			// ✅ Localization
			var supportedCultures = new[]
			{
				new CultureInfo("ms"),
				new CultureInfo("en"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			// ⚠️ Optional: disable if warning bothers you
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors();

			app.UseSession();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}