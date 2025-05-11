// Интерфейсы
public interface ISettingsProvider
{
    GameSettings GetSettings();
}

public interface INumberGenerator
{
    int Generate(int min, int max);
}

public interface IInputHandler
{
    int GetGuess();
}

public interface IOutputHandler
{
    void ShowMessage(string message);
    void ShowHint(int guess, int target);
    void ShowGameOver(int target);
}

// Модель настроек
public class GameSettings
{
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public int MaxAttempts { get; set; }
}

// Реализация генератора чисел
public class RandomNumberGenerator : INumberGenerator
{
    private readonly Random _random = new();
    public int Generate(int min, int max) => _random.Next(min, max + 1);
}

// Обработчики ввода/вывода
public class ConsoleInputHandler : IInputHandler
{
    public int GetGuess()
    {
        Console.Write("Ваша попытка: ");
        return int.Parse(Console.ReadLine()!);
    }
}

public class ConsoleOutputHandler : IOutputHandler
{
    public void ShowMessage(string message) => Console.WriteLine(message);
    
    public void ShowHint(int guess, int target)
    {
        Console.WriteLine(guess < target 
            ? "Загаданное число БОЛЬШЕ" 
            : "Загаданное число МЕНЬШЕ");
    }

    public void ShowGameOver(int target) => 
        Console.WriteLine($"Попытки закончились. Загаданное число: {target}");
}

// Логика игры
public class Game
{
    private readonly INumberGenerator _generator;
    private readonly IInputHandler _input;
    private readonly IOutputHandler _output;
    private readonly GameSettings _settings;

    public Game(
        INumberGenerator generator,
        IInputHandler input,
        IOutputHandler output,
        GameSettings settings)
    {
        _generator = generator;
        _input = input;
        _output = output;
        _settings = settings;
    }

    public void Play()
    {
        var target = _generator.Generate(
            _settings.MinNumber, 
            _settings.MaxNumber);

        _output.ShowMessage($"Угадайте число от {_settings.MinNumber} до {_settings.MaxNumber}. " +
                           $"У вас {_settings.MaxAttempts} попыток.");

        for (var attempt = 0; attempt < _settings.MaxAttempts; attempt++)
        {
            var guess = _input.GetGuess();

            if (guess == target)
            {
                _output.ShowMessage("Поздравляем! Вы угадали!");
                return;
            }

            _output.ShowHint(guess, target);
        }

        _output.ShowGameOver(target);
    }
}

// Настройки через консоль
public class ConsoleSettingsProvider : ISettingsProvider
{
    public GameSettings GetSettings()
    {
        Console.Write("Введите минимальное число: ");
        var min = int.Parse(Console.ReadLine()!);

        Console.Write("Введите максимальное число: ");
        var max = int.Parse(Console.ReadLine()!);

        Console.Write("Введите количество попыток: ");
        var attempts = int.Parse(Console.ReadLine()!);

        return new GameSettings
        {
            MinNumber = min,
            MaxNumber = max,
            MaxAttempts = attempts
        };
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        var settingsProvider = new ConsoleSettingsProvider();
        var settings = settingsProvider.GetSettings();

        var game = new Game(
            new RandomNumberGenerator(),
            new ConsoleInputHandler(),
            new ConsoleOutputHandler(),
            settings
        );

        game.Play();
    }
}
