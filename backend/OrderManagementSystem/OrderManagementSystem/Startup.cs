using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderManagementSystem.Api.Middleware;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Uow;
using OrderManagementSystem.Operation.Validation;
using System.Reflection;
using OrderManagementSystem.Operation.Mapper;
using static OrderManagementSystem.Operation.Cqrs.UserCqrs;
using OrderManagementSystem.Base.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrderManagementSystem.Base.Logger;
using Serilog;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.ShoppingCartCqrs;
using OrderManagementSystem.Operation.Command;
using OrderManagementSystem.Repositories;
using OrderManagementSystem.Operation.Query;
using static OrderManagementSystem.Operation.Cqrs.OrderReportCqrs;
using System.Data;
using System.Data.SqlClient;

namespace OrderManagementSystem;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {

        string connection = Configuration.GetConnectionString("MsSqlConnection");
        services.AddDbContext<OmsDbContext>(opts => opts.UseSqlServer(connection));

        var JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);
        services.AddScoped<IRequestHandler<UpdateShoppingCartByIdCommand, ApiResponse<ShoppingCartResponse>>, ShoppingCartCommandHandler>();

        services.AddScoped<OrderReportRepository>();
        services.AddMediatR(typeof(Startup));
        services.AddTransient<IRequestHandler<GetOrderReportQuery, IEnumerable<OrderReportDto>>, OrderReportHandler>();
        services.AddScoped<IDbConnection>(c => new SqlConnection(Configuration.GetConnectionString("MsSqlConnection")));



        var config = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperConfig()); });
        services.AddSingleton(config.CreateMapper());
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });
        services.AddControllers();
        services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<BaseValidator>();
        });

        

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderManagementSystem Api Management", Version = "v1.0" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "OrderManagementSystem Management for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = JwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                ValidAudience = JwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderManagementSystem v1"));
        }

        app.UseMiddleware<HeartBeatMiddleware>();
        app.UseMiddleware<HeartBeatMiddleware>();
        Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
        {
            Log.Information("-------------Request-Begin------------");
            Log.Information(requestProfilerModel.Request);
            Log.Information(Environment.NewLine);
            Log.Information(requestProfilerModel.Response);
            Log.Information("-------------Request-End------------");
        };
        app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

        app.UseHttpsRedirection();
        //auth
        app.UseAuthentication();
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
