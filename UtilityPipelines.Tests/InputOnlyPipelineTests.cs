using UtilityPipelines.InputOnly;

namespace UtilityPipelines.Tests;

public class InputOnlyPipelineTests
{
    [Theory]
    [InlineData(2, "the number is 2")]
    [InlineData(4, "the number is 4")]
    [InlineData(10, "the number is 10")]
    [InlineData(16, "the number is 16")]
    public void InputOnlyPipeline_CanModifyInputObject_ShouldReturnExpectedValue(int input, string expectedOutput)
    {
        var pipeline = new InputOnlyPipelineBuilder<TestClass>()
            .AddStep(x => x.SomeString = $"MyTestString for value {input}")
            .CreatePipeline();

        var testClass = new TestClass();
        pipeline.Execute(testClass);
        
        Assert.Equal($"MyTestString for value {input}", testClass.SomeString);
    }
    
    // [Theory]
    // [InlineData(2, "the square value is 4")]
    // [InlineData(4, "the square value is 16")]
    // [InlineData(10, "the square value is 100")]
    // [InlineData(16, "the square value is 256")]
    // public void InputOnly_WithTypeExchange_ShouldReturnExpectedValue(int input, string expectedOutput)
    // {
    //     var pipeline = new BidirectionalPipelineBuilder<int, string>()
    //         .AddStep(x => x * x)
    //         .AddStep(x => new { Value = x })
    //         .AddStep(x => x.Value)
    //         .AddStep(x => $"the square value is {x}")
    //         .CreatePipeline();
    //
    //     var actualOutput = pipeline.Execute(input);
    //     Assert.Equal(expectedOutput, actualOutput);
    // }
}

public class TestClass
{
    public string SomeString { get; set; } = string.Empty;
}