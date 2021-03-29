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
using ALISS.Data.D6_Report.Antibiogram;
using ALISS.Data.D6_Report.Glass;
using ALISS.Data.D6_Report.Antibiotrend;

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

            services.AddScoped<GridDataService>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<IFileSave, FileSave>();

            //Master
            services.AddSingleton<LoginUserDataList>();

            services.AddScoped<LoginUserData>();
            services.AddScoped<ConfigDataService>();

            services.AddScoped<LoginUserService>();
            services.AddScoped<DropDownListDataService>();
            services.AddScoped<MenuDataService>();
            services.AddScoped<HomeService>();
            services.AddScoped<WHONETService>();

            //AUTH
            services.AddScoped<PasswordConfigService>();
            services.AddScoped<ColumnConfigService>();
            services.AddScoped<MenuService>();
            services.AddScoped<RoleService>();
            services.AddScoped<LoginLogService>();

            //MasterManagement
            services.AddScoped<MasterHospitalService>();
            services.AddScoped<MasterTemplateService>();
            services.AddScoped<WardTypeService>();
            services.AddScoped<SpecimenService>();
            services.AddScoped<AntibioticService>();
            services.AddScoped<OrganismService>();
            services.AddScoped<QCRangeService>();
            services.AddScoped<ExpertRuleService>();
            services.AddScoped<WHONETColumnService>();

            //UserManagement
            services.AddScoped<HospitalService>();
            services.AddScoped<UserLoginService>();

            //Process
            services.AddScoped<ProcessRequestService>();

            //Mapping
            services.AddScoped<LabFileUploadService>();
            services.AddScoped<MappingService>();
            services.AddScoped<TemplateUploadService>();
            services.AddScoped<FileUploadService>();

            //Report   
            services.AddScoped<ReportService>();
            services.AddSingleton<AMPService>();
            services.AddSingleton<GlassService>();
            services.AddScoped<AntibiogramTemplateService>();

            //HIS
            services.AddScoped<HISFileUploadService>();
            services.AddScoped<FileUploadManageService>();
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
