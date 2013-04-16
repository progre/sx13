using System.Runtime.InteropServices;
using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Progressive.Scarlex13.Infrastructures
{
    class File : DxLibraryBase
    {
        [DllImport("DxLib.dll")]
        extern static int dx_FileRead_read(byte[] buffer, int readSize, int fileHandle);

        public string ConfigurationFilePath
        {
            get
            {
                var fullName = Assembly.GetExecutingAssembly().Location;
                return Directory.GetParent(fullName)
                    + Path.DirectorySeparatorChar.ToString()
                    + Path.GetFileNameWithoutExtension(fullName)
                    + ".cfg";
            }
        }

        public string GetNormalStages()
        {
            return GetStages("stage.txt");
        }

        public string GetExtraStages()
        {
            return GetStages("stage2.txt");
        }

        private string GetStages(string fileName)
        {
            int size = DX.FileRead_size(GetPath(fileName));
            int handle = DX.FileRead_open(GetPath(fileName));
            var data = new byte[size];
            if (dx_FileRead_read(data, size, handle) < 0)
                throw new Exception();
            DX.FileRead_close(handle);
            return Encoding.UTF8.GetString(data);
        }

        public void SaveStages(string data)
        {
            System.IO.File.WriteAllText(GetPath("stage2.txt"), data);
        }

        public void SaveExFlag()
        {
            System.IO.File.WriteAllText(ConfigurationFilePath, "(^^)");
        }

        public bool CheckExFlag()
        {
            return System.IO.File.Exists(ConfigurationFilePath);
        }
    }
}
