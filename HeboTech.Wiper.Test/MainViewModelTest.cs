using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeboTech.Wiper.Test
{
    [TestClass]
    public class MainViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForFolderOperationsShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                null,
                new DialogServiceMockup(true),
                new FolderBrowserDialogServiceMockup(null),
                new SettingsMockup(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForDialogServicesShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new FolderOperationsMockup(null),
                null,
                new FolderBrowserDialogServiceMockup(null),
                new SettingsMockup(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForFolderBrowserDialogServiceShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new FolderOperationsMockup(null),
                new DialogServiceMockup(true),
                null,
                new SettingsMockup(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForSettingsProviderShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new FolderOperationsMockup(null),
                new DialogServiceMockup(true),
                new FolderBrowserDialogServiceMockup(null),
                null);
        }

        [TestMethod]
        public void FindFoldersCommandShouldPopulateFoldersPropertyTest()
        {
            MainViewModel mvm = new MainViewModel(
                new FolderOperationsMockup("Folder1", "Folder2"),
                new DialogServiceMockup(true),
                new FolderBrowserDialogServiceMockup(null),
                new SettingsMockup(new Dictionary<string, object>() {
                    { "FolderToDelete", "" },
                    { "RootFolder", null },
                    { "IsRecursive", false }
                }));
            mvm.FindFoldersCommand.Execute(null);

            Assert.AreEqual(2, mvm.Folders.Count());
            Assert.AreEqual("Folder1", mvm.Folders.ElementAt(0));
            Assert.AreEqual("Folder2", mvm.Folders.ElementAt(1));
        }

        [TestMethod]
        public void AbortDeleteCommandShouldShowDialogWithCorrectText()
        {
            DialogServiceMockup dsm = new DialogServiceMockup(false);
            MainViewModel mvm = new MainViewModel(
                new FolderOperationsMockup("Folder1", "Folder2"),
                dsm,
                new FolderBrowserDialogServiceMockup(null),
                new SettingsMockup(new Dictionary<string, object>() {
                    { "FolderToDelete", "" },
                    { "RootFolder", null },
                    { "IsRecursive", false }
                }));
            mvm.FindFoldersCommand.Execute(null);
            mvm.DeleteCommand.Execute(null);

            Assert.AreEqual("Do you want to delete folder(s) '' in ''?", dsm.Message);
        }
    }
}
