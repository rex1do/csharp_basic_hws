string name = "";
bool running = true;
List<string> tasks = new();

string helpAnswer = "Вот список команд:\n" +
                    "/start - установить имя пользователя\n" +
                    "/help - информация о возможностях программы\n" +
                    "/info - информация о программе\n" +
                    "/echo - вывести введенный текст в консоль (только после установки имени)\n" +
                    "/addtask - добавить задачу в список\n" +
                    "/showtasks - отобразить список задач\n" + 
                    "/removetask - удалить задачу из списка\n" +
                    "/exit - выход из программы"; 

string infoAnswer = "Вот информация о приложении:\n" +
                    "Чат-бот Questofor\n" +
                    "Версия: Console\n" +
                    "Дата создания: 03.12.2025";

Console.WriteLine("Добро пожаловать!\n" +
                  "Список доступных команд: /start, /help, /info, /echo, /addtask, /showtasks, /removetask, /exit");

while (running)
{
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Введите команду");

        continue;
    }

    var prefix = !string.IsNullOrWhiteSpace(name) ? $"{name}, " : "";

    switch (input)
    {
        case "/start":
            {
                Console.WriteLine("Как Вас зовут?");
                SetName();

                break;
            }

        case "/help":
            {
                Console.WriteLine($"{prefix}{helpAnswer}");

                break;
            }

        case "/info":
            {
                Console.WriteLine($"{prefix}{infoAnswer}");

                break;
            }

        case string i when input.StartsWith("/echo"):
            {
                PrintEcho(i);

                break;
            }

        case "/addtask":
            {
                AddTask();

                break;
            }

        case "/showtasks":
            {
                ShowTasks();

                break;
            }

        case "/removetask":
            {
                RemoveTask();

                break;
            }

        case "/exit":
            {
                Console.WriteLine($"{prefix}Всего хорошего! Осуществляется выход из программы");
                running = false;

                break;
            }

        default:
            {
                Console.WriteLine($"{prefix}Такой команды не существует, ознакомиться со списком команд можно введя /help");

                break;
            }
    }
}

void SetName()
{
    while (true)
    {
        var newName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newName))
        {
            Console.WriteLine("Имя не может быть пустой строкой");

            continue;
        }

        name = newName.Trim();
        Console.WriteLine($"Имя сохранено, {name}.\n" +
                           "Теперь для Вас доступна команда /echo");
        break;
    }
}

void PrintEcho(string input)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Сначала установите имя пользователя");
        return;
    }

    var splitInput = input.Split(' ', 2);
    var argument = splitInput.Length > 1 ? splitInput[1] : "";

    if (string.IsNullOrWhiteSpace(argument))
    {
        Console.WriteLine("После /echo необходимо ввести текст");
    }
    else
    {
        Console.WriteLine(argument);
    }
}

void AddTask()
{
    Console.WriteLine("Пожалуйста, введите описание задачи:");
    var description = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("Описание задачи не может быть пустым");
        return;
    }

    description = description.Trim();
    tasks.Add(description);
    Console.WriteLine($"Задача \"{description}\" добавлена.");
}

bool ShowTasks()
{
    if (tasks.Count == 0)
    {
        Console.WriteLine("Список Ваших задач пуст");

        return false;
    }

    Console.WriteLine("Вот Ваш список задач:");

    for (int i = 0; i < tasks.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {tasks[i]}");
    }

    return true;
}

void RemoveTask()
{
    if (!ShowTasks())
    {
        return;
    }

    while (true)
    {
        Console.WriteLine("Введите номер задачи для удаления:");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var number) || number < 1 || number > tasks.Count)
        {
            Console.WriteLine("Некорректный номер задачи, попробуйте ещё раз");

            continue;
        }

        var removed = tasks[number - 1];
        tasks.RemoveAt(number - 1);

        Console.WriteLine($"Задача \"{removed}\" удалена.");

        return;
    }
}

