using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.QdrantAdmin.Client.Infrastructure.Services
{
    public class AppInterop
    {

        private readonly IJSRuntime _jsRuntime;

        public AppInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<object> PrettyPrintJsonData(ElementReference jsonContainer, string jsonData)
        {
            return _jsRuntime.InvokeAsync<object>("qdrantapp.prettyPrintJsonData", jsonContainer, jsonData);
        }
    }
}
