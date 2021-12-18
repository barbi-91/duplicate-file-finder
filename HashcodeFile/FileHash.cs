using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HashcodeFile
{
    class FileHash
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

        public FileHash(string fName, string fHash, string fPath, long fSize)
        {
            FileNameP = fName;
            HashP = fHash;
            PathP = fPath;
            FileSizeP = fSize;
        }

    }
}
