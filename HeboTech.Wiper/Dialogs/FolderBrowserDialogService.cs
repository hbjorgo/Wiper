namespace HeboTech.Wiper.Dialogs
{
    public class FolderBrowserDialogService : IFolderBrowserDialogService
    {
        public string SelectedFolder { get; private set; }

        public bool ShowDialog(string initialFolder = null)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = initialFolder;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectedFolder = fbd.SelectedPath;
                return true;
            }
            return false;
        }
    }
}
