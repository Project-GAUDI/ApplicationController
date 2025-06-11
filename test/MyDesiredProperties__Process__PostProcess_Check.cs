namespace IotedgeV2ApplicationController.Test;

public class MyDesiredProperties__Process__PostProcess_Check
{
    [Fact(DisplayName = "No.22 プロパティ\"condition_path\"がnull、かつ、プロパティ\"condition_operator\"がnull/空文字以外")]
    public void Case022()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = null;
        process.condition_operator = "test-no22-operator";

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.23 プロパティ\"condition_path\"が空文字、かつ、プロパティ\"condition_operator\"がnull/空文字以外")]
    public void Case023()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "";
        process.condition_operator = "test-no23-operator";

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.24 プロパティ\"condition_path\"がnull/空文字以外、かつ、プロパティ\"condition_operator\"がnull")]
    public void Case024()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "test-no24-path";
        process.condition_operator = null;

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.25 プロパティ\"condition_path\"がnull/空文字以外、かつ、プロパティ\"condition_operator\"が空文字")]
    public void Case025()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "test-no25-path";
        process.condition_operator = "";

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.26 プロパティ\"condition_path\"がnull、かつ、プロパティ\"condition_operator\"がnull")]
    public void Case026()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = null;
        process.condition_operator = null;

        process.Check();
    }

    [Fact(DisplayName = "No.27 プロパティ\"condition_path\"が空文字、かつ、プロパティ\"condition_operator\"がnull")]
    public void Case027()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "";
        process.condition_operator = null;

        process.Check();
    }

    [Fact(DisplayName = "No.28 プロパティ\"condition_path\"がnull、かつ、プロパティ\"condition_operator\"が空文字")]
    public void Case028()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = null;
        process.condition_operator = "";

        process.Check();
    }

    [Fact(DisplayName = "No.29 プロパティ\"condition_path\"が空文字、かつ、プロパティ\"condition_operator\"が空文字")]
    public void Case029()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "";
        process.condition_operator = "";

        process.Check();
    }

    [Fact(DisplayName = "No.31 プロパティ\"condition_path\"がnull/空文字以外、かつ、プロパティ\"condition_operator\"が\"EQ(\"で始まらない")]
    public void Case031()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "test-no31-path";
        process.condition_operator = "operator-not-start-EQ)";

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.32 プロパティ\"condition_path\"がnull/空文字以外、かつ、プロパティ\"condition_operator\"が\")\"で終わらない")]
    public void Case032()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "test-no32-path";
        process.condition_operator = "EQ(test-no32-operator";

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.33 プロパティ\"condition_path\"がnull/空文字以外、かつ、プロパティ\"condition_operator\"が\"EQ(\"で始まり、\")\"で終わる")]
    public void Case033()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.condition_path = "test-no33-path";
        process.condition_operator = "EQ(ValidOperator)";

        process.Check();
    }

    [Fact(DisplayName = "No.34 プロパティ\"tasks\"の要素数が0")]
    public void Case034()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.tasks = [];

        Assert.ThrowsAny<Exception>(() => process.Check());
    }

    [Fact(DisplayName = "No.35 プロパティ\"tasks\"の要素数が2")]
    public void Case035()
    {
        var process = MyDesiredPropertiesCreater.CreateValidPostProcess();
        process.tasks = Enumerable.Range(0, 2)
            .Select(i => MyDesiredPropertiesCreater.CreateValidPostProcessTask($"dummy-task-{i}"))
            .ToList();

        process.Check();
    }
}
