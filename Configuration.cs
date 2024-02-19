namespace Blogg;

public static class  Configuration
{
    public static string JwtKey = "daspokdq7winrqp5sdqwpdkqw8ok1qwdq";

    public static string ApiKeyName = "api_key";
    public static string ApiKey = "sakdawkqowpddsq1sda584sadas81sda56";
    public static StmpConfirguration Stmp {get; set;}//metodo para configurar o email

    public class StmpConfirguration//metodo para configurar o email
    {
        public static string Host {get; set;}
        public static int Port {get; set;}= 25;
        public static string UserName {get; set;}
        public static string Password {get; set;}
    }
}