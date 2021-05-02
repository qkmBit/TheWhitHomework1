using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace whitezv
{
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
            DirectoryInfo directory = new DirectoryInfo(path);
            Regex regex = new Regex(@" \w{8}-\w{4}-\w{4}-\w{4}-\w{12}");
            if (directory.Exists)
            {
                DirectoryInfo[] dirs = directory.GetDirectories();
                string[] dUUID = new string[dirs.Length];
                string[] d = new string[dUUID.Length];
                for (int i = 0; i < dirs.Length; i++)
                {
                    dUUID[i] = dirs[i].Name;
                }
                for (int i = 0; i < dirs.Length; i++)
                {
                    d[i] = dirs[i].Name;
                }
                Array.Sort(dUUID);
                for (int i = 0; i < dirs.Length; i++)
                {

                    dUUID[i] = regex.Replace(dirs[i].Name, "");
                }
                for (int i = 1; i < dirs.Length; i++)
                {
                    if (dUUID[i] == dUUID[i - 1])
                    {
                        FileInfo[] filesInfo1 = dirs[i].GetFiles();
                        FileInfo[] fileInfo2 = dirs[i - 1].GetFiles();
                        if (fileInfo2.Length != filesInfo1.Length)
                        {
                            int p = 1;
                            if (Directory.Exists(path + "\\" + dUUID[i]) == false)
                                dirs[i - 1].MoveTo(path + "\\" + dUUID[i]);
                            while (true)
                            {
                                dUUID[i] = dUUID[i] + "(" + p + ")";
                                if (Directory.Exists(path + "\\" + dUUID[i]))
                                    p++;
                                else
                                {
                                    dirs[i].MoveTo(path + "\\" + dUUID[i]);
                                    break;
                                }
                            }
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
                                        using (FileStream fstrem = File.OpenRead($"{path}\\{d[i - 1]}\\{file}"))
                                        {
                                            byte[] vs = new byte[fstrem.Length];
                                            fstrem.Read(vs, 0, vs.Length);
                                            Txt = System.Text.Encoding.Default.GetString(vs);
                                        }
                                        using (FileStream fstrem = File.OpenRead($"{path}\\{d[i]}\\{fileInfo}"))
                                        {
                                            byte[] vs = new byte[fstrem.Length];
                                            fstrem.Read(vs, 0, vs.Length);
                                            Txt1 = System.Text.Encoding.Default.GetString(vs);
                                        }
                                        if (Txt != Txt1)
                                        {
                                            int p = 1;
                                            dirs[i - 1].MoveTo(path + "\\" + dUUID[i]);
                                            while (true)
                                            {
                                                dUUID[i] = dUUID[i] + "(" + p + ")";
                                                if (Directory.Exists(path + "\\" + dUUID[i]))
                                                    p++;
                                                else
                                                {
                                                    dirs[i].MoveTo(path + "\\" + dUUID[i]);
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            dirs[i - 1].MoveTo(path + "\\" + dUUID[i]);
                                            dirs[i].Delete(true);
                                        }
                                    }
                                    else
                                    {
                                        int p = 1;
                                        dirs[i - 1].MoveTo(path + "\\" + dUUID[i]);
                                        while (true)
                                        {
                                            dUUID[i] = dUUID[i] + "(" + p + ")";
                                            if (Directory.Exists(path + "\\" + dUUID[i]))
                                                p++;
                                            else
                                            {
                                                dirs[i].MoveTo(path + "\\" + dUUID[i]);
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                DirectoryInfo[] directories = directory.GetDirectories();
                string[] str = new string[directories.Length];
                for (int i = 0; i < directories.Length; i++)
                {
                    str[i] = directories[i].Name;
                    str[i] = regex.Replace(directories[i].Name, "");
                }
                for (int i = 0; i < str.Length; i++)
                {
                    if (Directory.Exists(path + "\\" + str[i]) == false)
                    {
                        directories[i].MoveTo(path + "\\" + str[i]);
                    }
                }
                FileInfo[] files = directory.GetFiles();
                string[] fUUID = new string[files.Length];
                string[] f = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    f[i] = files[i].Name;
                    fUUID[i] = files[i].Name;
                }
                Array.Sort(fUUID);
                for (int i = 0; i < files.Length; i++)
                {
                    fUUID[i] = regex.Replace(files[i].Name, "");
                }
                for (int i = 1; i < files.Length; i++)
                {
                    if (fUUID[i] == fUUID[i - 1])
                    {
                        string Txt, Txt1;
                        using (FileStream fstrem = File.OpenRead($"{path}\\{f[i]}"))
                        {
                            byte[] vs = new byte[fstrem.Length];
                            fstrem.Read(vs, 0, vs.Length);
                            Txt1 = System.Text.Encoding.Default.GetString(vs);
                        }
                        using (FileStream fstrem = File.OpenRead($"{path}\\{f[i - 1]}"))
                        {
                            byte[] vs = new byte[fstrem.Length];
                            fstrem.Read(vs, 0, vs.Length);
                            Txt = System.Text.Encoding.Default.GetString(vs);
                        }
                        if (Txt != Txt1)
                        {
                            int p = 1;
                            files[i - 1].MoveTo(path + "\\" + fUUID[i]);
                            while (true)
                            {
                                string res;
                                string[] rs = fUUID[i].Split('.');
                                res = rs[0];
                                for (int j = 1; j < rs.Length-2; j++)
                                {
                                    res = res + " " + rs[j];
                                }
                                res = res + " (" + p + ")." + rs[rs.Length-1];
                                if (Directory.Exists(path + "\\" + fUUID[i]))
                                    p++;
                                else
                                {
                                    fUUID[i] = res;
                                    files[i].MoveTo(path + "\\" + fUUID[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
                FileInfo[] filestest = directory.GetFiles();
                string[] test = new string[filestest.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    test[i] = filestest[i].Name;
                    test[i] = regex.Replace(filestest[i].Name, "");
                }
                for (int i = 0; i < test.Length; i++)
                {
                    if (File.Exists(path + "\\" + test[i]) == false)
                    {
                        filestest[i].MoveTo(path + "\\" + test[i]);
                    }
                }
                string compressionPath = directory.Parent.FullName + @"\Utrom's secrets.zip";
                ZipFile.CreateFromDirectory(path, compressionPath);
                Console.ReadLine();
            }
        }
    }
}