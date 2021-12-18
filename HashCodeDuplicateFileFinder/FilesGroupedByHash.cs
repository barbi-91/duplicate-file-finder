using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeDuplicateFileFinder
{
    class FilesGroupedByHash
    {
        #region property
        public string HashGP { get; set; }
        public List<FileHashes> HeshesGroupListGP { get; set; }
        #endregion


        
        public FilesGroupedByHash(string hash, List<FileHashes> fileinfo)
        {
            HashGP = hash;
            HeshesGroupListGP = fileinfo;
        }
    }
}
