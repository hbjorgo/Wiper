using HeboTech.Wiper.Dialogs;

namespace HeboTech.Wiper.Test
{
    public class FolderBrowserDialogServiceMockup : IFolderBrowserDialogService
    {
        public FolderBrowserDialogServiceMockup(string returnFolder)
        {
            this.SelectedFolder = returnFolder;
        }

        public string SelectedFolder
        {
            get; private set;
        }

        public bool ShowDialog(string initialFolder = null)
        {
            return true;
        }
    }
}
