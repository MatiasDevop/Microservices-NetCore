using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApi.Data.Database;
using CustomerApi.Data.Repository.v1;
using CustomerApi.Domain.Entities;
using CustomerApi.Messaging.Send.Options.v1;
using CustomerApi.Messaging.Send.Sender.v1;
using CustomerApi.Models.v1;
using CustomerApi.Service.v1.Command;
using CustomerApi.Service.v1.Query;
using CustomerApi.Validators.v1;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CustomerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            services.AddDbContext<CustomerContext>(Options => Options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            //services.AddDbContext<CustomContext>(options => options.UseSqlServer(Configuration["Database:ConnectionString"]));
            services.AddAutoMapper(typeof(Startup));
            services.AddMvcCore();//.AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Customer Api",
                    Description = "A simple API to create or update customers",
                    Contact = new OpenApiContact
                    {
                        Name = "Wolfgang Ofner",
                        Email = "WolfgangOfner@aon.at",
                        Url = new Uri("https://www.programmingwithwolfgang.com/")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as ActionExecutingContext;
                    if (actionContext.ModelState.ErrorCount > 0 
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IValidator<CreateCustomerModel>, CreateCustomerModelValidator>();
            services.AddTransient<IValidator<UpdateCustomerModel>, UpdateCustomerModelValidator>();

            services.AddTransient<ICustomerUpdateSender, CustomerUpdateSender>();

            services.AddTransient<IRequestHandler<CreateCustomerCommand, Customer>, CreateCustomerCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateCustomerCommand, Customer>, UpdateCustomerCommandHandler>();
            services.AddTransient<IRequestHandler<GetCustomerByIdQuery, Customer>, GetCustomerByIdQueryHandler>();
               

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
