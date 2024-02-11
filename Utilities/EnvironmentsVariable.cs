namespace server.Utilities;

public static class EnvironmentsVariable
{
      private static readonly IConfiguration configuration;

       static EnvironmentsVariable()
      {
         configuration = new ConfigurationBuilder()
                                              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                              .Build();
      }

     public static string ClientSecret()
     {
         var clientId =  configuration["AppSettings:CLIENT_ID"];
         return clientId!;
     }

}