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
    class Program
    {
        static string FileRead(string path)
        {
            string txt;
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] vs = new byte[fstream.Length];
                fstream.Read(vs, 0, vs.Length);
                txt = System.Text.Encoding.Default.GetString(vs);
            }
            return txt;
        }
        static void DirectoryRename(string path, string name, DirectoryInfo directory1,DirectoryInfo directory2)
        {
            int p = 1;
            if (Directory.Exists(path + "\\" + name) == false)
                directory1.MoveTo(path + "\\" + name);
            while (true)
            {
                name = name + "(" + p + ")";
                if (Directory.Exists(path + "\\" + name))
                    p++;
                else
                {
                    directory2.MoveTo(path + "\\" + name);
                    break;
                }
            }
        }
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
            DirectoryInfo directory = new DirectoryInfo(path);
            Regex regex = new Regex(@" \w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
            if (directory.Exists)
            {
                DirectoryInfo[] directories = directory.GetDirectories();
                string[] DirectoriesNameWithoutUUID = new string[directories.Length];
                string[] DirectoriesName = new string[DirectoriesNameWithoutUUID.Length];
                for (int i = 0; i < directories.Length; i++)
                {
                    DirectoriesNameWithoutUUID[i] = directories[i].Name;
                    DirectoriesName[i] = directories[i].Name;
                }
                Array.Sort(DirectoriesNameWithoutUUID);
                for (int i = 0; i < directories.Length; i++)
                {

                    DirectoriesNameWithoutUUID[i] = regex.Replace(directories[i].Name, "");
                }
                for (int i = 1; i < directories.Length; i++)
                {
                    if (DirectoriesNameWithoutUUID[i] == DirectoriesNameWithoutUUID[i - 1])
                    {
                        FileInfo[] filesInfo1 = directories[i].GetFiles();
                        FileInfo[] fileInfo2 = directories[i - 1].GetFiles();
                        if (fileInfo2.Length != filesInfo1.Length)
                        {
                            DirectoryRename(path, DirectoriesNameWithoutUUID[i], directories[i - 1], directories[i]);
                        }
                        else
                        {
                            foreach (FileInfo file in fileInfo2)
                            {
                                foreach (FileInfo fileInfo in filesInfo1)
                                {
                                    if (file.Name == fileInfo.Name)
                                    {
                                        string Txt, Txt1;
                                        Txt = FileRead($"{path}\\{DirectoriesName[i - 1]}\\{file.Name}");
                                        Txt1 = FileRead($"{path}\\{DirectoriesName[i]}\\{fileInfo.Name}");
                                        if (Txt != Txt1)
                                        {
                                            DirectoryRename(path, DirectoriesNameWithoutUUID[i], directories[i - 1], directories[i]);
                                            break;
                                        }
                                        else
                                        {
                                            directories[i - 1].MoveTo(path + "\\" + DirectoriesNameWithoutUUID[i]);
                                            directories[i].Delete(true);
                                        }
                                    }
                                    else
                                    {
                                        DirectoryRename(path, DirectoriesNameWithoutUUID[i], directories[i - 1], directories[i]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                directories = directory.GetDirectories();
                DirectoriesNameWithoutUUID = new string[directories.Length];
                for (int i = 0; i < directories.Length; i++)
                {
                    DirectoriesNameWithoutUUID[i] = directories[i].Name;
                    DirectoriesNameWithoutUUID[i] = regex.Replace(directories[i].Name, "");
                }
                for (int i = 0; i < DirectoriesNameWithoutUUID.Length; i++)
                {
                    if (Directory.Exists(path + "\\" + DirectoriesNameWithoutUUID[i]) == false)
                    {
                        directories[i].MoveTo(path + "\\" + DirectoriesNameWithoutUUID[i]);
                    }
                }
                FileInfo[] files = directory.GetFiles();
                string[] FilesWithoutUUID = new string[files.Length];
                string[] FilesName = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    FilesName[i] = files[i].Name;
                    FilesWithoutUUID[i] = files[i].Name;
                }
                Array.Sort(FilesWithoutUUID);
                for (int i = 0; i < files.Length; i++)
                {
                    FilesWithoutUUID[i] = regex.Replace(files[i].Name, "");
                }
                for (int i = 1; i < files.Length; i++)
                {
                    if (FilesWithoutUUID[i] == FilesWithoutUUID[i - 1])
                    {
                        string Txt, Txt1;
                        Txt = FileRead($"{path}\\{FilesName[i]}");
                        Txt1 = FileRead($"{path}\\{FilesName[i - 1]}");
                        if (Txt != Txt1)
                        {
                            int p = 1;
                            files[i - 1].MoveTo(path + "\\" + FilesWithoutUUID[i]);
                            while (true)
                            {
                                string filename;
                                string[] FileAndItsExtension = FilesWithoutUUID[i].Split('.'); 
                                filename = FileAndItsExtension[0];
                                for (int j = 1; j < FileAndItsExtension.Length - 2; j++)
                                {
                                    filename = filename + " " + FileAndItsExtension[j];
                                }
                                filename = filename + " (" + p + ")." + FileAndItsExtension[FileAndItsExtension.Length - 1];
                                if (Directory.Exists(path + "\\" + FilesWithoutUUID[i]))
                                    p++;
                                else
                                {
                                    FilesWithoutUUID[i] = filename;
                                    files[i].MoveTo(path + "\\" + FilesWithoutUUID[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
                files = directory.GetFiles();
                FilesWithoutUUID = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    FilesWithoutUUID[i] = files[i].Name;
                    FilesWithoutUUID[i] = regex.Replace(files[i].Name, "");
                }
                for (int i = 0; i < files.Length; i++)
                {
                    if (File.Exists(path + "\\" + FilesWithoutUUID[i]) == false)
                    {
                        files[i].MoveTo(path + "\\" + FilesWithoutUUID[i]);
                    }
                }
                string compressionPath = directory.Parent.FullName + @"\Utrom's secrets.zip";
                ZipFile.CreateFromDirectory(path, compressionPath);
            }
        }
    }
}