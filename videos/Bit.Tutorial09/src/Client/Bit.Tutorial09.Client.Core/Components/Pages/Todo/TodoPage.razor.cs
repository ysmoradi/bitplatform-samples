﻿using Bit.Tutorial09.Client.Core.Controllers.Todo;
using Bit.Tutorial09.Shared.Dtos.Todo;

namespace Bit.Tutorial09.Client.Core.Components.Pages.Todo;

[Authorize]
// [Authorize(Policy = "AdminsOnly")]
public partial class TodoPage : IDisposable
{
    [AutoInject] Keyboard keyboard = default!;
    [AutoInject] ITodoItemController todoItemController = default!;
    [AutoInject] private IAuthorizationService authorizationService = default!;

    private bool isAdding;
    private bool isLoading;
    private string? searchText;
    private string? selectedSort;
    private string? selectedFilter;
    private string? underEditTodoItemTitle;
    private string newTodoTitle = string.Empty;
    private ConfirmMessageBox confirmMessageBox = default!;
    private IList<TodoItemDto> allTodoItems = default!;
    private IList<TodoItemDto> viewTodoItems = default!;
    private List<BitDropdownItem<string>> sortItems = [];
    private BitSearchBox searchBox = default!;

    protected override async Task OnInitAsync()
    {
        _ = keyboard.Add(ButilKeyCodes.KeyF, () => _ = searchBox.FocusInput(), ButilModifiers.Ctrl);

        selectedFilter = nameof(AppStrings.All);
        selectedSort = nameof(AppStrings.Alphabetical);

        sortItems =
        [
            new BitDropdownItem<string> { Text = Localizer[nameof(AppStrings.Alphabetical)], Value = nameof(AppStrings.Alphabetical) },
            new BitDropdownItem<string> { Text = Localizer[nameof(AppStrings.Date)], Value = nameof(AppStrings.Date) }
        ];

        await LoadTodoItems();

        await base.OnInitAsync();
    }

    private async Task LoadTodoItems()
    {
        isLoading = true;

        try
        {
            allTodoItems = await (await todoItemController.Get(CurrentCancellationToken)).ToListAsync(CurrentCancellationToken);

            FilterViewTodoItems();
        }
        finally
        {
            isLoading = false;
        }
    }

    private void FilterViewTodoItems()
    {
        viewTodoItems = allTodoItems
            .Where(t => TodoItemIsVisible(t))
            .OrderByIf(selectedSort == nameof(AppStrings.Alphabetical), t => t.Title!)
            .OrderByIf(selectedSort == nameof(AppStrings.Date), t => t.Date!)
            .ToList();
    }

    private bool TodoItemIsVisible(TodoItemDto todoItem)
    {
        var condition1 = string.IsNullOrWhiteSpace(searchText) || todoItem.Title!.Contains(searchText!, StringComparison.OrdinalIgnoreCase);

        var condition2 = selectedFilter == nameof(AppStrings.Active) ? todoItem.IsDone is false
            : selectedFilter == nameof(AppStrings.Completed) ? todoItem.IsDone
            : true;

        return condition1 && condition2;
    }

    private async Task ToggleIsDone(TodoItemDto todoItem)
    {
        todoItem.IsDone = !todoItem.IsDone;

        await UpdateTodoItem(todoItem);
    }

    private void SearchTodoItems(string text)
    {
        searchText = text;

        FilterViewTodoItems();
    }

    private void SortTodoItems(BitDropdownItem<string> sort)
    {
        selectedSort = sort.Value;

        FilterViewTodoItems();
    }

    private void FilterTodoItems(string filter)
    {
        selectedFilter = filter;

        FilterViewTodoItems();
    }

    private void ToggleEditMode(TodoItemDto todoItem)
    {
        underEditTodoItemTitle = todoItem.Title;
        todoItem.IsInEditMode = !todoItem.IsInEditMode;
    }

    private async Task AddTodoItem()
    {
        if (isAdding) return;

        isAdding = true;

        try
        {
            var addedTodoItem = await todoItemController.Create(new() { Title = newTodoTitle }, CurrentCancellationToken);

            allTodoItems.Add(addedTodoItem!);

            if (TodoItemIsVisible(addedTodoItem!))
            {
                viewTodoItems.Add(addedTodoItem!);
            }

            newTodoTitle = "";
        }
        finally
        {
            isAdding = false;
        }
    }

    private async Task DeleteTodoItem(TodoItemDto todoItem)
    {
        if ((await authorizationService.AuthorizeAsync((await AuthenticationStateTask).User, "AdminsOnly")).Succeeded is false)
            throw new UnauthorizedException(nameof(AppStrings.YouAreNotAdmin)); // Check AdminsOnly via IAuthorizationService programatically

        if (isLoading) return;

        try
        {
            var confirmed = await confirmMessageBox.Show(Localizer.GetString(nameof(AppStrings.AreYouSureWannaDelete), todoItem.Title!),
                                                     Localizer[nameof(AppStrings.DeleteTodoItem)]);

            if (confirmed)
            {
                isLoading = true;

                StateHasChanged();

                await todoItemController.Delete(todoItem.Id, CurrentCancellationToken);

                allTodoItems.Remove(todoItem);

                viewTodoItems.Remove(todoItem);
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task SaveTodoItem(TodoItemDto todoItem)
    {
        if (isLoading) return;

        isLoading = true;

        try
        {
            todoItem.Title = underEditTodoItemTitle;

            await UpdateTodoItem(todoItem);
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task UpdateTodoItem(TodoItemDto todoItem)
    {
        (await todoItemController.Update(todoItem, CurrentCancellationToken)).Patch(todoItem);

        todoItem.IsInEditMode = false;

        if (TodoItemIsVisible(todoItem) is false)
        {
            viewTodoItems.Remove(todoItem);
        }
    }

    public override void Dispose()
    {
        keyboard.DisposeAsync();
        base.Dispose();
    }
}
