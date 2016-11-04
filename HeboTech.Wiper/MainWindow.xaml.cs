using HeboTech.Wiper.Dialogs;
using HeboTech.Wiper.IO;
using System.Windows;

namespace HeboTech.Wiper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(
                new FolderOperations(),
                new MessageBoxService(),
                new FolderBrowserDialogService(),
                new ApplicationSettings());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel vm = (MainViewModel)DataContext;
            if (vm.SaveSettingsCommand.CanExecute(null))
                vm.SaveSettingsCommand.Execute(null);
        }
    }
}
