using System.Text;
using Blog.Data;
using Blogg;
using Blogg.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigurationAuthentication(builder);//metodo para configurar a autenticação
ConfigureMvc(builder);//metodo para configurar o mvc
ConfigureService(builder);//metodo para configurar o serviço


var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");//pegando o valor do appsettings
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.StmpConfirguration();//Estanciando a classe
    app.Configuration.GetSection("Stmp").Bind(smtp);//metodo para configurar o email
    Configuration.Stmp = smtp;
}

void ConfigurationAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//autenticação
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//desafio
    }).AddJwtBearer(x =>
    {

        x.TokenValidationParameters = new TokenValidationParameters//validando o token
        {
            ValidateIssuerSigningKey = true,//validando a chave
            IssuerSigningKey = new SymmetricSecurityKey(key),//chave de segurança
            ValidateIssuer = false,//validando o emissor
            ValidateAudience = false//validando o publico
        };
    });

}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
}

void ConfigureService(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>();//sempre cria um nov
}