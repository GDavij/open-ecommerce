using System.Net;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class ValidationResult<T> 
    where T : class
{
    
   public bool IsValid { get; }
   public HttpStatusCode Code { get; }
   public T? Result { get; }

   private ValidationResult(bool isValid, HttpStatusCode code, T? result = null)
   {
       IsValid = isValid;
       Code = code;
       Result = result;
   }

   public static ValidationResult<T> Error(HttpStatusCode code)
   {
       return new ValidationResult<T>(false, code);
   }

   public static ValidationResult<T> Success(T result)
   {
       return new ValidationResult<T>(true, HttpStatusCode.OK, result);
   }
}
