using AD.Demo.BookPublication.Domain.AutoMapper;
using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Interfaces;
using AD.Demo.BookPublication.ElasticSearch.Infrastructure.Repositories;
using AD.Demo.BookPublication.Interfaces.Interfaces;
using AD.Demo.BookPublication.Services;
using AD.Demo.BookPublication.Services.BusinessServices;
using AD.Demo.BookPublication.SQL.Infrastructure.Data;
using AD.Demo.BookPublication.SQL.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
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
            services.AddScoped<IBookRepositoryES, BookRepositoryES>();
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

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];
            var settings = new ConnectionSettings(new Uri(baseUrl ?? "")).PrettyJson().CertificateFingerprint("6b6a8c2ad2bc7b291a7363f7bb96a120b8de326914980c868c1c0bc6b3dc41fd").BasicAuthentication("elastic", "JbNb_unwrJy3W0OaZ07n").DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<BookDTO>(m=> m);
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<BookDTO>(x => x.AutoMap()));
        }
    }
}
