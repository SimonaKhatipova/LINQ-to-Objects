using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ClassLibraryFor14;
using ClassLibrary1;

namespace Lab14
{
    class Program
    {
        private static Random rnd = new Random();

        static List<List<Transport>> City = new List<List<Transport>>();
        

        static int ReadIntNumber(string message, int left, int right)
        {
            bool ok = false;
            int number = 1;
            do
            {
                try
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor =ConsoleColor.Black; 
                    Console.WriteLine(message);
                    Console.ResetColor(); 
                    string buf = Console.ReadLine();
                    number = Convert.ToInt32(buf);//возможно возникновение исключений вида FormatException, OverflowException               
                    if (number >= left && number <= right) ok = true;
                    else
                    {
                        Console.WriteLine("Неверный ввод. Попробуйте ввести значение еще раз.");
                        ok = false;
                    }
                }
                //Исключение, которое возникает в случае, если формат аргумента недопустим или строка составного формата построена неправильно.
                catch (FormatException e)
                {
                    Console.WriteLine(e);
                    ok = false;
                }
            } while (!ok);
            return number;
        }
        public static List<Transport> MakeList()
        {
            List<Transport> list = new List<Transport>();
            var n = ReadIntNumber("Введите количество транспорта на каждом  вокзале ", 1, 100000000);
            for (int i = 0; i < n; i++)
            {
                list.Add(NewTransport(rnd.Next(0, 3)));
            }
            return list;
        }

        public static Transport NewTransport(int i)
        {
            switch (i)
            {
                case 0: return NewTrain1();
                case 1: return Expresses(rnd.Next(0, 5));
                default: return Expresses(rnd.Next(0, 5));
            }
        }
        public static Train NewTrain1()
        {
            string brand = Brands(rnd.Next(0, 6));
            int capacity = rnd.Next(100, 500);
            int speed = rnd.Next(50, 91);

            return new Train(speed, capacity, brand);
        }
        //список названий
        static string Brands(int i)
        {
            string[] strArr = new string[6] { "Черная жемчужина", "Летучий голландец", "Разящий", "Месть королевы Анны", "Стремительный", "Перехватчик" };
            return strArr[i];
        }       

        static Express Expresses(int i)
        {
            Express[] expresses = new Express[]
            {
                
                new Express(100,398,"Потешный"),
                new Express(101,399,"Потешный"),
                new Express(102,401,"Потешный"),
                new Express(103,402,"Потешный"),
                new Express(104,411,"Потешный"),
                new Express(105,412,"Потешный")
            };
            return expresses[i];
        }
        
