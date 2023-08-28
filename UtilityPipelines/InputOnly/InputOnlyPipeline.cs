namespace UtilityPipelines.InputOnly;

public interface IInputOnlyPipeline<TInput>
{
    public Task ExecuteAsync(TInput input);
    public void Execute(TInput input);
}

public class InputOnlyPipeline<TInput> : IInputOnlyPipeline<TInput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineSteps;

    public InputOnlyPipeline(ICollection<Func<object, Task<object>>> pipelineSteps)
    {
        _pipelineSteps = pipelineSteps;
    }

    public async Task ExecuteAsync(TInput input)
    {
        var nextInput = input as object;
        foreach (var stepFunc in _pipelineSteps)
        {
            nextInput = await stepFunc.Invoke(nextInput!);
        }
    }
    
    public void Execute(TInput input)
    {
        ExecuteAsync(input).Wait();
    }
}