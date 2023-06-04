using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    public delegate void HTableHandler(object source, HTableHandlerEventArgs args);

    /// <summary>
    /// класс для храения/передачи переменных/аргументов
    /// </summary>
    public class HTableHandlerEventArgs
    {
        public object Data { get; }
        public string Name { get; }
        public string Message { get; set; }
        public object? Data2 { get; }

        /// <summary>
        /// конструктор с параметрами
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        /// <param name="data2"></param>
        public HTableHandlerEventArgs(object data, string name, string message, string data2 = "")
        {
            this.Data = data;
            this.Message = message;
            this.Data2 = data2;
            this.Name = name;
        }
    }

    /// <summary>
    /// класс для записи событий
    /// </summary>
    public class Journal
    {
        public List<JournalEntry> journal;

        /// <summary>
        /// конструктор класса
        /// </summary>
        public Journal()
        {
            journal = new List<JournalEntry>();
        }
        
        /// <summary>
        /// печать журнала
        /// </summary>
        public void PrintJournal()
        {
            if (journal.Count == 0)
            {
                Console.WriteLine("Журнал пуст");
                return;
            }
            foreach(JournalEntry j in journal)
                Console.WriteLine(j);
        }

        /// <summary>
        /// подписчик, действующий при изменении длины таблицы
        /// </summary>
        /// <param name="item"></param>
        /// <param name="args"></param>
        public void NumberOfElementsChanged(object item, HTableHandlerEventArgs args)
        {
            JournalEntry j = new JournalEntry(args.Name, args.Message, args.Data.ToString());
            journal.Add(j);
        }

        /// <summary>
        /// подписчик, действующий при изменении элемента таблицы
        /// </summary>
        /// <param name="item"></param>
        /// <param name="args"></param>
        public void ReferenceChanged(object item, HTableHandlerEventArgs args)
        {
            JournalEntry j = new JournalEntry(args.Name, args.Message, args.Data.ToString(), args.Data2.ToString());
            journal.Add(j);
        }

    }

    /// <summary>
    /// класс хранящий в себе данные о событии
    /// </summary>
    public class JournalEntry
    {
        public string Name { get; }
        public string TypeOfEdition { get; }
        public string ObjectEdited { get; }
        public string NewObject { get;  }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="name"></param>
        /// <param name="typeOfEdition"></param>
        /// <param name="objectEdited"></param>
        /// <param name="newObject"></param>
        public JournalEntry(string name, string typeOfEdition, string objectEdited, string newObject = "")
        {
            this.Name = name;
            this.TypeOfEdition = typeOfEdition;
            this.ObjectEdited = objectEdited;
            this.NewObject = newObject;
        }

        /// <summary>
        /// переопределение Tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (NewObject == "")
                return "В коллекции " + Name + " объект " + ObjectEdited + " " + TypeOfEdition;
            else
                return "В коллекции " + Name + " объект " + ObjectEdited + " " + TypeOfEdition + " " + NewObject;
        }
    }

    /// <summary>
    /// новая коллекция
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyNewHTable<T> : HTable<T> where T: ICloneable
    {
        public event HTableHandler HTableCountChanged;
        public event HTableHandler HTableReferenceChanged;

        public string Name { get; } //имя коллекции

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public MyNewHTable(string name, int size=10)
            : base(size)
        {
            Name = name;
        }

        /// <summary>
        /// вызов функции при событии - изменилась длина таблицы
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void OnCountChanged(object source, HTableHandlerEventArgs args)
        {
            HTableCountChanged?.Invoke(source, args);
        }

        /// <summary>
        /// вызов функции при событии - изменился элемент таблицы
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void OnReferenceChanged(object source, HTableHandlerEventArgs args)
        {
            HTableReferenceChanged?.Invoke(source, args);
        }

        /// <summary>
        /// переопределение add для добавления элементов
        /// </summary>
        /// <param name="item"></param>
        public override void Add(T item)
        {
            base.Add(item);
            OnCountChanged(this, new HTableHandlerEventArgs(item, this.Name, "был добавлен"));
        }

        /// <summary>
        /// удаление элементов из хэш-таблицы
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool Remove(T item)
        {
            OnCountChanged(this, new HTableHandlerEventArgs(item, this.Name, "был удален"));
            return base.Remove(item);
        }

        /// <summary>
        /// переопределенный индексатор
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public override T this[int i] 
        { 
            get => base[i];
            set
            {
                OnReferenceChanged(this, new HTableHandlerEventArgs(base[i], this.Name, "был изменен на", value.ToString()));
                base[i]=value;
            }
        }

        /// <summary>
        /// переопределенная печать хэш-таблицы
        /// </summary>
        public override void PrintHTable()
        {
            Console.WriteLine(this.Name + ":");
            base.PrintHTable();
        }
    }
}