        static void PrintAll(List<List<Transport>> City)
        {

            var i = 1;
            foreach (var list in City)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Вокзал {0}", i);
                Console.ForegroundColor = ConsoleColor.White;
                foreach (Transport p in list)
                {
                    p.Show();
                }
                Console.WriteLine();
                i++;
            }
        }

        // Печать результатов поисков
        static void PrintResults(IEnumerable<Transport> enumerable)
        {
            var results = enumerable.ToList();
            if (results.Count() != 0)
            {
                foreach (Transport transport in results)
                {
                    transport.Show();
                }
            }
            else
            {
                Console.WriteLine("Объекты не найдены.");
            }
        }

        //Запрос 1 на выборку
        ///Поиск всех поездов в городе
        static void SearchLinq(string brand, List<List<Transport>> City)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ТС поезд в городе:");
            Console.ForegroundColor = ConsoleColor.White;
            var search =
                from list in City
                from transport in list
                where (transport is Train) && ((Train)transport).Brand == brand
                select transport;
            PrintResults(search);
        }

        //метод расширения
        static void SearchEM(string brand, List<List<Transport>> City)
        {
            int i = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Поезда во всем городе:");
            Console.ForegroundColor = ConsoleColor.White;
            var search = City.SelectMany(list => list.Select(transport => { if (transport is Train && ((Train)transport).Brand == brand) return transport; else return null; }));
            foreach (var p in search)
            {
                if (p != null)
                {
                    p.Show();
                    i++;
                }
            }
            if (i == 0) Console.WriteLine("Объекты не найдены.");
            //PrintResults(search);
        }
        
        //Запрос 2 счетчик
        //LINQ-запрос
        static void CountLinq(int capacity, List<List<Transport>> factory)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Число экспрессов с вместимостью более 400:");
            Console.ForegroundColor = ConsoleColor.White;
            int numb =
                 (from list in City
                  from trans in list
                  where (trans is Express)
                  where trans.Capacity > 400
                  select trans).Count<Transport>();
            Console.WriteLine("Результат " + numb);
        }

        //метод расширения
        static void CountEM(int capacity, List<List<Transport>> City)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Число экспрессов с вместимостью более 400:");
            Console.ForegroundColor = ConsoleColor.White;
            int number = City.SelectMany(list => list.Where(trans => (trans is Express) && (trans.Capacity > 400))).Count<Transport >();
            Console.WriteLine("Результат " + number);
        }


        //Запрос 3 пересечения
        /// Поиск поездов с одинаковыми названиями
        /// //LINQ-запрос
        static void IntersectionLinq(List<string> list_1, List<string> list_2)
        {
            var equal = (from p1 in list_1 select p1).SequenceEqual(from p2 in list_2 select p2);
            if (equal == true)
                Console.WriteLine("В городе только 1 поезд!");
            else
            {
                int i = 0;
                var intersection = (from p in list_1 select p).Intersect(from p2 in list_2 select p2);
                foreach (var str in intersection)
                {
                    Console.Write(str + " | ");
                    i++;
                }
                if (i == 0) Console.WriteLine("Одинаковых поездов на вокзалах нет");
                Console.WriteLine();
            }
        }

        //метод расширения
        static void IntersectionEM(List<string> list_1, List<string> list_2)
        {
            var equal = list_1.SequenceEqual(list_2);
            if (equal == true)
                Console.WriteLine("В городе только 1 поезд!!");
            else
            {
                int i = 0;
                var intersection = list_1.Intersect(list_2);
                foreach (var str in intersection)
                {
                    Console.Write(str + " | ");
                    i++;
                }
                if (i == 0) Console.WriteLine("Одинаковых поездов на вокзалах нет!");
                Console.WriteLine();
            }
        }
        static List<string> Brands_list(List<Transport> list)
        {
            List<string> br_l = new List<string>();
            foreach (var p in list)
            {
                br_l.Add(p.Brand);
            }
            return br_l ;
        }


        //Запрос 4 Агрегирование данных
        //LINQ-запрос
        static void AverageLinq(List<Transport > list)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Средняя скорость ТС на первом вокзале: {0} км/ч", (from p in list select p.Speed ).Average());
            Console.ForegroundColor = ConsoleColor.White;
        }

        //метод расширения
        static void AverageEM(List<Transport> list)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Средняя скорость ТС на первом вокзале: {0} км/ч", list.Average<Transport >(p => p.Speed ));
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Запрос 5 Объединение коллекций
        //LINQ-запрос
        static void UnionLinq(List<Transport > list_1, List<Transport> list_2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Объединение вокзалов:");
            Console.ForegroundColor = ConsoleColor.White;
            var equal = list_1.SequenceEqual(list_2);
            if (equal == true)
                Console.WriteLine("На предприятии только 1 вокзал!");
            else
            {
                var union = (from p1 in list_1 select p1).Union(from p2 in list_2 select p2);
                PrintResults(union);
            }
        }

        //метод расширения
        static void UnionEM(List<Transport > list_1, List<Transport > list_2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Объединение вокзалов:");
            Console.ForegroundColor = ConsoleColor.White;
            var equal = list_1.SequenceEqual(list_2);
            if (equal == true)
                Console.WriteLine("В городе только  вокзал!");
            else
            {
                var union = list_1.Union(list_2);
                PrintResults(union);
            }
        }
        static void Menu()
        {
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;

           // //Разность​
           // Console.WriteLine("разность вокзалов");
           ///* List<List<Transport>>*/ var carDiff = (from c in City.First() select c).Except(from c2 in City.Last() select c2);
           // PrintResults(carDiff);                      
           // Console.WriteLine();

            Console.WriteLine("1. Запрос на выборку (LINQ)");
            SearchLinq("Черная жемчужина", City);
            Console.WriteLine();

            Console.WriteLine("2. Запрос на выборку (Метод расширения)");
            SearchEM("Черная жемчужина", City);
            Console.WriteLine();

            Console.WriteLine("3. Получение счетчика (LINQ)");
            CountLinq(5, City);
            Console.WriteLine();

            Console.WriteLine("4. Получение счетчика (Метод расширения) ");
            CountEM(5, City);
            Console.WriteLine();

            Console.WriteLine("5. Использование операции над множествами (пересечение) (LINQ)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Поиск одинаковых ТС в первом и последнем вокзалах:");
            Console.ForegroundColor = ConsoleColor.White;
            IntersectionLinq(Brands_list(City.First()), Brands_list(City.Last()));
            Console.WriteLine();

            Console.WriteLine("6. Использование операции над множествами (пересечение) (Метод расширения)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Поиск одинаковых ТС в первом и последнем вокзалах:");
            Console.ForegroundColor = ConsoleColor.White;
            IntersectionEM(Brands_list(City.First()), Brands_list(City.First()));
            Console.WriteLine();

            Console.WriteLine("7. Агрегирование данных (LINQ)");
            AverageLinq(City.First());
            Console.WriteLine();

            Console.WriteLine("8. Агрегирование данных (Метод расширения) ");
            AverageEM(City.First());
            Console.WriteLine();

            Console.WriteLine("9. Объединение (LINQ) ");
            UnionLinq(City.First(), City.Last());
            Console.WriteLine();

            Console.WriteLine("10.Объединение (Метод расширения) ");
            UnionEM(City.First(), City.Last());
            Console.WriteLine();

        }

        static void Main(string[] args)
        {
            var n = ReadIntNumber("Введите количество вокзалов в городе", 1, 100000000);
            for (int i = 0; i < n; i++)
            {
                City.Add(MakeList());
            }
            PrintAll(City);
            Menu();                    

            Console.ReadLine();
        }
    }
}
