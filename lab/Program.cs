using ClassLibrary1;
namespace lab
{
    internal class Program
    {
        /// <summary>
        /// функция для вывода меню
        /// </summary>
        /// <param name="text">само меню</param>
        static void PrintMenu(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; //смена цвета текста консоли
            Console.WriteLine(message);
            Console.WriteLine("Введите соответствующий номер операции");
            Console.ResetColor();
        }

        /// <summary>
        /// добавление случайного человека (Person) в таблицу
        /// </summary>
        /// <param name="hTable"></param>
        public static void AddRandomPeople(ref MyNewHTable<Person> hTable)
        {
            int numberOfElementsAdd = ReadNumbers.ReadInt(1, 10000, "Впишите кол-во элементов, которые вы хотите добавить");
            for (int i = 0; i < numberOfElementsAdd; i++)
            {
                Person p = new Person();
                p.RandomInit();
                int initialSize = hTable.Size;
                hTable.Add(p);
                if (hTable.Size > initialSize)
                    Console.WriteLine("В хэш таблице недостаточно мест, она увеличена");
                Console.WriteLine("Элемент был добавлен.");
            }
        }

        /// <summary>
        /// поиск и удаление человека из хэш таблицы
        /// </summary>
        /// <param name="hTable"></param>
        public static void RemovePersonFromHashTable(ref MyNewHTable<Person> hTable)
        {
            Person per2 = new Person();
            per2.Init();
            if (hTable.Remove(per2)) //был ли удален элемент
                Console.WriteLine("Элемент был удален");
            else
                Console.WriteLine("Элемент не был найден в хэш таблице");
        }

        /// <summary>
        /// изменение уже существующего человека в хэш-таблице
        /// </summary>
        /// <param name="table"></param>
        public static void ChangeDataHTable(ref MyNewHTable<Person> table)
        {
            if (table.Count == 0)
            {
                Console.WriteLine("таблица пустая!");
                return;
            }
            int indexPerson;
            do
            {
                Console.WriteLine("Напишите индекс человека, которого хотите заменить в таблице");
                indexPerson = ReadNumbers.ReadInt(0, table.Size);
                if (table.table[indexPerson] == null)
                    Console.WriteLine($"Нет человека стоящем на месте {indexPerson}");
            } while (table.table[indexPerson] == null);
            Person p = new Person();
            p.RandomInit();
            table[indexPerson] = p;
            Console.WriteLine("Человек был изменен");
        }

        static void Main(string[] args)
        {
            //инициализация журналов и хэш-таблиц
            Journal journal1 = new Journal();
            Journal journal2 = new Journal();
            MyNewHTable<Person> hTable1 = new MyNewHTable<Person>("Хэш-таблица 1");
            MyNewHTable<Person> hTable2 = new MyNewHTable<Person>("Хэш-таблица 2");

            //привязка сигналов
            hTable1.HTableCountChanged += journal1.NumberOfElementsChanged;
            hTable1.HTableReferenceChanged += journal1.ReferenceChanged;
            hTable1.HTableReferenceChanged += journal2.ReferenceChanged;
            hTable2.HTableReferenceChanged += journal2.ReferenceChanged;

            string? operationNumber;
            Console.WriteLine("На данный момент ваши таблиы пусты");
            string textMenu = "    Первая таблица" + string.Concat(Enumerable.Range(0, 40 - 18).Select(i => " ")) + "    Вторая таблица\n" +
                "1 - Вывести таблицу" + string.Concat(Enumerable.Range(0, 40 - 19).Select(i => " ")) + "6 - Вывести таблицу\n" +
                "2 - Добавить элементы" + string.Concat(Enumerable.Range(0, 40 - 21).Select(i => " ")) + "7 - Добавить элементы\n" +
                "3 - Удалить элемент" + string.Concat(Enumerable.Range(0, 40 - 19).Select(i => " ")) + "8 - Удалить элемент\n" +
                "4 - Изменить элемент" + string.Concat(Enumerable.Range(0, 40 - 20).Select(i => " ")) + "9 - Изменить элемент\n" +
                "5 - Вывести журнал 1" + string.Concat(Enumerable.Range(0, 40 - 20).Select(i => " ")) + "10 - Вывести журнал 2\n" +
                "11 - Конец программы";
            do
            {
                PrintMenu(textMenu);
                operationNumber = Console.ReadLine();
                switch(operationNumber)
                {
                    case "1": //вывод 1 хэш-таблицы
                        hTable1.PrintHTable();
                        break;
                    case "6": //вывод 2 хэш-таблицы
                        hTable2.PrintHTable();
                        break;
                    case "2": //добавление элемента в 1 хэш-таблицу
                        AddRandomPeople(ref hTable1);
                        hTable1.PrintHTable();
                        break;
                    case "7": //добавление элемента во 2 хэш-таблицу
                        AddRandomPeople(ref hTable2);
                        hTable2.PrintHTable();
                        break;
                    case "3": //удаление элемента в 1 таблице
                        RemovePersonFromHashTable(ref hTable1);
                        hTable1.PrintHTable();
                        break;
                    case "8": //удаление элемента во 2 таблице
                        RemovePersonFromHashTable(ref hTable2);
                        hTable2.PrintHTable();
                        break;
                    case "4": //поменять значение элемента в 1 таблице
                        ChangeDataHTable(ref hTable1);
                        hTable1.PrintHTable();
                        break;
                    case "9": //поменять значение элемента во 2 таблице
                        ChangeDataHTable(ref hTable2);
                        hTable2.PrintHTable();
                        break;
                    case "5": //вывод журнала 1
                        journal1.PrintJournal();
                        break;
                    case "10": //вывод журнала 2
                        journal2.PrintJournal();
                        break;
                    default: //вывод ошибки при некорректном вводе
                        if (operationNumber != "11")
                            Console.WriteLine("\aВпишите номер операции (целое число от 1 до 11)!");
                        break;
                }
            } while (operationNumber != "11"); //программа действует пока пользователь не введет 11
        }
    }
}