# Utility Pipelines
Utility Pipelines provides a collection of generic, sequential pipeline builders.

## Bidirectional Pipeline
As the name suggests, a `bidirectional pipeline` takes in an input and produces an output.
A simple example of a bidirectional pipeline would be a pipeline that takes in a number and squares it.
```csharp
var pipeline = new BidirectionalPipelineBuilder<int, int>()
    .AddStep(x => x * x)
    .CreatePipeline();

var outputA = pipeline.Execute(3);  // Value would be 9
var outputB = pipeline.Execute(16); // Value would be 256
```

Whilst the input type and output type are predefined for the pipeline, it remains possible to mutate the type of the
object as it passes through the pipeline.

Consider this example pipeline that: 1) converts pennies to pounds, 2) rounds this value to two decimal places, 
and 3) outputs the value as a string with the '£' symbol prefixed.

```csharp
var pipeline = new BidirectionalPipelineBuilder<int, string>()
    .AddStep(x => x / 100f)         // Accepts int, returns float
    .AddStep(x => Math.Round(x, 2)) // Accepts float, returns double
    .AddStep(x => $"£{x}")          // Accepts double, returns string
    .CreatePipeline();

var outputA = pipeline.Execute(316); // Value would be £3.16
var outputB = pipeline.Execute(16);  // Value would be £0.16
var outputC = pipeline.Execute(0);  // Value would be £0
```