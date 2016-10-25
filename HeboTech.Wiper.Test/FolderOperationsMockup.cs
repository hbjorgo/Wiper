using HeboTech.Wiper.IO;
using System.Collections.Generic;
using System.Linq;

namespace HeboTech.Wiper.Test
{
    public class FolderOperationsMockup : IFolderOperations
    {
        IEnumerable<string> foldersToReturn = new List<string>();

        public FolderOperationsMockup(IEnumerable<string> foldersToReturn)
        {
            if (foldersToReturn != null)
                this.foldersToReturn = foldersToReturn;
        }

        public FolderOperationsMockup(params string[] foldersToReturn)
        {
            if (foldersToReturn != null)
                this.foldersToReturn = foldersToReturn;
        }

        public int DeleteFolders(IEnumerable<string> folders)
        {
            return foldersToReturn.Count();
        }

        public IEnumerable<string> EnumerateFolders(string path, string searchPattern, bool recursive)
        {
            return foldersToReturn;
        }
    }
}
