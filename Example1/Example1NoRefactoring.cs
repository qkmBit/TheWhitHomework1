using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace LR8._2
{
    class LR
    {
        int konec;
        string vixod;

        public int Konec { get => konec; set => konec = value; }
        public string Vixod { get => vixod; set => vixod = value; }

        public string getSubString(LR into, string left, string right, int start)
        {
            string back = null;
            char[] Vxod = into.vixod.ToCharArray();
            char[] Left = left.ToCharArray();
            char[] Right = right.ToCharArray();
            bool flag = true;
            int count = 0;
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
                                    into.konec = k;
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
        public int GetSum(LR into, string left, string right, int start) // Находим сумму баллов
        {
            int sum = 0;
            int next = start;
            for (int i = 0; i < 8; i++) //Суммируем 8 строк, найденных методом getSubString
            {
                sum += int.Parse(into.getSubString(into, left, right, next));
                next = into.Konec;
            }
            return sum;
        }
    }
    class Program
    {
        static string GET(string type, int page)
        {
            WebRequest request = WebRequest.Create("http://playpit.ru/220/tableResultGET.php?type=" + type+"&page="+page) ;
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            string info;
            using (StreamReader reader = new StreamReader(stream))
            {
                info=reader.ReadToEnd();
            }
            stream.Close();
            response.Close();
            return info;
        }
        static int EndOfInputString(string vxod, string left, string right, int start)
        {
            char[] Vxod = vxod.ToCharArray();
            char[] Left = left.ToCharArray();
            char[] Right = right.ToCharArray();
            bool flag = true;
            int count = 0;
            int xxx=0;
            for (int i = start; i<Vxod.Length|| flag; i++)
            {
                count = 0;
                for (int j = 0; j < Left.Length; j++)
                {
                    if (Vxod[i] == Left[j])
                    {
                        i++;
                        count++;
                    }
                    else break;
                    if (count == Left.Length)
                    {
                        for (int k = i; flag; k++)
                        {
                            count = 0;
                            for (int f = 0; f < Right.Length; f++)
                            {
                                if (Vxod[k] == Right[f])
                                {
                                    k++;
                                    count++;
                                }
                                else break;
                                if (count == Right.Length)
                                    flag = false;
                            }
                                xxx = k;
                        }
                    }
                }
            }
            return xxx;
        }
        static string getSubString (string vxod, string left, string right, int start)
        {
            string back=null;
            char[] Vxod = vxod.ToCharArray();
            char[] Left = left.ToCharArray();
            char[] Right = right.ToCharArray();
            bool flag = true;
            int count=0;
            for (int i = start; i<Vxod.Length||flag; i++) //В первом цикле перебираем значения массива символов 
            {
                count = 0;
                for(int j = 0; j < Left.Length; j++) //Во втором цикле поэлементно сравниваем с левой границей 
                {
                    if (Vxod[i] == Left[j])
                    {
                        i++;
                        count++;
                    }
                    else break;
                    if (count == Left.Length) //Если нашли левую границу, то начинаем записывать символу в новую строку, пока не найдем правую границу
                    {
                        for (int k = i; flag ; k++)
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
        static int GetEnd(string vxod, string left, string right, int start)
        {
            int next = start;
            for (int i = 0; i < 8; i++)
            {
                next = EndOfInputString(vxod, left, right, next);
            }
            return next;
        }
        static int GetSum(string vxod, string left, string right, int start) // Находим сумму баллов
        {
            int sum=0;
            int next=start;
            for (int i = 0; i < 8; i++) //Суммируем 8 строк, найденных методом getSubString
            {
                sum += int.Parse(getSubString(vxod, left, right, next)); 
                next = EndOfInputString(vxod, left, right, next);
            }
            return sum;
        }
        static void Main(string[] args)
        {
            List<string[]> list = new List<string[]>();
            string labi= "";
            string practiki = "";
            for(int i = 0; i<4; i++)
            {
                var lab = GET("lb", i);
                var pract = GET("pr", i);
                labi += lab;
                labi += "\n";
                practiki += pract;
                practiki += "\n";
            }
            string[,] lablist = new string[20, 4]; //Создаем двумерный массив [20,4], где 20 - кол-во студентов в группе, 4 - колв-во полей
            int nextl = 0;
            int nextp = 0;
            LR test = new LR();
            test.Vixod = labi;
            LR practs = new LR();
            practs.Vixod = practiki;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 0) //1-e поле - ФИО
                    {
                        lablist[i, j] = test.getSubString(test, "<td>", "</td>", nextl);
                        nextl = test.Konec;
                        practs.getSubString(practs, "<td>", "</td>", nextp);
                        nextp = practs.Konec;
                    }
                    else if (j == 1) //2-е поле - балл по лабам
                    {
                        lablist[i, j] = test.GetSum(test, "<td>", "</td>", nextl).ToString();
                        nextl = test.Konec;
                    }
                    else if (j == 2) //3-е поле - балл по практикам
                    {
                        lablist[i, j] = practs.GetSum(practs, "<td>", "</td>", nextp).ToString();
                        nextp = practs.Konec;
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
                for (int i =0 ; i < 18-j; i++)
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
                Console.Write("|{0, -35}|{1, -10}|{2, -10}|{3, -10}|", lablist[i,0], lablist[i,1], lablist[i,2], lablist[i,3]);
                Console.WriteLine();
            }
            Console.WriteLine(".____________________________________________________________________.");
        }
    }
}
