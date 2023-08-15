# 🚀 Functional.Toolkit.OptionType

Functional.Toolkit.OptionType is a lightweight library that provides an implementation of the Option type in C#.
The Option type is a functional programming concept that represents a value that may or may not be present. 
This can help reduce the number of null checks in your code, making it more robust and easier to reason about.

_Here its just a few examples, check all the available extensions in the [OptionExtensions.cs](https://github.com/ricardotondello/Functional.Toolkit.OptionType/blob/main/src/Functional.Toolkit.OptionType/OptionExtensions.cs) file_

## Usage

```csharp
//Object Initialization

var option1 = Option.From((ClassForTest)null); //will automatically determine if its None or Some based on the generic type
var option2 = Option.From(new ClassForTest());
var option3 = Option.None<ClassForTest>();
var option4 = Option.Some(new ClassForTest());
...

//Sync
var maybeClass = await MaybeGetClass();

maybeClass
    .WhenSome(value =>
    {
        Console.WriteLine($"Proceed doing something with the value: {value.Name}");
        //It could be whatever you want, here you have the value of the type of the Option
        //Example:
        //Call another methods, write in the DB, Publish to a Service Bus, infinity...
    })
    .WhenNone(() =>
    {
        Console.WriteLine("Proceed as a fallback without the value");
        //since the Option is None the value couldn't be found.
        //Example:
        //Write logs, alerts, whatever.
    })
    .WhenAny(maybeValue =>
    {
        Console.WriteLine("Despite having a value or not this will be execute and you can decide what to do.");
        Console.WriteLine($"Does my object has a value? {maybeValue.HasValue}");
    });


//Async
var maybeClassTask = MaybeGetClass();

var result = await maybeClassTask
    .WhenSomeAsync(value => 
    { 
        Console.WriteLine($"Proceed doing something with the value: {value.Name}");
        return Task.CompletedTask;
    })
    .WhenNoneAsync(() =>
    {
        Console.WriteLine("Proceed as a fallback without the value");
        return Task.CompletedTask;
    }).WhenAnyAsync(maybeValue =>
    {
        Console.WriteLine($"Does my object has a value? {maybeValue.HasValue}");
    });
```