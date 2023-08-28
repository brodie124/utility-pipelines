namespace UtilityPipelines.InputOnly;

public interface IInputOnlyPipelineBuilder<TFirstInput, TNextInput>
{
    IInputOnlyPipelineBuilder<TFirstInput, TOutput> AddStep<TOutput>(Func<TNextInput, TOutput> input);
    IInputOnlyPipeline<TFirstInput> CreatePipeline();
}

public class InputOnlyPipelineBuilder<TFirstInput, TNextInput>
    : IInputOnlyPipelineBuilder<TFirstInput, TNextInput>
{
    private readonly ICollection<Func<object, Task<object>>> _pipelineStepList;

    protected InputOnlyPipelineBuilder(ICollection<Func<object, Task<object>>>? pipelineStepList = null)
    {
        _pipelineStepList = pipelineStepList ?? new List<Func<object, Task<object>>>();
    }

    public IInputOnlyPipelineBuilder<TFirstInput, TOutput> AddStep<TOutput>(
        Func<TNextInput, TOutput> input
    )
    {
        _pipelineStepList.Add(childInput => Task.FromResult((object)input((TNextInput)childInput)!));
        return new InputOnlyPipelineBuilder<TFirstInput, TOutput>(_pipelineStepList);
    }

    public IInputOnlyPipeline<TFirstInput> CreatePipeline()
    {
        return new InputOnlyPipeline<TFirstInput>(_pipelineStepList);
    }
}

public class InputOnlyPipelineBuilder<TFirstInput> : InputOnlyPipelineBuilder<TFirstInput, TFirstInput>
{
    public InputOnlyPipelineBuilder(ICollection<Func<object, Task<object>>>? pipelineStepList = null)
        : base(pipelineStepList)
    {
    }
}