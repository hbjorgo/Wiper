using System.Windows;

namespace HeboTech.Wiper.Dialogs
{
    public class MessageBoxService : IDialogService
    {
        public bool ShowConfirmDialog(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public void ShowDialog(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }
    }
}
