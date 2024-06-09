﻿using Bit.Tutorial08.Shared.Dtos.Identity;

namespace Bit.Tutorial08.Client.Core.Components.Pages.Identity;

public partial class SignInPage
{
    private bool isLoading;
    private string? signInMessage;
    private BitSeverity signInMessageType;
    private SignInRequestDto signInModel = new();

    [SupplyParameterFromQuery(Name = "redirect-url"), Parameter] public string? RedirectUrl { get; set; }
    [SupplyParameterFromQuery(Name = "email"), Parameter] public string? Email { get; set; }

    protected override async Task OnParamsSetAsync()
    {
        await base.OnParamsSetAsync();

        if (string.IsNullOrEmpty(signInModel.UserName))
        {
            signInModel.UserName = Email;
        }
    }

    private async Task DoSignIn()
    {
        if (isLoading) return;

        isLoading = true;
        signInMessage = null;

        try
        {
            await AuthenticationManager.SignIn(signInModel, CurrentCancellationToken);

            NavigationManager.NavigateTo(RedirectUrl ?? "/");
        }
        catch (KnownException e)
        {
            signInMessageType = BitSeverity.Error;

            signInMessage = e.Message;
        }
        finally
        {
            isLoading = false;
        }
    }
}

