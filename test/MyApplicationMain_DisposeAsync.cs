using TICO.GAUDI.Commons;
using System.Runtime.CompilerServices;
using IotedgeV2ApplicationController;

namespace ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_DisposeAsync(ITestOutputHelper output)
{
    private readonly MyApplicationMain _app = new();

    private readonly ITestOutputHelper _output = output;

    // privateプロパティの取得
    [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "get_MyLogger")]
    static extern ILogger GetMyLogger(MyApplicationMain _app);

    // privateフィールドの取得
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_disposed")]
    static extern ref int GetDisposed(MyApplicationMain _app);

    [Fact(DisplayName = "Case001_リソース解放を1回実行する")]
    public async Task Case001()
    {
        await _app.DisposeAsync();
        var disposed = GetDisposed(_app);

        Assert.Equal(1, disposed);
    }

    [Fact(DisplayName = "Case002_リソース解放を2回実行する")]
    public async Task Case002()
    {
        var logger = GetMyLogger(_app);
        var stringWriter = new StringWriter();
        var logLevel = ILogger.LogLevel.TRACE;

        logger.SetOutputLogLevel(logLevel.ToString());
        logger.SetOutputTextWriter(stringWriter);

        await _app.DisposeAsync();
        stringWriter.GetStringBuilder().Clear(); //2回目のログのみを残すためにクリア
        await _app.DisposeAsync();

        var consoleOutput = stringWriter.GetStringBuilder().ToString().Trim();
        _output.WriteLine("-- StringWriter Output Log --");
        _output.WriteLine(consoleOutput);

        string containMessage = "Exit Method: Already disposed, skipping DisposeAsync";
        string notContainMessage = "End Method: DisposeAsync";

        Assert.Contains(containMessage, consoleOutput);
        Assert.DoesNotContain(notContainMessage,consoleOutput);
    }
}
