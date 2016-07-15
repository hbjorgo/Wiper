using System.Collections.Generic;

namespace HeboTech.Wiper.IO
{
    public interface IFolderOperations
    {
        IEnumerable<string> EnumerateFolders(string path, string searchPattern, bool recursive);
        int DeleteFolders(IEnumerable<string> folders);
    }
}
