using Functional.Toolkit.OptionType;

var rnd = new Random();

//Sync
var maybeClass = await MaybeGetClass();

maybeClass
    .WhenSome(value =>
    {
        Console.WriteLine($"Proceed doing something with the value: {value.Id} {value.Name}");
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

Console.WriteLine($"Result returned: {result.HasValue}");

#region Methods

//Randomly creates ar not the object purposely
Task<Option<MyClass>> MaybeGetClass()
{
    
    if (rnd.Next(2) == 1)
    {
        Task.FromResult(Option.None<MyClass>());
    }
    return Task.FromResult(Option.From(MyClass.CreateNew("test")));
}
#endregion


#region FakeClass

public class MyClass
{
    public Guid Id { get; }
    public string Name { get; }

    private MyClass(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static MyClass CreateNew(string name) => new(Guid.NewGuid(), name);
}

#endregion