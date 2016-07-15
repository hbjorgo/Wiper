using System;
using System.Collections.Generic;
using System.IO;

namespace HeboTech.Wiper.IO
{
    public class FolderOperations : IFolderOperations
    {
        public IEnumerable<string> EnumerateFolders(string path, string searchPattern, bool recursive)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            try
            {
                return Directory.GetDirectories(path, searchPattern, searchOption);
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public int DeleteFolders(IEnumerable<string> folders)
        {
            int deletedFolders = 0;
            foreach (string folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    try
                    {
                        Directory.Delete(folder, true);
                        deletedFolders++;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return deletedFolders;
        }
    }
}
