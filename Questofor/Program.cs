bool running = true;

List<string> tasks = new();
int taskCountLimit = 0;
int taskLength = 0;

const int minTaskLimits = 1;
const int maxTaskLimits = 100;

string? prefix = "";
string? input = "";
string? name = "";

string helpAnswer = "Вот список команд:\n" +
                    "/start - установить имя пользователя\n" +
                    "/help - информация о возможностях программы\n" +
                    "/info - информация о программе\n" +
                    "/echo - вывести введенный текст в консоль (только после установки имени)\n" +
                    "/addtask - добавить задачу в список\n" +
                    "/showtasks - отобразить список задач\n" +
                    "/removetask - удалить задачу из списка\n" +
                    "/exit - выход из программы\n";

string infoAnswer = "Вот информация о приложении:\n" +
                    "Чат-бот Questofor\n" +
                    "Версия: Console\n" +
                    "Дата создания: 03.12.2025\n";

string helloText = "\nДобро пожаловать!\n" +
                   "Список доступных команд: /start, /help, /info, /echo, /addtask, /showtasks, /removetask, /exit\n";

while (running)
{
    try
    {
        ShowStartMessage();

        input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Введите команду");
            continue;
        }

        switch (input)
        {
            case "/start":
                {
                    SetName();
                    break;
                }
            case "/help":
                {
                    ShowHelp();
                    break;
                }
            case "/info":
                {
                    ShowInfo();
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
                    InvokeExit();
                    break;
                }
            default:
                {
                    ShowNonExistCommand();
                    break;
                }
        }
    }

    catch (TaskCountLimitException ex)
    {
        Console.WriteLine(ex.Message);
    }

    catch (TaskLengthLimitException ex)
    {
        Console.WriteLine(ex.Message);
    }

    catch (DuplicateTaskException ex)
    {
        Console.WriteLine(ex.Message);
    }

    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }

    catch (Exception ex)
    {
        Console.WriteLine($"Произошла непредвиденная ошибка:\nType: {ex.GetType()},\nMessage: {ex.Message},\nStackTrace: {ex.StackTrace},\nInnerException: {ex.InnerException}");
    }
}



void ShowStartMessage()
{
    if (taskCountLimit == 0)
    {
        SetTaskCountLimit();
        SetTaskLengthLimit();

        Console.WriteLine(helloText);
    }
}
void SetTaskCountLimit()
{
    Console.WriteLine("Введите максимально допустимое количество задач");

    taskCountLimit = ParseAndValidateInt(Console.ReadLine(), minTaskLimits, maxTaskLimits);
}

void SetTaskLengthLimit()
{
    Console.WriteLine("Введите максимально допустимую длину задачи");

    taskLength = ParseAndValidateInt(Console.ReadLine(), minTaskLimits, maxTaskLimits);
}

void ShowHelp()
{
    Console.WriteLine($"{prefix}{helpAnswer}");
}

void ShowInfo()
{
    Console.WriteLine($"{prefix}{infoAnswer}");
}

void InvokeExit()
{
    Console.WriteLine($"{prefix}Всего хорошего! Осуществляется выход из программы");
    running = false;
}

void ShowNonExistCommand()
{
    Console.WriteLine($"{prefix}Такой команды не существует, ознакомиться со списком команд можно введя /help");
}

void SetName()
{
    Console.WriteLine("Как Вас зовут?");

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

        //обновляем префикс сразу здесь, а не в main
        prefix = $"{name}, ";

        return;
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
    if (tasks.Count >= taskCountLimit)
    {
        throw new TaskCountLimitException(taskCountLimit);
    }

    Console.WriteLine("Пожалуйста, введите описание задачи:");

    var description = Console.ReadLine();
    ValidateString(description);
    description = description!.Trim();

    if (description.Length > taskLength)
    {
        throw new TaskLengthLimitException(description.Length, taskLength);
    }

    if (tasks.Contains(description))
    {
        throw new DuplicateTaskException(description);
    }

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

    Console.WriteLine();

    return true;
}

void RemoveTask()
{
    if (!ShowTasks())
    {
        return;
    }

    Console.WriteLine("Введите номер задачи для удаления:");

    while (true)
    {
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

int ParseAndValidateInt(string? str, int min, int max)
{
    if (string.IsNullOrWhiteSpace(str) ||
        !int.TryParse(str, out var result) ||
        result < min ||
        result > max)
    {
        throw new ArgumentException($"Допустимый диапазон чисел от {min} до {max}");
    }

    return result;
}

void ValidateString(string? str)
{
    if (string.IsNullOrWhiteSpace(str))
    {
        throw new ArgumentException("Строка не может быть пустой");
    }
}

class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit)
            : base($"Превышено максимальное количество задач равное {taskCountLimit}")
    {
    }
}

class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int taskLength, int taskLengthLimit)
            : base($"Длина задачи '{taskLength}' превышает максимально допустимое значение {taskLengthLimit}")
    {
    }
}

class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
            : base($"Задача '{task}' уже существует")
    {
    }
}