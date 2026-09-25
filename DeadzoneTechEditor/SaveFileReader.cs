using System;

namespace DeadzoneTechEditor
{
    public class SaveFileReader
    {
        public byte[] Read(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
