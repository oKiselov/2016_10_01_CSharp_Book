using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books
{
    class Book
    {
        // Приватные поля 
        private string strBookName;
        private string strBookAuthor;
        private int iPrice;

        // Конструктор по умолчанию
        public Book()
        {
        }

        // Конструктор с параметрами 
        public Book(string strName, string strAuthor, int iPrice)
        {
            this.strBookName = strName;
            this.strBookAuthor = strAuthor;
            this.iPrice = iPrice;
        }

        // Аксессоры для полей класса 
        public string BookNameAccess
        {
            get { return strBookName; }
            set { strBookName = value; }
        }

        public string AuthorAccess
        {
            get { return strBookAuthor; }
            set { strBookAuthor = value; }
        }

        public int PriceAccess
        {
            get { return iPrice; }
            set { iPrice = value; }
        }

        // Метод поиска по автору в массиве объектов. Принимает по ссылке массив, искомое слово и позицию для поиска
        // Позиция в цикле смещается, если находится более одной книги одного автора 
        public static int SearchByAuthor(ref Book[] arrBooks, string searchedAuthor, int indexToStart)
        {
            for (int i = indexToStart; i < arrBooks.Length; i++)
            {
                if (arrBooks[i].strBookAuthor == searchedAuthor)
                    return i;
            }
            return -1;
        }

        // Для сортировки массива книг выбран стандартный метод с применением анонимного делегата / лямбда-выражения 
        public static void BookSort(ref Book[] arrBooks)
        {
            Array.Sort(arrBooks, (x, y) => String.Compare(x.strBookName, y.strBookName));
        }

        // Метод удаления с массива книг по цене 
        public static bool DeleteByPrice(ref Book[] arrBooks, int iCompPrice)
        {
            // С пониманием, что на такой подход тратится больше времени, но с желанием освежить приемы бинарного поиска
            // Сначала массив книг сортирован по их цене с помощью лямбда-выражения 
            Array.Sort(arrBooks, (x, y) =>
            {
                if (x.iPrice > y.iPrice)
                    return 1;
                else if (x.iPrice < y.iPrice)
                    return -1;
                else
                    return 0;
            });

            // далее установлены средний, начальный и конечный индексы 
            int average_index = 0, first_index = 0, last_index = arrBooks.Length - 1;

            if (last_index == -1)
                return false;

            // пока первый индекс больше последнего запущен бинарный поиск 
            while (first_index < last_index)
            {
                // средний индекс установлен в средний элемент массива 
                average_index = first_index + (last_index - first_index)/2;
                // сравнение искомого значения с средним индексом и корректировка среднего индекса  
                if (iCompPrice <= arrBooks[average_index].iPrice)
                    last_index = average_index;
                else
                    first_index = average_index + 1;
            }
            // при нахождении соответствия найденому элементу массива присваиваются значения последнего элемента
            // а последний соответственно удаляется 
            if (arrBooks[last_index].iPrice == iCompPrice)
            {
                arrBooks[last_index].PriceAccess = arrBooks[arrBooks.Length - 1].PriceAccess;
                arrBooks[last_index].AuthorAccess = arrBooks[arrBooks.Length - 1].AuthorAccess;
                arrBooks[last_index].BookNameAccess = arrBooks[arrBooks.Length - 1].BookNameAccess;
                Array.Resize(ref arrBooks, arrBooks.Length - 1);
                return true;
            }
            else
                return false;
        }

        // метод инициализации объектов в массиве книг с помощью акссесоров - свойств 
        public static void Setting(ref Book[] library, int i)
        {
            library[i] = new Book();
            Console.WriteLine("\r\nEnter the title of {0} book: ", i + 1);
            library[i].BookNameAccess = Console.ReadLine();
            Console.WriteLine("Enter author's name of {0} book: ", i + 1);
            library[i].AuthorAccess = Console.ReadLine();
            Console.WriteLine("Enter the price of {0} book: ", i + 1);
            library[i].PriceAccess = Program.ParserFunction();
        }

        // основной рабочий метод Меню 
        public static void Menu(ref Book[] library)
        {
            Console.WriteLine("Menu\nPush key:\n");
            Console.WriteLine("1 - to search by authors name");
            Console.WriteLine("2 - to sort by title");
            Console.WriteLine("3 - to delete by price");
            int option = Program.ParserFunction();
            // выбор действия 
            switch (option)
            {
                case 1:
                    // Поиск по автору 
                    Console.WriteLine("Enter authors name: ");
                    string Name = Console.ReadLine();
                    int searchingIndex = 0;
                    // создание массива значений для хранения индексов найденых книг
                    int[] arrayOfFoundedAuthors = new int[0];
                    do
                    {
                        // Запуск в цикле функции посика по автору со сдвигом на позицию найденной книги 
                        searchingIndex = Book.SearchByAuthor(ref library, Name, searchingIndex);
                        if (searchingIndex >= 0)
                        {
                            // добавление в массив индексов найденного индекса 
                            Array.Resize(ref arrayOfFoundedAuthors, arrayOfFoundedAuthors.Length + 1);
                            arrayOfFoundedAuthors[arrayOfFoundedAuthors.Length - 1] = searchingIndex;
                            searchingIndex++;
                        }
                        else
                            break;
                        // пока соответственные книги находятся 
                    } while (searchingIndex != -1);
                    // если массив индексов найденых книг пуст 
                    if (arrayOfFoundedAuthors.Length == 0)
                        Console.WriteLine("Not found");
                    // если массив не пуст 
                    else
                    {
                        Console.WriteLine("Here is(are) found books:");
                        // Вывод найденых книг на консоль 
                        for (int i = 0; i < arrayOfFoundedAuthors.Length; i++)
                            Console.WriteLine("Author: {0}, Title: {1}, Price: {2}",
                                library[arrayOfFoundedAuthors[i]].strBookAuthor,
                                library[arrayOfFoundedAuthors[i]].strBookName,
                                library[arrayOfFoundedAuthors[i]].iPrice);
                    }
                    break;
                case 2:
                    // Сортировка по названию книг 
                    Book.BookSort(ref library);
                    // Вывод на консоль 
                    Console.WriteLine("Here is(are) sorted library:");
                    for (int i = 0; i < library.Length; i++)
                        Console.WriteLine("Author: {0}, Title: {1}, Price: {2}",
                            library[i].strBookAuthor, library[i].strBookName, library[i].iPrice);
                    break;
                case 3:
                    // Удаление по цене 
                    Console.WriteLine("Enter the price to delete: ");
                    int priceToDelete = Program.ParserFunction();
                    // Запуск функции удаления по цене и вывод информации на консоль  
                    if (Book.DeleteByPrice(ref library, priceToDelete))
                    {
                        Console.WriteLine("Successful deleting");
                        Console.WriteLine("Here is(are) library after deleting:");
                        for (int i = 0; i < library.Length; i++)
                            Console.WriteLine("Author: {0}, Title: {1}, Price: {2}",
                                library[i].strBookAuthor, library[i].strBookName, library[i].iPrice);
                    }
                    else
                        Console.WriteLine("Not found");
                    break;
            }
        }
    }
}