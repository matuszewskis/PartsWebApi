using PartsWebApi;

class Program
{
    static void Main(string[] args)
    {
        InitializeHost();
    }

    private static void InitializeHost() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            }).Build().Run();

}