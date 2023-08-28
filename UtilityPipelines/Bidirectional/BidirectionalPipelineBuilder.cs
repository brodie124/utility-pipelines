namespace UtilityPipelines.Bidirectional;

public interface IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
{
    IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput> AddStep<TOutput>(Func<TNextInput, TOutput> input);
    IBidirectionalPipeline<TFirstInput, TLastOutput> CreatePipeline();
}

public class BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
    : IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TNextInput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineStepList;

    protected BidirectionalPipelineBuilder(ICollection<Func<object, Task<object>>>? pipelineStepList = null)
    {
        _pipelineStepList = pipelineStepList ?? new List<Func<object, Task<object>>>();
    }

    public IBidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput> AddStep<TOutput>(
        Func<TNextInput, TOutput> input
    )
    {
        _pipelineStepList.Add(childInput => Task.FromResult((object)input((TNextInput)childInput)!));
        return new BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TOutput>(_pipelineStepList);
    }

    public IBidirectionalPipeline<TFirstInput, TLastOutput> CreatePipeline()
    {
        return new BidirectionalPipeline<TFirstInput, TLastOutput>(_pipelineStepList);
    }
}

public class BidirectionalPipelineBuilder<TFirstInput, TLastOutput>
    : BidirectionalPipelineBuilder<TFirstInput, TLastOutput, TFirstInput>
{
    public BidirectionalPipelineBuilder(ICollection<Func<object, Task<object>>>? pipelineStepList = null)
        : base(pipelineStepList)
    {
    }
}