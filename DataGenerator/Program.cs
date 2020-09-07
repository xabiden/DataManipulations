using CsvHelper;
using DataGenerator.Models;
using System;
using System.Globalization;
using System.IO;

namespace DataGenerator
{
    public class Program
    {
        const int DataListCapacity = 1000;

        public const string FilePath = "../../../products.csv";

        static void Main()
        {
            var records = InitializeRecords();

            WriteRecordsToFile(records);
        }

        public static Product[] InitializeRecords()
        {
            try
            {
                var records = new Product[DataListCapacity];

                var rnd = new Random();

                for (int i = 0; i < records.Length; i++)
                {
                    var id = i + 1;

                    var name = $"Продукт {id}";

                    var price = Math.Round(rnd.NextDouble() * 999 + 1, 2);

                    records[i] = new Product(id, name, price);
                }

                return records;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось инициализировать записи:\n{ex}");

                return null;
            }
        }

        public static void WriteRecordsToFile(Product[] records)
        {
            try
            {
                if ((records?.Length ?? 0) == 0)
                    return;

                using (var writer = new StreamWriter(FilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentUICulture))
                {
                    csv.WriteRecords(records);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось записать данные:\n{ex}");

                return;
            }
        }
    }
}
