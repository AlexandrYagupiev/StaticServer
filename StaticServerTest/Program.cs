using System;
using System.Threading;
using System.Threading.Tasks;

public static class Server
{
    // Переменная для хранения текущего значения счетчика
    private static int count = 0;

    // Объект ReaderWriterLockSlim для управления доступом к ресурсу
    private static ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

    // Метод для получения текущего значения счетчика
    public static int GetCount()
    {
        // Блокируем ресурс для чтения
        rwLock.EnterReadLock();
        try
        {
            // Возвращаем текущее значение
            return count;
        }
        finally
        {
            // Освобождаем блокировку после завершения чтения
            rwLock.ExitReadLock();
        }
    }

    // Метод для добавления значения к текущему значению счетчика
    public static void AddToCount(int value)
    {
        // Блокируем ресурс для записи
        rwLock.EnterWriteLock();
        try
        {
            // Добавляем переданное значение к текущему значению
            count += value;
        }
        finally
        {
            // Освобождаем блокировку после завершения записи
            rwLock.ExitWriteLock();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        const int numClients = 20;
        const int numWriters = 5;

        // Создаем массив задач для моделирования множества клиентов
        Task[] tasks = new Task[numClients];

        for (int i = 0; i < numClients; i++)
        {
            if (i < numWriters)
            {
                // Несколько писателей
                tasks[i] = Task.Run(() => WriteRandomly(i + 1));
            }
            else
            {
                // Остальные - читатели
                tasks[i] = Task.Run(() => ReadContinuously(i + 1));
            }
        }

        // Ожидаем завершения всех задач
        Task.WaitAll(tasks);

        Console.WriteLine("Окончательный подсчет: " + Server.GetCount());
    }

    // Метод для моделируемого клиента-писателя
    static void WriteRandomly(int writerId)
    {
        Random random = new Random(writerId);
        while (true)
        {
            Thread.Sleep(random.Next(1000)); // Ждем случайное количество времени
            int value = random.Next(-50, 51); // Генерируем случайное число от -50 до 50
            Server.AddToCount(value);
            Console.WriteLine($"Писатель {writerId}: Добавлен {value}, Текущий счетчик: {Server.GetCount()}");
        }
    }

    // Метод для моделируемого клиента-читателя
    static void ReadContinuously(int readerId)
    {
        while (true)
        {
            Thread.Sleep(500); // Ждем полсекунды
            int currentCount = Server.GetCount();
            Console.WriteLine($"Читатель {readerId}: Текущий счетчик: {currentCount}");
        }
    }
}