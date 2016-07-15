namespace HeboTech.Wiper.Dialogs
{
    public interface IFolderBrowserDialogService
    {
        bool ShowDialog(string initialFolder = null);

        string SelectedFolder { get; }
    }
}
