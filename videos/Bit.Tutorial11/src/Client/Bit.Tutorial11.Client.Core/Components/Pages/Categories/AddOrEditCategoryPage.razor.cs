﻿using Bit.Tutorial11.Client.Core.Controllers.Categories;
using Bit.Tutorial11.Shared.Dtos.Categories;

namespace Bit.Tutorial11.Client.Core.Components.Pages.Categories;

[Authorize]
public partial class AddOrEditCategoryPage
{
    [AutoInject] ICategoryController categoryController = default!;

    [Parameter] public int? Id { get; set; }

    private bool isLoading;
    private bool isSaving;
    private string? saveMessage;
    private bool isColorPickerOpen;
    private BitColor saveMessageType;
    private CategoryDto category = new();

    protected override async Task OnInitAsync()
    {
        await LoadCategory();
    }

    private async Task LoadCategory()
    {
        if (Id is null) return;

        isLoading = true;

        try
        {
            category = await categoryController.Get(Id.Value, CurrentCancellationToken);
        }
        finally
        {
            isLoading = false;
        }
    }

    private void SetCategoryColor(string color)
    {
        category.Color = color;
    }

    private void ToggleColorPicker()
    {
        isColorPickerOpen = !isColorPickerOpen;
    }

    private async Task Save()
    {
        if (isSaving) return;

        isSaving = true;

        try
        {
            if (category.Id == 0)
            {
                await categoryController.Create(category, CurrentCancellationToken);
            }
            else
            {
                await categoryController.Update(category, CurrentCancellationToken);
            }

            NavigationManager.NavigateTo("categories");
        }
        catch (ResourceValidationException e)
        {
            saveMessageType =  BitColor.Error;

            saveMessage = string.Join(Environment.NewLine, e.Payload.Details.SelectMany(d => d.Errors).Select(e => e.Message));
        }
        catch (KnownException e)
        {
            saveMessage = e.Message;
            saveMessageType =  BitColor.Error;
        }
        finally
        {
            isSaving = false;
        }
    }
}
