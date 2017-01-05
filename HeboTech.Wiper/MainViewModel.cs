using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeboTech.Wiper.Dialogs;
using HeboTech.Wiper.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HeboTech.Wiper
{
    public class MainViewModel : ViewModelBase
    {
        private IFolderOperations folderOperations;
        private IDialogService dialogService;
        private IFolderBrowserDialogService folderBrowserService;
        private ISettings settingsProvider;

        public MainViewModel(
            IFolderOperations folderOperations,
            IDialogService dialogService,
            IFolderBrowserDialogService folderBrowserService,
            ISettings settingsProvider)
        {
            if (folderOperations == null)
                throw new ArgumentNullException(nameof(folderOperations));
            this.folderOperations = folderOperations;

            if (dialogService == null)
                throw new ArgumentNullException(nameof(dialogService));
            this.dialogService = dialogService;

            if (folderBrowserService == null)
                throw new ArgumentNullException(nameof(folderBrowserService));
            this.folderBrowserService = folderBrowserService;

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));
            this.settingsProvider = settingsProvider;

            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FolderToDelete)
                || e.PropertyName == nameof(RootFolder)
                || e.PropertyName == nameof(IsRecursive))
            {
                CanDelete = false;
            }
        }

        private void Browse()
        {
            if (folderBrowserService.ShowDialog(RootFolder))
            {
                RootFolder = folderBrowserService.SelectedFolder;
            }
        }

        private void FindFolders()
        {
            IEnumerable<string> foldersToDelete = Parse(folderToDelete);
            Folders = EnumerateFolders(rootFolder, foldersToDelete, isRecursive);
            CanDelete = true;
        }

        private void Delete()
        {
            IEnumerable<string> foldersToDelete = Parse(folderToDelete);

            if (dialogService.ShowConfirmDialog(
                string.Format("Do you want to delete folder(s) '{0}' in '{1}'?",
                    string.Join(", ", foldersToDelete.Select(x => x)),
                    rootFolder),
                "Delete folder(s)?"))
            {
                IEnumerable<string> folders = EnumerateFolders(rootFolder, foldersToDelete, isRecursive);
                int numberOfDeletedFolders = DeleteFolders(folders);
                dialogService.ShowDialog(string.Format("{0} of {1} folder(s) deleted.", numberOfDeletedFolders, folders.Count()), "Folder(s) deleted");
            }
        }

        private bool canDelete = false;
        public bool CanDelete
        {
            get { return canDelete; }
            set
            {
                if (canDelete != value)
                {
                    canDelete = value;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                        deleteCommand.RaiseCanExecuteChanged();
                    }));
                }
            }
        }

        private IEnumerable<string> Parse(string input)
        {
            if (input == null)
                return new List<string>();
            return input.Split('|');
        }

        private IEnumerable<string> EnumerateFolders(string path, IEnumerable<string> foldersToDelete, bool recursive)
        {
            List<string> allFolders = new List<string>();
            foreach (string folderToDelete in foldersToDelete)
                allFolders.AddRange(folderOperations.EnumerateFolders(path, folderToDelete, recursive));
            return allFolders as IEnumerable<string>;
        }

        private int DeleteFolders(IEnumerable<string> folders)
        {
            return folderOperations.DeleteFolders(folders);
        }

        private bool isRecursive = true;
        public bool IsRecursive
        {
            get { return isRecursive; }
            set
            {
                if (isRecursive != value)
                {
                    isRecursive = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string folderToDelete;
        public string FolderToDelete
        {
            get { return folderToDelete; }
            set
            {
                if (folderToDelete != value)
                {
                    folderToDelete = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string rootFolder;
        public string RootFolder
        {
            get { return rootFolder; }
            set
            {
                if (rootFolder != value)
                {
                    rootFolder = value;
                    RaisePropertyChanged();
                }
            }
        }

        private IEnumerable<string> folders;
        public IEnumerable<string> Folders
        {
            get { return folders; }
            private set
            {
                folders = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                return browseCommand ?? (browseCommand = new RelayCommand(Browse));
            }
        }

        private AsyncCommand findFoldersCommand;
        public IAsyncCommand FindFoldersCommand
        {
            get
            {
                return findFoldersCommand ?? (findFoldersCommand = new AsyncCommand(async _ => { await Task.Factory.StartNew(() => { FindFolders(); }); }));
            }
        }

        private AsyncCommand deleteCommand;
        public IAsyncCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new AsyncCommand(async _ => { await Task.Factory.StartNew(() => { Delete(); }); }, _ => (canDelete && Folders.Count() > 0)));
            }
        }

        private RelayCommand saveSettingsCommand;
        public ICommand SaveSettingsCommand
        {
            get
            {
                return saveSettingsCommand ?? (saveSettingsCommand = new RelayCommand(SaveSettings));
            }
        }

        private RelayCommand loadSettingsCommnad;
        public ICommand LoadSettingsCommand
        {
            get
            {
                return loadSettingsCommnad ?? (loadSettingsCommnad = new RelayCommand(LoadSettings));
            }
        }

        private void SaveSettings()
        {
            settingsProvider.SetSetting(nameof(FolderToDelete), FolderToDelete);
            settingsProvider.SetSetting(nameof(RootFolder), RootFolder);
            settingsProvider.SetSetting(nameof(IsRecursive), IsRecursive);
            settingsProvider.Save();
        }

        private void LoadSettings()
        {
            FolderToDelete = settingsProvider.GetSetting<string>(nameof(FolderToDelete));
            RootFolder = settingsProvider.GetSetting<string>(nameof(RootFolder));
            IsRecursive = settingsProvider.GetSetting<bool>(nameof(IsRecursive));
        }
    }
}
