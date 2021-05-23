using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace whites
{
    class DefaultFile
    {
        string nameWithoutUUID;
        FileInfo file;
        bool needToDelete;

        public FileInfo File { get => file; set => file = value; }
        public string NameWithoutUUID { get => nameWithoutUUID; set => nameWithoutUUID = value; }
        public bool NeedToDelete { get => needToDelete; set => needToDelete = value; }

        public DefaultFile(FileInfo File)
        {
            this.File = File;
            Regex regex = new Regex(@" \w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
            NameWithoutUUID = regex.Replace(this.File.Name, "");
        }

        public string FileRead()
        {
            string path = file.FullName;
            string txt;
            using (FileStream fstream = System.IO.File.OpenRead(path))
            {
                byte[] vs = new byte[fstream.Length];
                fstream.Read(vs, 0, vs.Length);
                txt = System.Text.Encoding.Default.GetString(vs);
            }
            return txt;
        }
        public void FileRename()
        {
            if (NeedToDelete == false)
            {
                int p = 1;
                while (true)
                {
                    string filename;
                    string[] FileAndItsExtension = NameWithoutUUID.Split('.');
                    filename = FileAndItsExtension[0];
                    for (int j = 1; j < FileAndItsExtension.Length - 2; j++)
                    {
                        filename = filename + " " + FileAndItsExtension[j];
                    }
                    if(System.IO.File.Exists(File.Directory+ "\\" + NameWithoutUUID)==false)
                    {
                        File.MoveTo(File.Directory + "\\" + NameWithoutUUID);
                        break;
                    }
                    filename = filename + " (" + p + ")." + FileAndItsExtension[FileAndItsExtension.Length - 1];
                    if (System.IO.File.Exists(File.Directory + "\\" + filename))
                        p++;
                    else
                    {
                        File.MoveTo(File.Directory + "\\" + filename);
                        break;
                    }
                }
            }
            else
                file.Delete();
        }
    }
    class MainDirectory
    {
        DirectoryInfo directory;
        DefaultFile[] files;
        DefaultDirectory[] directories;
        public MainDirectory(DirectoryInfo Directory)
        {
            this.Directory = Directory;
            files = GetFiles(Directory);
            directories = GetDirectories(Directory);
        }
        public DefaultFile[] GetFiles(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            DefaultFile[] defaultFiles = new DefaultFile[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                defaultFiles[i] = new DefaultFile(files[i]);
            }
            return defaultFiles;
        }
        public DefaultDirectory[] GetDirectories(DirectoryInfo directory)
        {
            DirectoryInfo[] directories = directory.GetDirectories();
            DefaultDirectory[] defaultdirectories = new DefaultDirectory[directories.Length];
            for (int i = 0; i < directories.Length; i++)
            {
                defaultdirectories[i] = new DefaultDirectory(directories[i]);
            }
            return defaultdirectories;
        }

        public DirectoryInfo Directory { get => directory; set => directory = value; }

        public void Start()
        {
            for (int i = 1; i < directories.Length; i++)
            {
                if (directories[i].NameWithoutUUID == directories[i-1].NameWithoutUUID)
                {
                    if (directories[i-1].Files.Length == directories[i].Files.Length)
                    {
                        foreach (DefaultFile file in directories[i-1].Files)
                        {
                            foreach (DefaultFile fileInfo in directories[i].Files)
                            {
                                if (file.File.Name == fileInfo.File.Name)
                                {
                                    string Txt, Txt1;
                                    Txt = file.FileRead();
                                    Txt1 = file.FileRead();
                                    if (Txt != Txt1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        directories[i].DirectoryNeedToDelete();
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < directories.Length; i++)
            {
                directories[i].DirectoryRename();
            }
            for (int i = 1; i < files.Length; i++)
            {
                if (files[i].NameWithoutUUID == files[i-1].NameWithoutUUID)
                {
                    string Txt, Txt1;
                    Txt = files[i].FileRead();
                    Txt1 = files[i-1].FileRead();
                    if (Txt == Txt1)
                    {
                        files[i].NeedToDelete = true;
                    }
                }
            }
            for (int i = 0; i < files.Length; i++)
            {
                files[i].FileRename();
            }
        }
    }
    class DefaultDirectory
    {
        DefaultFile[] files;
        DirectoryInfo directory;
        string nameWithoutUUID;
        bool needToDelete;

        public string NameWithoutUUID { get => nameWithoutUUID; set => nameWithoutUUID = value; }
        internal DefaultFile[] Files { get => files; set => files = value; }
        public DirectoryInfo Directory { get => directory; set => directory = value; }

        public DefaultDirectory(DirectoryInfo Directory)
        {
            this.Directory = Directory;
            Files = GetFiles(Directory);
            Regex regex = new Regex(@" \w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
            NameWithoutUUID = regex.Replace(this.Directory.Name, "");
        }
        public DefaultFile[] GetFiles(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            DefaultFile[] defaultFiles = new DefaultFile[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                defaultFiles[i] = new DefaultFile(files[i]);
            }
            return defaultFiles;
        }
        public void DirectoryRename()
        {
            Regex regex = new Regex(Directory.Name);
            string path = regex.Replace(Directory.FullName, "");
            if (needToDelete != true)
            {
                int p = 1;
                while (true)
                {
                    if (System.IO.Directory.Exists(path + "\\" + NameWithoutUUID) == false)
                    {
                        Directory.MoveTo(path + "\\" + NameWithoutUUID);
                        break;
                    }
                    string temp;
                    temp = NameWithoutUUID + "(" + p + ")";
                    if (System.IO.Directory.Exists(path + "\\" + temp))
                        p++;
                    else
                    {
                        Directory.MoveTo(path + "\\" + temp);
                        break;
                    }
                }
            }
            else
                directory.Delete(true);
        }
        public void DirectoryNeedToDelete()
        {
            needToDelete = true;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            Console.Write("Введите путь:\t");
            while (true)
            {
                path = Console.ReadLine();
                if (Regex.IsMatch(path, @"\S:\\\w", RegexOptions.IgnoreCase))
                {
                    break;
                }
            }
            MainDirectory directory = new MainDirectory(new DirectoryInfo(path));
            directory.Start();
            string compressionPath = directory.Directory.Parent.FullName + @"\Utrom's secrets.zip";
            ZipFile.CreateFromDirectory(path, compressionPath);
        }
    }
}
    }
}
