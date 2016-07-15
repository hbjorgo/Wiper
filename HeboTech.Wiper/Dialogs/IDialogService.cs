namespace HeboTech.Wiper.Dialogs
{
    public interface IDialogService
    {
        bool ShowConfirmDialog(string message, string caption);
        void ShowDialog(string message, string caption);
    }
}
