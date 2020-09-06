using CsvHelper;
using DataGenerator.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataSorter
{
    class Program
    {
        public const string FilePath = "../../../products.csv";

        static void Main(string[] args)
        {
            var records = ReadFile(FilePath);

            if ((records?.Length ?? 0) == 0)
            {
                Console.WriteLine($"Массив прочитанных данных пуст или null");

                return;
            }
        }

        public static Product[] ReadFile(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    csv.Configuration.PrepareHeaderForMatch = 
                        (string header, int index) => header.ToLower();

                    var records = csv.GetRecords<Product>();

                    return records?.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вышло прочитать файл:\n{ex}");
                return null;
            }
        }
    }
}
