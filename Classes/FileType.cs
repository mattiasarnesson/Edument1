using System;

namespace Edument1
{
    public class FileType
    {

        private String name;
        private String path;
        private String extension;
        private String img;
        private long size;
        private bool dir; //

        public FileType()
        {
        }

        public FileType(string name, string path, string extension, string img, long size, bool dir)
        {
            this.Name = name;
            this.Path = path;
            this.Extension = extension;
            this.Img = img;
            this.Size = size;
            this.Dir = dir;
        }

        public string Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }
        public string Extension { get => extension; set => extension = value; }
        public string Img { get => img; set => img = value; }
        public long Size { get => size; set => size = value; }
        public bool Dir { get => dir; set => dir = value; }
    }
}