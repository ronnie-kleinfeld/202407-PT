namespace Comtec.BE.IO.File {
    public class Writer {
        // members
        public StreamWriter StreamWriter {
            get; set;
        }
        public string Directory {
            get; set;
        }
        public virtual string File {
            get; set;
        }

        // properties
        public string Path => System.IO.Path.Combine(Directory, File);
        protected string NewLine => StreamWriter.NewLine;

        // class
        public Writer() : base() {
        }
        public Writer(string directory, string file) : base() {
            Directory = directory;
            File = file;
        }

        // directory
        protected void CreateDirectory() {
            System.IO.Directory.CreateDirectory(Directory);
        }
        protected void CreateDirectory(string directory) {
            System.IO.Directory.CreateDirectory(directory);
        }

        // file methods
        protected void OpenFile() {
            CreateDirectory();
            StreamWriter = new StreamWriter(Path);
        }
        protected void FlushFile() {
            StreamWriter.Flush();
        }
        protected void CloseFile() {
            try {
                StreamWriter.Close();
            } catch {
            }
        }
        protected void DeleteFile() {
            try {
                System.IO.File.Delete(Path);
            } catch {
            }
        }

        // write methods
        protected void Write(string str) {
            StreamWriter.Write(str);
        }
        protected void Write(string str, params object[] args) {
            StreamWriter.Write(str, args);
        }
        protected void WriteLine(string str) {
            StreamWriter.WriteLine(str);
        }
        protected void WriteLine(string str, params object[] args) {
            StreamWriter.WriteLine(str, args);
        }
    }
}