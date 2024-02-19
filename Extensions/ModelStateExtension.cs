using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blogg.Extensions
{
    public static class ModelStateExtension
    {
       public static List<string> GetErrors(this ModelStateDictionary modelState)//sempre statico conforme a classe
       {
           var result = new List<string>();
           foreach (var item in modelState.Values)
               result.AddRange(item.Errors.Select(e => e.ErrorMessage));
           
            return result;
       }
    }
}