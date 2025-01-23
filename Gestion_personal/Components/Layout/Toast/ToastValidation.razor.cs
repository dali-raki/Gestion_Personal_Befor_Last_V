using Gestion_personal.Components.Models.Toast;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Toast;

public partial class ToastValidation
{
    [Parameter] public ToastType Type { get; set; } = ToastType.Success;
    [Parameter] public string Title { get; set; } = "Success!";
    [Parameter] public string Message { get; set; } = "Your operation was successful.";
    [Parameter] public bool IsVisible { get; set; } = false;
    [Parameter] public EventCallback OnClose { get; set; }

    private async Task CloseToastAfterDelay()
    {
        await Task.Delay(3000);
        await CloseToast();
    }

    private async Task CloseToast()
    {
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }
    }

    protected override void OnParametersSet()
    {
        if (IsVisible)
        {
            _ = CloseToastAfterDelay();
        }
    }

    private string GetToastClass()
    {
        return Type switch
        {
            ToastType.Success => "toast-success",
            ToastType.Warning => "toast-warning",
            ToastType.Danger => "toast-danger",
            _ => "toast-success"
        };
    }
}