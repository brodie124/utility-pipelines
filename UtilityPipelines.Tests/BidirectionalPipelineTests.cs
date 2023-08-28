using UtilityPipelines.Bidirectional;

namespace UtilityPipelines.Tests;

public class BidirectionalPipelineTests
{
    [Theory]
    [InlineData(2, "the number is 2")]
    [InlineData(4, "the number is 4")]
    [InlineData(10, "the number is 10")]
    [InlineData(16, "the number is 16")]
    public void BidirectionalPipeline_WithValidSetup_ShouldReturnExpectedType(int input, string expectedOutput)
    {
        var pipeline = new BidirectionalPipelineBuilder<int, string>()
            .AddStep(x => $"the number is {x}")
            .CreatePipeline();

        var actualOutput = pipeline.Execute(input);
        Assert.Equal(expectedOutput, actualOutput);
    }
}