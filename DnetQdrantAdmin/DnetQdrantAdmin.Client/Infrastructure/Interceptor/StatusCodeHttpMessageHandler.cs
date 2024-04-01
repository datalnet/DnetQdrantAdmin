using Dnet.QdrantAdmin.Application.Shared.Constants;
using Dnet.QdrantAdmin.Client.Infrastructure.ExceptionHandling;
using Dnet.QdrantAdmin.Client.Infrastructure.Models;
using Dnet.Blazor.Components.AdminLayout.Infrastructure.HelperClasses;
using Dnet.Blazor.Components.Spinner.Infrastructure.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Dnet.QdrantAdmin.Client.Infrastructure.Interceptor;

public class StatusCodeHttpMessageHandler : DelegatingHandler
{
    private readonly IJSRuntime _jSRuntime;
    private readonly ISpinnerService _spinnerService;
    private int authenticating = 0;

    public StatusCodeHttpMessageHandler(
        IJSRuntime jSRuntime,
        ISpinnerService spinnerService
        )
    {
        _jSRuntime = jSRuntime;
        _spinnerService = spinnerService;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        _spinnerService.Show();

        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _spinnerService.Hide();

            return response;
        }

        try
        {
            problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            _spinnerService.Hide();
        }
        catch (Exception)
        {
            _spinnerService.Hide();
        }

        _spinnerService.Hide();
        throw new CustomReponseException("", problemDetails);
    }
}