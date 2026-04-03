using JanganKantoi.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JanganKantoi
{
	public class Startup
	{
		// Uncomment if intend to test locally
		//public Startup()
		//{
		//	// Load configuration from appsettings.json
		//	var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
		//	Configuration = builder.Build();

		//}


		// Uncomment if intend to deploy
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
		public IConfiguration Configuration { get; set; }

		// -------------------------------------------------------------
		// ConfigureServices: Called by the runtime to add services
		// -------------------------------------------------------------
		public void ConfigureServices(IServiceCollection services)
		{
			// ✅ Cookie configuration
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.None;
				options.CheckConsentNeeded = context => false;
			});

			// ✅ Enable sessions
			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.Cookie.IsEssential = true;
				options.IdleTimeout = TimeSpan.FromMinutes(180);
				options.Cookie.HttpOnly = true;
			});

			// ✅ Add this line: HttpContext Accessor
			services.AddHttpContextAccessor();

			// ✅ Database context setup (PostgreSQL)
			//services.AddDbContext<MyDbContext>(options =>
			//{
			//	options.UseNpgsql(Configuration.GetConnectionString("mydb"), builder =>
			//	{
			//		builder.EnableRetryOnFailure(3);
			//	});
			//});
			//services.AddDbContext<MyDbContext>(options =>
			//{
			//	var connection = Environment.GetEnvironmentVariable("DATABASE_URL")
			//					 ?? Configuration.GetConnectionString("mydb");

			//	options.UseNpgsql(connection);
			//});

			//services.AddDbContext<MyDbContext>(options =>
			//{
			//	//var connection = Environment.GetEnvironmentVariable("DATABASE_URL");

			//	//if (!string.IsNullOrEmpty(connection))
			//	//{
			//	//	var uri = new Uri(connection);
			//	//	var userInfo = uri.UserInfo.Split(':');

			//	//	var builder = new Npgsql.NpgsqlConnectionStringBuilder
			//	//	{
			//	//		Host = uri.Host,
			//	//		Port = uri.Port,
			//	//		Username = userInfo[0],
			//	//		Password = userInfo[1],
			//	//		Database = uri.AbsolutePath.Trim('/'),
			//	//		SslMode = Npgsql.SslMode.Require,
			//	//		TrustServerCertificate = true
			//	//	};

			//	//	options.UseNpgsql(builder.ConnectionString);
			//	//}
			//	//else
			//	//{
			//		options.UseNpgsql(Configuration.GetConnectionString("mydb"));
			//	//}
			//});

			services.AddDbContext<MyDbContext>(options =>
			{
				var envVar = Environment.GetEnvironmentVariable("ConnectionStrings__mydb");

				Console.WriteLine("=== ENV VAR ===");
				Console.WriteLine(envVar);

				var connection = envVar;

				Console.WriteLine("=== DB CONNECTION ===");
				Console.WriteLine(connection);
				Console.WriteLine("=====================");

				options.UseNpgsql(connection);
			});

			// ✅ Controllers + Views
			services.AddControllersWithViews();
			services.AddRazorPages();

			// ✅ Localization setup
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

			// ✅ Handle large form data
			services.Configure<FormOptions>(options =>
			{
				options.ValueCountLimit = int.MaxValue;
				options.ValueLengthLimit = int.MaxValue;
			});
		}

		// -------------------------------------------------------------
		// Configure: Defines HTTP request pipeline
		// -------------------------------------------------------------
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

			// Optional: Base path
			//app.UsePathBase("/CIPM2");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors();

			// ✅ Must be before Authorization
			app.UseSession();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers(); // ⭐ REQUIRED for API controllers

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			//using (var scope = app.ApplicationServices.CreateScope())
			//{
			//	var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
			//	db.Database.Migrate();
			//}
		}
	}
}
