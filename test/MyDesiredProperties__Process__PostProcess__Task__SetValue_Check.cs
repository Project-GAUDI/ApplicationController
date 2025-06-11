namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process__PostProcess__Task__SetValue_Check
{
    [Fact(DisplayName = "No.41 プロパティ\"set_values.type\"が\"Body\"")]
    public void Case041()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue();
        setValue.type = MyDesiredProperties.MessageData.Body;

        setValue.Check();
    }

    [Fact(DisplayName = "No.42 プロパティ\"set_values.type\"が\"Properties\"")]
    public void Case042()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue();
        setValue.type = MyDesiredProperties.MessageData.Properties;

        setValue.Check();
    }

    [Fact(DisplayName = "No.43 プロパティ\"set_values.type\"が\"Body\"/\"Properties\"以外")]
    public void Case043()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue();
        setValue.type = MyDesiredProperties.MessageData.NotSelected;

        Assert.ThrowsAny<Exception>(() => setValue.Check());
    }

    [Fact(DisplayName = "No.44 プロパティ\"key\"がnull")]
    public void Case044()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyKey: null);

        Assert.ThrowsAny<Exception>(() => setValue.Check());
    }

    [Fact(DisplayName = "No.45 プロパティ\"key\"が空文字")]
    public void Case045()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyKey: "");

        Assert.ThrowsAny<Exception>(() => setValue.Check());
    }

    [Fact(DisplayName = "No.46 プロパティ\"key\"がnull/空文字以外")]
    public void Case046()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyKey: "$.key");

        setValue.Check();
    }

    [Fact(DisplayName = "No.47 プロパティ\"value\"がnull")]
    public void Case047()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyValue: null);

        Assert.ThrowsAny<Exception>(() => setValue.Check());
    }

    [Fact(DisplayName = "No.48 プロパティ\"value\"が空文字")]
    public void Case048()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyValue: "");

        Assert.ThrowsAny<Exception>(() => setValue.Check());
    }

    [Fact(DisplayName = "No.49 プロパティ\"value\"がnull/空文字以外")]
    public void Case049()
    {
        var setValue = MyDesiredPropertiesCreater.CreateValidSetValue(dummyValue: "TestValue");

        setValue.Check();
    }
}
