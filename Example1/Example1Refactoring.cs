using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace smthing
{
    class Program
    {
        static public string getSubString(string into, string leftBorder, string rightBorder, int start, out int end)
        {
            string back = null;
            char[] Vxod = into.ToCharArray();
            char[] Left = leftBorder.ToCharArray();
            char[] Right = rightBorder.ToCharArray();
            bool flag = true;
            int count = 0;
            end = 0;
            for (int i = start; i < Vxod.Length || flag; i++) //В первом цикле перебираем значения массива символов 
            {
                count = 0;
                for (int j = 0; j < Left.Length; j++) //Во втором цикле поэлементно сравниваем с левой границей 
                {
                    if (Vxod[i] == Left[j])
                    {
                        i++;
                        count++;
                    }
                    else break;
                    if (count == Left.Length) //Если нашли левую границу, то начинаем записывать символу в новую строку, пока не найдем правую границу
                    {
                        for (int k = i; flag; k++)
                        {
                            count = 0;
                            for (int f = 0; f < Right.Length; f++) //Поэлементно сравниваем символы исходной строки с символами правой границы, пока её не найдем
                            {
                                if (Vxod[k] == Right[f])
                                {
                                    k++;
                                    count++;
                                }
                                else break;
                                if (count == Right.Length)
                                {
                                    flag = false;
                                    end = k;
                                }
                            }
                            if (flag == true)
                            {
                                back += Vxod[k];
                            }
                        }
                    }
                }
            }
            return back;
        }
        static public int GetSum(string into, string leftBorder, string rightBorder, int start, out int nextString) // Находим сумму баллов
        {
            int sum = 0;
            int next = start;
            for (int i = 0; i < 8; i++) //Суммируем 8 строк, найденных методом getSubString
            {
                sum += int.Parse(getSubString(into, leftBorder, rightBorder, next, out next));
            }
            nextString = next;
            return sum;
        }
        static string GETHTML(string type, int page)
        {
            WebRequest request = WebRequest.Create("http://playpit.ru/220/tableResultGET.php?type=" + type + "&page=" + page);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string info;
            using (StreamReader reader = new StreamReader(stream))
            {
                info = reader.ReadToEnd();
            }
            stream.Close();
            response.Close();
            return info;
        }
        static void Main(string[] args)
        {
            List<string[]> list = new List<string[]>();
            string labi = "";
            string practiki = "";
            for (int i = 0; i < 4; i++)
            {
                labi += GETHTML("lb", i)+ "\n";
                practiki += GETHTML("pr", i)+ "\n";
            }
            string[,] lablist = new string[20, 4]; //Создаем двумерный массив [20,4], где 20 - кол-во студентов в группе, 4 - колв-во полей
            int nextStringLab = 0;
            int nextStringPractic = 0;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 0) //1-e поле - ФИО
                    {
                        lablist[i, j] = getSubString(labi, "<td>", "</td>", nextStringLab, out nextStringLab);
                        getSubString(practiki, "<td>", "</td>", nextStringPractic, out nextStringPractic);
                    }
                    else if (j == 1) //2-е поле - балл по лабам
                    {
                        lablist[i, j] = GetSum(labi, "<td>", "</td>", nextStringLab, out nextStringLab).ToString();
                    }
                    else if (j == 2) //3-е поле - балл по практикам
                    {
                        lablist[i, j] = GetSum(practiki, "<td>", "</td>", nextStringPractic, out nextStringPractic).ToString();
                    }
                    else if (j == 3) //4-е поле - сумма баллов
                    {
                        int k = int.Parse(lablist[i, 1]) + int.Parse(lablist[i, 2]);
                        lablist[i, j] = k.ToString();
                    }
                }
            }
            for (int j = 0; j < 19; j++) //Сортировка пузырьком по сумме баллов
            {
                for (int i = 0; i < 18 - j; i++)
                {
                    if (int.Parse(lablist[i, 3]) < int.Parse(lablist[i + 1, 3]))
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            string temp = lablist[i, k];
                            lablist[i, k] = lablist[i + 1, k];
                            lablist[i + 1, k] = temp;
                        }
                    }
                }
            }
            Console.WriteLine(".____________________________________________________________________.");
            Console.WriteLine("|{0, -35}|{1, -10}|{2, -10}|{3, -10}|", "ФИО", "Лаб", "Практ", "Сумма");
            Console.WriteLine("|___________________________________|__________|__________|__________|");
            for (int i = 0; i < 19; i++)
            {
                Console.Write("|{0, -35}|{1, -10}|{2, -10}|{3, -10}|", lablist[i, 0], lablist[i, 1], lablist[i, 2], lablist[i, 3]);
                Console.WriteLine();
            }
            Console.WriteLine(".____________________________________________________________________.");
        }
    }
}