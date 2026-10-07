namespace MinhaBiblioteca.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        if (Shell.Current != null)
        {
            return Shell.Current.DisplayAlert(title, message, cancel);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Sim", string cancel = "Não")
    {
        if (Shell.Current != null)
        {
            return Shell.Current.DisplayAlert(title, message, accept, cancel);
        }
        return Task.FromResult(false);
    }
}
