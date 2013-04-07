using System.IO;

namespace Progressive.Scarlex13.Infrastructures
{
    internal class DxLibraryBase
    {
        private const string ImageFolder = "resources";

        protected string GetPath(string fileName)
        {
            return ImageFolder + Path.DirectorySeparatorChar + fileName;
        }
    }
}