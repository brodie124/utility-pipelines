namespace UtilityPipelines.Bidirectional;

/// <summary>
/// Bi-directional pipeline taking in one input and providing exactly one output.
/// Asynchronous and synchronous execution allowed.
/// </summary>
/// <typeparam name="TInput">Input of the first step in the pipeline</typeparam>
/// <typeparam name="TOutput">Output from the last step in the pipeline</typeparam>
public interface IBidirectionalPipeline<TInput, TOutput>
{
    /// <summary>
    /// Asynchronously executes the pipeline with the provided input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<TOutput> ExecuteAsync(TInput input);
    
    /// <summary>
    /// Synchronously executes the pipeline with the provided input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public TOutput Execute(TInput input);
}

/// <summary>
/// Bi-Directional pipeline implementation - taking in one input and providing exactly one output.
/// Capable of both synchronous and asynchronous execution.
/// </summary>
/// <typeparam name="TFirstInput">Input of the first step in the pipeline</typeparam>
/// <typeparam name="TLastOutput">Output from the last step in the pipeline</typeparam>
public class BidirectionalPipeline<TFirstInput, TLastOutput> : IBidirectionalPipeline<TFirstInput, TLastOutput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineSteps;

    /// <summary>
    /// Creates a bi-directional pipeline based on the provided list of steps.
    /// </summary>
    /// <param name="pipelineSteps"></param>
    public BidirectionalPipeline(ICollection<Func<object, Task<object>>> pipelineSteps)
    {
        _pipelineSteps = pipelineSteps;
    }

    /// <summary>
    /// Executes the pipeline asynchronously - whilst still remaining in sequence.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<TLastOutput> ExecuteAsync(TFirstInput input)
    {
        var nextInput = input as object;
        foreach (var stepFunc in _pipelineSteps)
        {
            nextInput = await stepFunc.Invoke(nextInput!);
        }

        return (TLastOutput)nextInput!;
    }
    
    /// <summary>
    /// Executes the pipeline synchronously.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public TLastOutput Execute(TFirstInput input)
    {
        return ExecuteAsync(input).Result;
    }
}