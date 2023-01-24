using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_2V10
{
    //Вариант 10 Каталог товаров Соловьёв Е.В.
    class Program
    {
        static void Main(string[] args)
        {
            var rp = new ResultsProcessing();
            rp.ReadFromFile($@"D:\Ynic\Lab2V10\Lab2V10\Lab2V10\Data.txt");
            rp.PrintList();

            int number = 0;
            while (true)
            {
                Console.WriteLine("Выберите действие:\n" +
                    "1 - Вывести все товары в категории в выбранной категории\n" +
                    "2 - Вывести все товары стоимостью больше выбранной\n" +
                    "3 - Сортировать список сортировкой выбором\n" +
                    "4 - Сортировать список сортировкой слиянием\n" +
                    "5 - Общая стоимость товаров в выбранной категории\n" +
                    "0 - Выход");
                while (true)
                {
                    try
                    {
                        number = Convert.ToInt32(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Введите цифру от 1 до 5");
                    }
                }

                if (number == 0)
                {
                    break;
                }

                switch (number)
                {
                    case 1:
                        Console.WriteLine("Введите категорию товаров:\n");

                        while (true)
                        {
                            Console.WriteLine();
                            string category = Console.ReadLine();
                            try
                            {
                                rp.PrintResultsBySpecificCatalog(category);
                                break;
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        break;
                    case 2:
                        Console.WriteLine("Введите стоимость товара:");
                        while (true)
                        {
                            try
                            {
                                double price = double.Parse(Console.ReadLine().Replace('.', ','));
                                rp.PrintResultsBySpecificPrice(price);
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("Проверьте введенное значение");
                            }
                        }

                        break;
                    case 3:
                        Console.WriteLine("Выберите параметр сортировки:\n1 - По убыванию количества товаров на складе\n2 - По возрастанию категории товара, по убыванию стоимости");
                        while (true)
                        {
                            try
                            {
                                number = Convert.ToInt32(Console.ReadLine());
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("Проверьте введенное значение");
                            }
                        }

                        switch (number)
                        {
                            case 1:
                                rp.PrintList(rp.SortListWithSelectionSort(SortingParameter.SortByQuantity));
                                break;
                            case 2:
                                rp.PrintList(rp.SortListWithSelectionSort(SortingParameter.SortByCategory));
                                break;
                            default:
                                Console.WriteLine("Проверьте введенное значение");
                                break;
                        }

                        break;
                    case 4:
                        Console.WriteLine("Выберите параметр сортировки:\n1 - По убыванию количества товаров на складе\n2 - По возрастанию категории товара, по убыванию стоимости");
                        while (true)
                        {
                            try
                            {
                                number = Convert.ToInt32(Console.ReadLine());
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("Проверьте введенное значение");
                            }
                        }

                        switch (number)
                        {
                            case 1:
                                rp.PrintList(rp.SortListWithMergeSort(SortingParameter.SortByQuantity));
                                break;
                            case 2:
                                rp.PrintList(rp.SortListWithMergeSort(SortingParameter.SortByCategory));
                                break;
                            default:
                                Console.WriteLine("Проверьте введенное значение");
                                break;
                        }

                        break;
                    case 5:
                        while (true)
                        {
                            Console.WriteLine("Введите категорию товара:");
                            try
                            {
                                string category = Console.ReadLine();
                                Console.WriteLine($"Общая стоимость товаров в категории {category}");
                                rp.GetSummBySpecificCatalog(category);
                                break;
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            catch
                            {
                                Console.WriteLine("Проверьте введенное значение");
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Попробуйте снова");
                        break;
                }
            }

        }
    }
}
