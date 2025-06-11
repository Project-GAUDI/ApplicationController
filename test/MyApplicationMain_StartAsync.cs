namespace IotedgeV2ApplicationController.Test;

public class MyApplicationMain_StartAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.5 正常終了する", Skip = "ModuleClientのインスタンス生成がCommonsによるライフサイクルで動作するため、現構成での実装は不可。自動テストから除外")]
    public void Case005()
    {
        throw new NotImplementedException();
    }
}
