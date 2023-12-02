namespace Core.Modules.Shared.Domain.ResultObjects;

public class ValidationResult<T> 
    where T : class
{
    
   public bool IsValid { get; }
   public T? Result { get; }

   private ValidationResult(bool isValid, T? result = null)
   {
       IsValid = isValid;
       Result = result;
   }

   public static ValidationResult<T> Error()
   {
       return new ValidationResult<T>(false);
   }

   public static ValidationResult<T> Success(T result)
   {
       return new ValidationResult<T>(true, result);
   }
}
