using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IotedgeV2ApplicationController.Test;

[Collection(nameof(CollectionAttribute))]
public class HttpApplicationCleint_SetParam
{
    private readonly HttpApplicationClient _applicationClient = new ();


    [Fact(DisplayName = "No.104 引数\"key\"が\"url\"/\"timeout\"以外")]
    public void Case104()
    {
        Assert.ThrowsAny<Exception>(() => _applicationClient.SetParam("invalidKey", "value"));
    }
}
