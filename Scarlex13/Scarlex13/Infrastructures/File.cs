using System.Runtime.InteropServices;
using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13.Infrastructures
{
    class File : DxLibraryBase
    {
        [DllImport("DxLib.dll")]
        extern static int dx_FileRead_read(byte[] buffer, int readSize, int fileHandle);

        public string GetStages()
        {
            int size = DX.FileRead_size(GetPath("stage.txt"));
            int handle = DX.FileRead_open(GetPath("stage.txt"));
            var data = new byte[size];
            if (dx_FileRead_read(data, size, handle) < 0)
                throw new Exception();
            DX.FileRead_close(handle);
            return Encoding.UTF8.GetString(data);
        }
    }
}
