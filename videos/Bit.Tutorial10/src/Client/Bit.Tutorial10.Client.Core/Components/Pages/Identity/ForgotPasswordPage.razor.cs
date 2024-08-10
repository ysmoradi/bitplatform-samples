using Bit.Tutorial10.Client.Core.Controllers.Identity;
using Bit.Tutorial10.Shared.Dtos.Identity;

namespace Bit.Tutorial10.Client.Core.Components.Pages.Identity;

public partial class ForgotPasswordPage
{
    [AutoInject] IIdentityController identityController = default!;

    private bool isLoading;
    private string? forgotPasswordMessage;
    private BitColor forgotPasswordMessageColor;
    private SendResetPasswordEmailRequestDto forgotPasswordModel = new();

    private async Task DoSubmit()
    {
        if (isLoading) return;

        isLoading = true;
        forgotPasswordMessage = null;

        try
        {
            await identityController.SendResetPasswordEmail(forgotPasswordModel, CurrentCancellationToken);

            forgotPasswordMessageColor =  BitColor.Success;

            forgotPasswordMessage = Localizer[nameof(AppStrings.ResetPasswordLinkSentMessage)];
        }
        catch (KnownException e)
        {
            forgotPasswordMessageColor =  BitColor.Error;

            forgotPasswordMessage = e.Message;
        }
        finally
        {
            isLoading = false;
        }
    }
}
