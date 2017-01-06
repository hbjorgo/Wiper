using HeboTech.Wiper.Dialogs;
using HeboTech.Wiper.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
                new Mock<IDialogService>().Object,
                new Mock<IFolderBrowserDialogService>().Object,
                new Mock<ISettings>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForDialogServicesShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new Mock<IFolderOperations>().Object,
                null,
                new Mock<IFolderBrowserDialogService>().Object,
                new Mock<ISettings>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForFolderBrowserDialogServiceShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new Mock<IFolderOperations>().Object,
                new Mock<IDialogService>().Object,
                null,
                new Mock<ISettings>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullForSettingsProviderShouldThrowExceptionTest()
        {
            MainViewModel mvm = new MainViewModel(
                new Mock<IFolderOperations>().Object,
                new Mock<IDialogService>().Object,
                new Mock<IFolderBrowserDialogService>().Object,
                null);
        }

        [TestMethod]
        public void FindFoldersCommandShouldPopulateFoldersPropertyTest()
        {
            var folderOperationsMock = new Mock<IFolderOperations>();
            folderOperationsMock.Setup(x => x.EnumerateFolders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new List<string> { "Folder1", "Folder2" });

            MainViewModel mvm = new MainViewModel(
                folderOperationsMock.Object,
                new Mock<IDialogService>().Object,
                new Mock<IFolderBrowserDialogService>().Object,
                new Mock<ISettings>().Object);
            mvm.FolderToDelete = "";

            mvm.FindFoldersCommand.Execute(null);

            SpinWait(new Func<bool>(() => { return mvm.FindFoldersCommand.Running; }));

            Assert.AreEqual(2, mvm.Folders.Count());
            Assert.AreEqual("Folder1", mvm.Folders.ElementAt(0));
            Assert.AreEqual("Folder2", mvm.Folders.ElementAt(1));
        }

        [TestMethod]
        public void DeleteCommandShouldShowDialogWithCorrectText()
        {
            var folderOperationsMock = new Mock<IFolderOperations>();
            folderOperationsMock.Setup(x => x.EnumerateFolders(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new List<string> { "Folder1", "Folder2" });

            string dialogMessageResult = string.Empty;
            string dialogCaptionResult = string.Empty;
            var dialogServiceMock = new Mock<IDialogService>();
            dialogServiceMock.Setup(x => x.ShowConfirmDialog(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((msg,cap) =>
            {
                dialogMessageResult = msg;
                dialogCaptionResult = cap;
            });

            MainViewModel mvm = new MainViewModel(
                folderOperationsMock.Object,
                dialogServiceMock.Object,
                new Mock<IFolderBrowserDialogService>().Object,
                new Mock<ISettings>().Object);

            mvm.FindFoldersCommand.Execute(null);
            SpinWait(new Func<bool>(() => { return mvm.FindFoldersCommand.Running; }));
            mvm.DeleteCommand.Execute(null);
            SpinWait(new Func<bool>(() => { return mvm.DeleteCommand.Running; }));

            
            Assert.AreEqual("Do you want to delete folder(s) '' in ''?", dialogMessageResult);
            Assert.AreEqual("Delete folder(s)?", dialogCaptionResult);
        }

        private void SpinWait(Func<bool> evaluate, int timeoutMs = 1000)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (evaluate.Invoke())
            {
                if (sw.ElapsedMilliseconds > timeoutMs)
                    return;
                Thread.Yield();
            }
        }
    }
}
