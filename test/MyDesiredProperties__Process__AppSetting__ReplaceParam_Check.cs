namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process__AppSetting__ReplaceParam_Check
{
    [Fact(DisplayName = "No.59 プロパティ\"base_name\"がnull")]
    public void Case059()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.base_name = null;

        Assert.ThrowsAny<Exception>(() => param.Check());
    }

    [Fact(DisplayName = "No.60 プロパティ\"base_name\"が空文字")]
    public void Case060()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.base_name = "";

        Assert.ThrowsAny<Exception>(() => param.Check());
    }

    [Fact(DisplayName = "No.61 プロパティ\"base_name\"がnull/空文字以外")]
    public void Case061()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.base_name = "valid-base-name";

        param.Check();
    }

    [Fact(DisplayName = "No.62 プロパティ\"source_data_type\"が\"Body\"")]
    public void Case062()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_type = MyDesiredProperties.MessageData.Body;

        param.Check();
    }

    [Fact(DisplayName = "No.63 プロパティ\"source_data_type\"が\"Properties\"")]
    public void Case063()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_type = MyDesiredProperties.MessageData.Properties;

        param.Check();
    }

    [Fact(DisplayName = "No.64 プロパティ\"source_data_type\"が\"Body\"/\"Properties\"以外")]
    public void Case064()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_type = MyDesiredProperties.MessageData.NotSelected;

        Assert.ThrowsAny<Exception>(() => param.Check());
    }

    [Fact(DisplayName = "No.65 プロパティ\"source_data_key\"がnull")]
    public void Case065()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_key = null;

        Assert.ThrowsAny<Exception>(() => param.Check());
    }

    [Fact(DisplayName = "No.66 プロパティ\"source_data_key\"が空文字")]
    public void Case066()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_key = "";

        Assert.ThrowsAny<Exception>(() => param.Check());
    }

    [Fact(DisplayName = "No.67 プロパティ\"source_data_key\"がnull/空文字以外")]
    public void Case067()
    {
        var param = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        param.source_data_key = "valid-data-key";

        param.Check();
    }
}
