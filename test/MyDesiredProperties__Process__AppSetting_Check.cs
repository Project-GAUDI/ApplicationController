namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process__AppSetting_Check
{
    [Fact(DisplayName = "No.50 プロパティ\"Application.type\"がhttp")]
    public void Case050()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.http;

        appSetting.Check();
    }

    [Fact(DisplayName = "No.51 プロパティ\"Application.type\"が\"http\"以外")]
    public void Case051()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.type = MyDesiredProperties.Process.AppSetting.Protocol.NotSelected;

        Assert.ThrowsAny<Exception>(() => appSetting.Check());
    }

    [Fact(DisplayName = "No.52 プロパティ\"url\"がnull")]
    public void Case052()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.url = null;

        Assert.ThrowsAny<Exception>(() => appSetting.Check());
    }

    [Fact(DisplayName = "No.53 プロパティ\"url\"が空文字")]
    public void Case053()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.url = "";

        Assert.ThrowsAny<Exception>(() => appSetting.Check());
    }

    [Fact(DisplayName = "No.54 プロパティ\"url\"がnull/空文字以外")]
    public void Case054()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.url = "https://test-dummy-host.org";

        appSetting.Check();
    }

    [Fact(DisplayName = "No.55 プロパティ\"replace_params\"の要素数が0")]
    public void Case055()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.replace_params = [];

        appSetting.Check();
    }

    [Fact(DisplayName = "No.56 プロパティ\"replace_params\"の要素数が2")]
    public void Case056()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.replace_params = Enumerable.Range(0, 2)
            .Select(i => MyDesiredPropertiesCreater.CreateValidReplaceParam())
            .ToList();

        appSetting.Check();
    }

    [Fact(DisplayName = "No.57 プロパティ\"url\"にプロパティ\"base_name\"が含まれない")]
    public void Case057()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.url = "http://test-app-setting-host.org";

        var invalidReplaceParam = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        invalidReplaceParam.base_name = "not-contains-name";

        appSetting.replace_params = [ invalidReplaceParam ];

        Assert.ThrowsAny<Exception>(() => appSetting.Check());
    }

    [Fact(DisplayName = "No.58 プロパティ\"url\"にプロパティ\"base_name\"が含まれる")]
    public void Case058()
    {
        var appSetting = MyDesiredPropertiesCreater.CreateValidAppSetting();
        appSetting.url = "http://test-contains-base-name-host.org";

        var validReplaceParam = MyDesiredPropertiesCreater.CreateValidReplaceParam();
        validReplaceParam.base_name = "contains-base-name";

        appSetting.replace_params = [ validReplaceParam ];

        appSetting.Check();
    }
}
