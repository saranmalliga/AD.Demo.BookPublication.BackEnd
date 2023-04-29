using AD.Demo.BookPublication.Domain.AutoMapper;
using AD.Demo.BookPublication.Domain.Interfaces;
using AD.Demo.BookPublication.Interfaces.Interfaces;
using AD.Demo.BookPublication.Services;
using AD.Demo.BookPublication.Services.BusinessServices;
using AD.Demo.BookPublication.SQL.Infrastructure.Data;
using AD.Demo.BookPublication.SQL.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SD.BuildingBlocks.Infrastructure;
using System.Data;

namespace AD.Demo.BookPublication.Api.DependencyConfig
{
    public static class DependencyConfig
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped<IBookRepository, BookRepository>();
        }

        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(EntityToDtoMappings));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EntityToDtoMappings());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            DefaultServices.Mapper = mapper;
        }
        public static void AddDapper(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BookMgmtCoreConnection");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddTransient<IDbConnection>(c => new SqlConnection(connectionString));
        }


    }
}
