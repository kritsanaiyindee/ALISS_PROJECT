using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ALISS.Areas.Identity;
using ALISS.Data;
using ALISS.Data.D4_UserManagement.AUTH;
using Radzen;
using ALISS.Data.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Blazored.SessionStorage;
using ALISS.Data.D4_UserManagement.MasterManagement;
using ALISS.Data.D4_UserManagement.UserManagement;
using ALISS.Data.D0_Master;
using ALISS.Data.Account;
using ALISS.Data.D2_Mapping;
using ALISS.Data.D1_Upload;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ALISS.Data.D3_Process;
using ALISS.Data.D5_HISData;
//using ALISS.Data.D6_Report.Antibiogram;
//using ALISS.Data.D6_Report.Glass;
//using ALISS.Data.D6_Report.Antibiotrend;

namespace ALISS
{
    public class Startup
    {    
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;        
        }
    
        public IConfiguration Configuration { get; }      

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddBlazoredSessionStorage();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //    {
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
            //        ClockSkew = TimeSpan.Zero
            //    });

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            //services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            
            services.AddSingleton<WeatherForecastService>();

            services.AddSingleton<GridDataService>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            //services.AddScoped<IFileSave, FileSave>();

            //Master
            services.AddSingleton<LoginUserData>();
            services.AddSingleton<ConfigDataService>();

            services.AddSingleton<LoginUserService>();
            services.AddSingleton<DropDownListDataService>();
            services.AddSingleton<MenuDataService>();
            services.AddSingleton<HomeService>();

            //AUTH
            services.AddSingleton<PasswordConfigService>();
            services.AddSingleton<ColumnConfigService>();
            services.AddSingleton<MenuService>();
            services.AddSingleton<RoleService>();
            services.AddSingleton<LoginLogService>();

            //MasterManagement
            services.AddSingleton<MasterHospitalService>();
            services.AddSingleton<MasterTemplateService>();
            services.AddSingleton<WardTypeService>();
            services.AddSingleton<SpecimenService>();
            services.AddSingleton<AntibioticService>();
            services.AddSingleton<OrganismService>();
            services.AddSingleton<QCRangeService>();
            services.AddSingleton<ExpertRuleService>();
            services.AddSingleton<WHONETColumnService>();

            //UserManagement
            services.AddSingleton<HospitalService>();
            services.AddSingleton<UserLoginService>();

            //Process
            services.AddSingleton<ProcessRequestService>();

            //Mapping
            services.AddSingleton<LabFileUploadService>();
            services.AddSingleton<MappingService>();
            services.AddSingleton<TemplateUploadService>();
            services.AddSingleton<FileUploadService>();

            //Report   
            //services.AddSingleton<ReportService>();
            //services.AddSingleton<AMPService>();
            //services.AddSingleton<GlassService>();
            //services.AddSingleton<AntibiogramTemplateService>();

            //HIS
            services.AddSingleton<HISFileUploadService>();
            services.AddSingleton<FileUploadManageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

           
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
