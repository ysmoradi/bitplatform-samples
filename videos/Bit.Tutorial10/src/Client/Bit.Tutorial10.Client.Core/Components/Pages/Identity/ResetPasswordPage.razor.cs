﻿using Bit.Tutorial10.Client.Core.Controllers.Identity;
using Bit.Tutorial10.Shared.Dtos.Identity;

namespace Bit.Tutorial10.Client.Core.Components.Pages.Identity;

public partial class ResetPasswordPage
{
    [AutoInject] IIdentityController identityController = default!;

    private bool isLoading;
    private bool passwordChanged;
    private string? resetPasswordMessage;
    private BitColor resetPasswordMessageColor;
    private ResetPasswordRequestDto resetPasswordModel = new();

    [Parameter, SupplyParameterFromQuery] public string? Email { get; set; }

    [Parameter, SupplyParameterFromQuery] public string? Token { get; set; }

    protected override async Task OnInitAsync()
    {
        resetPasswordModel.Email = Email;
        resetPasswordModel.Token = Token;

        await base.OnInitAsync();
    }

    private void RedirectToSignIn()
    {
        NavigationManager.NavigateTo($"/sign-in?email={Email}");
    }

    private async Task DoSubmit()
    {
        if (isLoading) return;

        isLoading = true;
        resetPasswordMessage = null;

        try
        {
            await identityController.ResetPassword(resetPasswordModel, CurrentCancellationToken);

            resetPasswordMessageColor =  BitColor.Success;

            resetPasswordMessage = Localizer[nameof(AppStrings.PasswordChangedSuccessfullyMessage)];

            passwordChanged = true;
        }
        catch (KnownException e)
        {
            resetPasswordMessageColor =  BitColor.Error;

            resetPasswordMessage = e.Message;
        }
        finally
        {
            isLoading = false;
        }
    }
}
