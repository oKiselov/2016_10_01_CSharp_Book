using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/* Написать класс Book. 
 * В нем должны быть методы для работы с Название, Автором и Стоимостью. 
 * Попросить пользователя ввести размер массива. 
 * Создает массив объектов класса Book и пользователь заполняет все данные о книге с клавиатуры. 
 * Предложить пользователю меню:
 * 1) Поиск по Автору книги
 * 2) Сортировка по Названию книги
 * 3) Удаление по стоимости.
 * Принимает число и в соотвествии с пунктом меню осуществляет нужное действие 
 * попутно запрашивая необходимые для дальнейшей работы данные.
 */

namespace Books
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the quantity of books in your library?");
                int sizeOfArray = ParserFunction();
                // Создание массива книг указанной длины 
                Book[] library = new Book[sizeOfArray];
                // инициализация всех книг массива 
                for (int i = 0; i < library.Length; i++)
                {
                    Book.Setting(ref library,i);
                }
                // запуск метода меню 
                Book.Menu(ref library);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Parsing from string to int
         public static int ParserFunction()
        {
            int iCounter = int.Parse(Console.ReadLine());
            return iCounter;
        }
    }
}