namespace UtilityPipelines.Bidirectional;

public interface IBidirectionalPipeline<TInput, TOutput>
{
    public Task<TOutput> ExecuteAsync(TInput input);
    public TOutput Execute(TInput input);
}

public class BidirectionalPipeline<TFirstInput, TFirstOutput> : IBidirectionalPipeline<TFirstInput, TFirstOutput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineSteps;

    public BidirectionalPipeline(ICollection<Func<object, Task<object>>> pipelineSteps)
    {
        _pipelineSteps = pipelineSteps;
    }

    public async Task<TFirstOutput> ExecuteAsync(TFirstInput input)
    {
        var nextInput = input as object;
        foreach (var stepFunc in _pipelineSteps)
        {
            nextInput = await stepFunc.Invoke(nextInput!);
        }

        return (TFirstOutput)nextInput!;
    }
    
    public TFirstOutput Execute(TFirstInput input)
    {
        return ExecuteAsync(input).Result;
    }
}