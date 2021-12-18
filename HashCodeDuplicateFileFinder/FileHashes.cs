using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCodeDuplicateFileFinder
{
    class FileHashes
    {

        //property
        public string FileNameP
        {
            get;
            set;
        }

        public string HashP
        {
            get;
            set;
        }

        public string PathP
        {
            get;
            set;
        }

        public long FileSizeP
        {
            get;
            set;
        }

        public DateTime LastModifiedP
        {
            get;
            set;
        }

        public bool IsChecked
        {
            get;
            set;
        }

        public FileHashes(string fName, string fHash, string fPath, long fSize, DateTime lastModified)
        {
            FileNameP = fName;
            HashP = fHash;
            PathP = fPath;
            FileSizeP = fSize;
            LastModifiedP = lastModified;
        }
    }
}
