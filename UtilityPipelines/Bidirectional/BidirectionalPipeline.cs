namespace UtilityPipelines.Bidirectional;

public interface IBidirectionalPipeline<TInput, TOutput>
{
    public Task<TOutput> ExecuteAsync(TInput input);
    public TOutput Execute(TInput input);
}

public class BidirectionalPipeline<TFirstInput, TLastOutput> : IBidirectionalPipeline<TFirstInput, TLastOutput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineSteps;

    public BidirectionalPipeline(ICollection<Func<object, Task<object>>> pipelineSteps)
    {
        _pipelineSteps = pipelineSteps;
    }

    public async Task<TLastOutput> ExecuteAsync(TFirstInput input)
    {
        var nextInput = input as object;
        foreach (var stepFunc in _pipelineSteps)
        {
            nextInput = await stepFunc.Invoke(nextInput!);
        }

        return (TLastOutput)nextInput!;
    }
    
    public TLastOutput Execute(TFirstInput input)
    {
        return ExecuteAsync(input).Result;
    }
}