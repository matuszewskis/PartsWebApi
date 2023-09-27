using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PartsWebApi.Repositories;
using PartsWebApi.Utils.Helpers;

namespace PartsWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddDbContext<ApiContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IPartHelper, PartHelper>();

            ConfigureAutoMapper(services);
        }

        public void Configure(IApplicationBuilder app, ApiContext dbContext)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
