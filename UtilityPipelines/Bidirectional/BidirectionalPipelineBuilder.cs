namespace UtilityPipelines.Bidirectional;

/// <summary>
/// Dynamically creates a bi-directional pipeline from a list of steps defined in a fluent manner.
/// </summary>
/// <typeparam name="TFirstInput">The input to the first step of the pipeline</typeparam>
/// <typeparam name="TLastOutput">The output from the last step of the pipeline</typeparam>
/// <typeparam name="TNextInput">The input into the NEXT step of the pipeline (the output of the previous step)</typeparam>
public interface IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
{
    /// <summary>
    /// Add a step to the pipeline.
    /// </summary>
    /// <param name="input">Lambda to be executed</param>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput> AddStep<TOutput>(Func<TNextInput, TOutput> input);
    
    /// <summary>
    /// Creates a bi-directional pipeline using the already-defined steps.
    /// </summary>
    /// <returns></returns>
    IBidirectionalPipeline<TFirstInput, TLastOutput> CreatePipeline();
}

/// <summary>
/// Dynamically creates a bi-directional pipeline from a list of steps defined in a fluent manner.
/// </summary>
public class BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
    : IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineStepList;

    /// <summary>
    /// Creates a bi-directional pipeline builder from an existing step of steps.
    /// If no existing list of steps is provided, a new collection is created.
    /// </summary>
    /// <param name="pipelineStepList">Existing collection of steps</param>
    protected BidirectionalPipelineBuilder(ICollection<Func<object, Task<object>>>? pipelineStepList)
    {
        _pipelineStepList = pipelineStepList ?? new List<Func<object, Task<object>>>();
    }

    /// <summary>
    /// Adds a step to the pipeline
    /// </summary>
    /// <param name="input"></param>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    public IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput> AddStep<TOutput>(
        Func<TNextInput, TOutput> input
    )
    {
        _pipelineStepList.Add(childInput => Task.FromResult((object)input((TNextInput)childInput)!));
        return new BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput>(_pipelineStepList);
    }

    /// <summary>
    /// Creates an instance of IBidirectionalPipeline based on the previously defined steps.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IBidirectionalPipeline<TFirstInput, TLastOutput> CreatePipeline()
    {
        if (typeof(TNextInput) != typeof(TLastOutput))
            throw new InvalidOperationException("The final step of the pipeline does not return the correct type");

        return new BidirectionalPipeline<TFirstInput, TLastOutput>(_pipelineStepList);
    }
}

/// <summary>
/// Dynamically creates a bi-directional pipeline from a list of steps defined in a fluent manner.
/// </summary>
public class BidirectionalPipelineBuilder<TFirstInput, TLastOutput>
    : BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TFirstInput>
{
    /// <summary>
    /// Creates a bi-directional pipeline builder.
    /// </summary>
    public BidirectionalPipelineBuilder() : base(null)
    {
    }
}