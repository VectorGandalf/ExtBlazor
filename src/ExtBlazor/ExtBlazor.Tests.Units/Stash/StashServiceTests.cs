using ExtBlazor.Stash;

namespace ExtBlazor.Tests.Units.Stash;
public class StashServiceTests
{
    [Fact]
    public void PutGet_OneObject_OneUri()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();
        currentUriProvider.Uri = "testuri1";
        IStashService stash = new StashService(currentUriProvider);
        var key = "Object 1";
        var putObject = new { Prop1 = 1, Prop2 = "test" };

        //Act        
        stash.Put(key, putObject);
        var getObject = stash.Get<object>(key);

        //Assert
        Assert.Equivalent(putObject, getObject);
    }

    [Fact]
    public void PutGet_MultipleObjects_SameUri()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();
        currentUriProvider.Uri = "testuri1";
        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        var key2 = "Object 2";
        var putObject2 = new { Prop1 = 3, Prop2 = "test89" };

        //Act        
        stash.Put(key1, putObject1);
        var getObject1 = stash.Get<object>(key1);

        stash.Put(key2, putObject2);
        var getObject2 = stash.Get<object>(key2);

        //Assert
        Assert.NotEqual(getObject1, getObject2);
    }

    [Fact]
    public void PutGet_SameKeyName_DifferentUris()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();

        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        var key2 = "Object 1";
        var putObject2 = new { Prop1 = 3, Prop2 = "test89" };

        //Act
        currentUriProvider.Uri = "testuri1";
        stash.Put(key1, putObject1);

        currentUriProvider.Uri = "testuri2";
        stash.Put(key2, putObject2);
        var getObject2 = stash.Get<object>(key2);

        currentUriProvider.Uri = "testuri1";
        var getObject1 = stash.Get<object>(key1);

        //Assert
        Assert.NotEqual(getObject1, getObject2);
    }

    [Fact]
    public void HasValue_True()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();

        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        //Act
        currentUriProvider.Uri = "testuri1";
        stash.Put(key1, putObject1);
        var hasValue = stash.HasValue(key1);

        //Assert
        Assert.True(hasValue);
    }

    [Fact]

    public void HasValue_False_DifferenUri()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();

        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        //Act
        currentUriProvider.Uri = "testuri1";
        stash.Put(key1, putObject1);

        currentUriProvider.Uri = "testuri2";
        var hasValue = stash.HasValue(key1);

        //Assert
        Assert.False(hasValue);
    }

    [Fact]

    public void HasValue_False_NoPut()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();

        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        //Act
        currentUriProvider.Uri = "testuri1";
        var hasValue = stash.HasValue(key1);

        //Assert
        Assert.False(hasValue);
    }

    [Fact]
    public void PutGet_Clear()
    {
        //Arrange
        var currentUriProvider = new TestCurrentUriProvider();

        IStashService stash = new StashService(currentUriProvider);
        var key1 = "Object 1";
        var putObject1 = new { Prop1 = 1, Prop2 = "test" };

        var key2 = "Object 1";
        var putObject2 = new { Prop1 = 3, Prop2 = "test89" };

        //Act
        currentUriProvider.Uri = "testuri1";
        stash.Put(key1, putObject1);

        currentUriProvider.Uri = "testuri2";
        stash.Put(key2, putObject2);

        stash.Clear();

        var getObject2 = stash.Get<object>(key2);

        currentUriProvider.Uri = "testuri1";
        var getObject1 = stash.Get<object>(key1);

        //Assert
        Assert.Null(getObject1);
        Assert.Null(getObject2);
    }

    private class TestCurrentUriProvider : ICurrentUriProvider
    {
        public string? Uri { get; set; }
    }

    private record TestType(int Prop1, string Prop2);
}
