using Dnet.Blazor.Components.Toast.Infrastructure.Enums;
using Dnet.Blazor.Components.Toast.Infrastructure.Models;

namespace Dnet.QdrantAdmin.Client.Infrastructure.HelperClasses;

public static class DnetToastConfig
{
    public static ToastConfig Get(ToastType toastType, string text, string title)
    {
        var toastConfig = new ToastConfig
        {
            Title = title,
            Text = text,
            HasBackdrop = false,
            HasTransparentBackdrop = false,
            ToastType = toastType,
            ToastPostion = ToastPostion.BottomRight,
            ExcutionTime = 5,
            ShowExcutionTime = true,
            Height = 80
        };

        return toastConfig;
    }
}