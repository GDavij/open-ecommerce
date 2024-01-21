namespace Core.Modules.Shared.Domain.ResultObjects;

public class EvaluationResult<T> 
    where T : class
{
    public T Eval { get; set; }

    public EvaluationResult(T eval)
    {
        Eval = eval;
    }
}


