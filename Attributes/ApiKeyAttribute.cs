using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Blogg.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)//metodo para verificar se a chave da api esta correta
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(Configuration.ApiKeyName, out var extractedApiKey))
            {
               context.Result = new ContentResult()//metodo para verificar se a chave da api esta correta com base na chave da api
               {
                   StatusCode = 401,
                   Content = "Api Key não encontrada"//mensagem de erro
               };
                return;
            }

            if (!Configuration.ApiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Acesso não autorizado"//mensagem de erro
                };
                return;
            }

            await next();
        }

    }
}