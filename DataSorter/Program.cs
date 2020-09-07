using CsvHelper;
using DataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataSorter
{
    class Program
    {
        public const string FilePath = "../../../products.csv";

        public const int PartSize = 100;

        static void Main()
        {
            var records = ReadFile(FilePath);

            if ((records?.Length ?? 0) == 0)
                return;

            var sortedRecords = GetSortedParts(records);

            var sortedPartsOfRecords = Split(sortedRecords, PartSize);

            WritePartsToFiles(sortedPartsOfRecords);
        }

        private static void WritePartsToFiles(IEnumerable<List<Product>> sortedPartsOfRecords)
        {
            try
            {
                int partNumber = 1;

                foreach ( var part in sortedPartsOfRecords)
                {
                    using (var writer = new StreamWriter($"../../../sortedProducts_{partNumber}.csv"))
                    using (var csv = new CsvWriter(writer, CultureInfo.CurrentUICulture))
                    {
                        csv.WriteRecords(part);
                    }

                    partNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось записать в файлы\n{ex}");
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
        public static List<Product> GetSortedParts(IEnumerable<Product> records)
        {
            try
            {
                if (records == null)
                    return null;

                var sortedRecords = records
                    .OrderBy(r => r.Price).ToList();

                return sortedRecords;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Записи не отсортированы:\n{ex}");

                return null;
            }
        }

        public static IEnumerable<List<T>> Split<T>(List<T> records, int size)
        {
            for (int i = 0; i < records.Count; i += size)
            {
                yield return records.GetRange(i, Math.Min(size, records.Count - i));
            }
        }
    }
}
