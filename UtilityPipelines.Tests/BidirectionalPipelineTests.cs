using UtilityPipelines.Bidirectional;

namespace UtilityPipelines.Tests;

public class BidirectionalPipelineTests
{
    [Theory]
    [InlineData(2, "the number is 2")]
    [InlineData(4, "the number is 4")]
    [InlineData(10, "the number is 10")]
    [InlineData(16, "the number is 16")]
    public void BidirectionalPipeline_WithSimpleInAndOut_ShouldReturnExpectedValue(int input, string expectedOutput)
    {
        var pipeline = new BidirectionalPipelineBuilder<int, string>()
            .AddStep(x => $"the number is {x}")
            .CreatePipeline();

        var actualOutput = pipeline.Execute(input);
        Assert.Equal(expectedOutput, actualOutput);
    }
    
    [Theory]
    [InlineData(2, "the square value is 4")]
    [InlineData(4, "the square value is 16")]
    [InlineData(10, "the square value is 100")]
    [InlineData(16, "the square value is 256")]
    public void BidirectionalPipeline_WithTypeExchange_ShouldReturnExpectedValue(int input, string expectedOutput)
    {
        var pipeline = new BidirectionalPipelineBuilder<int, string>()
            .AddStep(x => x * x)
            .AddStep(x => new { Value = x })
            .AddStep(x => x.Value)
            .AddStep(x => $"the square value is {x}")
            .CreatePipeline();

        var actualOutput = pipeline.Execute(input);
        Assert.Equal(expectedOutput, actualOutput);
    }
    
    [Theory]
    [InlineData(0, 32)]
    [InlineData(10, 50)]
    [InlineData(25, 77)]
    [InlineData(50, 122)]
    public void BidirectionalPipeline_FarenheitConverter_ShouldReturnCorrectValues(
        int inputCelcius, 
        int expectedOutputFarenheit
    )
    {
        var pipeline = new BidirectionalPipelineBuilder<int, int>()
            .AddStep(x => x * (9f/5f))
            .AddStep(x => x + 32)
            .AddStep(x => (int) x)
            .CreatePipeline();

        var actualOutput = pipeline.Execute(inputCelcius);
        Assert.Equal(expectedOutputFarenheit, actualOutput);
    }
}