namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process__PostProcess__Task_Check
{
    [Fact(DisplayName = "No.36 プロパティ\"output_name\"がnull")]
    public void Case036()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask(null);

        Assert.ThrowsAny<Exception>(() => task.Check());
    }

    [Fact(DisplayName = "No.37 プロパティ\"output_name\"が空文字")]
    public void Case037()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("");

        Assert.ThrowsAny<Exception>(() => task.Check());
    }

    [Fact(DisplayName = "No.38 プロパティ\"output_name\"がnull/空文字以外")]
    public void Case038()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("test-no38-output");

        task.Check();
    }

    [Fact(DisplayName = "No.39 プロパティ\"set_values\"の要素数が0")]
    public void Case039()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("test-no39-output");
        task.set_values = [];

        task.Check();
    }

    [Fact(DisplayName = "No.40 プロパティ\"set_values\"の要素数が2")]
    public void Case040()
    {
        var task = MyDesiredPropertiesCreater.CreateValidPostProcessTask("test-no40-output");
        task.set_values = Enumerable.Range(0, 2)
            .Select(i => MyDesiredPropertiesCreater.CreateValidSetValue($"$.dummyKey{i}"))
            .ToList();

        task.Check();
    }
}
