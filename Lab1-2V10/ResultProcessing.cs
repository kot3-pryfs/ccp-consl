using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_2V10
{
    public class ResultsProcessing
    {
        private readonly List<Catalog> _catalogList;
        private int sortStage = 0;

        public List<Catalog> CatalogList { get { return _catalogList; } }

        public ResultsProcessing()
        {
            _catalogList = new List<Catalog>();
        }

        /// Чтение данных из файла по указанному пути

        public void ReadFromFile(string filename)
        {
            using (var streamReader = new StreamReader(filename))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    string[] record = line.Split(' ');
                    string s = "";
                    for (int i = 3; i < record.Length; i++)
                    {
                        s += record[i] + " ";
                    }

                    _catalogList.Add(new Catalog(
                        double.Parse(record[0].Replace('.', ',')),
                        Convert.ToUInt32(record[1]),
                        record[2],
                        s
                    ));
                }
            }
        }

        /// <summary>
        /// Вывод в консоль одного элемента списка товаров
        /// </summary>
        /// <param name="catalog"></param>
        private static void PrintConcreteCatalog(Catalog catalog)
        {
            Console.WriteLine($"{catalog.Price,6}{catalog.Quantity,4}{catalog.Category,15}{catalog.Name,30}");
        }

        /// <summary>
        /// Вывод в консоль переданного списка товаров
        /// </summary>
        /// <param name="catalogs"></param>
        public void PrintList(IEnumerable<Catalog> catalogs)
        {
            Console.WriteLine($"{"Цена",6}{"Колличество",7}{"Категория товара",15}{"Наименование",30}");
            foreach (var catalog in catalogs)
            {
                PrintConcreteCatalog(catalog);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Вывод всего каталога
        /// </summary>
        public void PrintList()
        {
            PrintList(_catalogList);
        }

        /// <summary>
        /// Вывод каталогa по указанной категории
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="ArgumentException"></exception>
        public void PrintResultsBySpecificCatalog(string category)
        {
            if (!_catalogList.Any(x => x.Category.ToString().Equals(category)))
            {
                throw new ArgumentException("В списке нет товаров по данной категории");
            }

            Console.WriteLine($"Результаты по категории \"{category}\"");
            PrintList(_catalogList.Where(x => x.Category.ToString().Equals(category)));
        }

        /// <summary>
        /// Сумма всех товаров по указанной категории
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="ArgumentException"></exception>
        public void GetSummBySpecificCatalog(string category)
        {
            if (!_catalogList.Any(x => x.Category.ToString().Equals(category)))
            {
                throw new ArgumentException("В списке нет товаров по данной категории");
            }
            double summCatalog = 0;
            for (int i = 0; CatalogList.Count - 1 > i; i++)
            {
                if (CatalogList[i].Category == category)
                {
                    summCatalog += (CatalogList[i].Price * CatalogList[i].Quantity);
                }
            }
            Console.WriteLine($"Сумма товаров в данной категории = \"{summCatalog}\"");
        }

        /// <summary>
        /// Вывод товаров цена которых больше 100 рублей
        /// </summary>
        /// <param name="price"></param>
        public void PrintResultsBySpecificPrice(double price)
        {
            Console.WriteLine($"Товары с ценой больше чем {price}");
            PrintList(_catalogList.Where(x => x.Price > price));
        }

        /// <summary>
        /// Сортировка выбором с зависимостью от передаваемого параметра
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<Catalog> SortListWithSelectionSort(SortingParameter parameter)
        {
            var sortedList = new List<Catalog>(_catalogList);
            switch (parameter)
            {
                case SortingParameter.SortByQuantity:

                    for (int i = 0; i < sortedList.Count - 1; i++)
                    {
                        //поиск минимального числа
                        int max = i;
                        for (int j = i + 1; j < sortedList.Count; j++)
                        {
                            if (sortedList[j].Quantity > sortedList[max].Quantity)
                            {
                                max = j;
                            }
                        }
                        //обмен элементов
                        (sortedList[i], sortedList[max]) = (sortedList[max], sortedList[i]);
                    }
                    break;

                case SortingParameter.SortByCategory:
                    for (int i = 0; i < sortedList.Count - 1; i++)
                    {
                        //поиск минимального числа
                        int max = i;
                        for (int j = i + 1; j < sortedList.Count; j++)
                        {
                            if (sortedList[j].Price > sortedList[max].Price)
                            {
                                max = j;
                            }
                        }
                        //обмен элементов
                        (sortedList[i], sortedList[max]) = (sortedList[max], sortedList[i]);
                    }

                    for (int i = 0; i < sortedList.Count - 1; i++)
                    {
                        //поиск минимального числа
                        int max = i;
                        for (int j = i + 1; j < sortedList.Count; j++)
                        {
                            if (sortedList[j].Category.CompareTo(sortedList[max].Category) < 0)
                            {
                                max = j;
                            }
                        }
                        //обмен элементов
                        (sortedList[i], sortedList[max]) = (sortedList[max], sortedList[i]);
                    }

                    break;
            }
            return sortedList;
        }



        /// <summary>
        /// Сортировка слиянием с зависимостью от передаваемого параметра
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Catalog> SortListWithMergeSort(SortingParameter parameter)
        {
            switch (parameter)
            {
                case SortingParameter.SortByQuantity:
                    return SortListWithMergeSortByParameter(_catalogList, parameter);
                case SortingParameter.SortByCategory:
                    return SortListWithMergeSortByCategory(_catalogList, parameter);
                default:
                    throw new ArgumentException("Неверный параметр", nameof(parameter));
            }
        }

        /// <summary>
        /// Сортировка слиянием по категориям, проходящая через 2 этапа
        /// </summary>
        /// <param name="unsorted"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private List<Catalog> SortListWithMergeSortByCategory(List<Catalog> unsorted, SortingParameter parameter)
        {
            sortStage = 0;
            var result = unsorted;
            for (var i = 0; i < 2; i++)
            {
                result = SortListWithMergeSortByParameter(result, parameter);
                sortStage++;
            }

            return result;
        }

        /// <summary>
        /// Отправная точка сортировки слиянием
        /// </summary>
        /// <param name="unsorted"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private List<Catalog> SortListWithMergeSortByParameter(List<Catalog> unsorted, SortingParameter parameter)
        {
            if (unsorted.Count <= 1)
            {
                return unsorted;
            }

            var left = new List<Catalog>();
            var right = new List<Catalog>();
            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++)
            {
                left.Add(unsorted[i]);
            }

            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = SortListWithMergeSortByParameter(left, parameter);
            right = SortListWithMergeSortByParameter(right, parameter);
            if (parameter == SortingParameter.SortByCategory)
            {
                switch (sortStage)
                {
                    case 0:
                        return MergeByPrice(left, right);
                    case 1:
                        return MergeByCategory(left, right);
                }
            }

            return MergeByQuantity(left, right);
        }

        /// <summary>
        /// Сортировка слиянием по убыванию товара на складе
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static List<Catalog> MergeByQuantity(List<Catalog> left, List<Catalog> right)
        {
            var result = new List<Catalog>();
            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().Quantity <= right.First().Quantity)
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                        continue;
                    }

                    result.Add(left.First());
                    left.Remove(left.First());
                    continue;
                }

                if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                    continue;
                }

                result.Add(right.First());
                right.Remove(right.First());
            }

            return result;
        }

        /// <summary>
        /// Первый этам сортировки по категории товара
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static List<Catalog> MergeByCategory(List<Catalog> left, List<Catalog> right)
        {
            var result = new List<Catalog>();
            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    var str = left.First().Category;
                    int strVal = str.CompareTo(right.First().Category);
                    if (strVal < 0)
                    {
                        result.Add(left.First());
                        left.Remove(left.First());
                        continue;
                    }

                    result.Add(right.First());
                    right.Remove(right.First());
                    continue;
                }

                if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                    continue;
                }

                result.Add(right.First());
                right.Remove(right.First());
            }

            return result;
        }

        /// <summary>
        /// Второй этам сортировки по категории товара
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static List<Catalog> MergeByPrice(List<Catalog> left, List<Catalog> right)
        {
            var result = new List<Catalog>();
            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().Price <= right.First().Price)
                    {
                        result.Add(left.First());
                        left.Remove(left.First());
                        continue;
                    }

                    result.Add(right.First());
                    right.Remove(right.First());
                    continue;
                }

                if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                    continue;
                }

                result.Add(right.First());
                right.Remove(right.First());
            }

            return result;
        }

    }
}
