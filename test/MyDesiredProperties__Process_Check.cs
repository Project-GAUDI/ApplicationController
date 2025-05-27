namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process_Check
{
    [Fact(DisplayName = "No.16 プロパティ\"process_name\"がnull")]
    public void Case016()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess(null);

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.17 プロパティ\"process_name\"が空文字")]
    public void Case017()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess("");

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.18 プロパティ\"process_name\"がnull/空文字以外")]
    public void Case018()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess("test-no17-process");

        process.Check();
    }

    [Fact(DisplayName = "No.19 プロパティ\"application\"がnull")]
    public void Case019()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess("test-no18-process");
        process.application = null;

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.20 プロパティ\"post_processes\"の要素数が0")]
    public void Case020()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess("test-no18-process");
        process.post_processes = [];

        process.Check();
    }

    [Fact(DisplayName = "No.21 プロパティ\"post_processes\"の要素数が2")]
    public void Case021()
    {
        var process = MyDesiredPropertiesCreater.CreateValidProcess("test-no18-process");
        process.post_processes = Enumerable.Range(0, 2)
            .Select(i => MyDesiredPropertiesCreater.CreateValidPostProcess())
            .ToList();

        process.Check();
    }
}
