using HeboTech.Wiper.Dialogs;

namespace HeboTech.Wiper.Test
{
    public class DialogServiceMockup : IDialogService
    {
        private bool returnValue;

        public DialogServiceMockup(bool returnValue)
        {
            this.returnValue = returnValue;
        }

        public bool ShowConfirmDialog(string message, string caption)
        {
            this.Message = message;
            this.Caption = caption;
            return returnValue;
        }

        public void ShowDialog(string message, string caption)
        {
            this.Message = message;
            this.Caption = caption;
        }

        public string Message { get; private set; }
        public string Caption { get; private set; }
    }
}
