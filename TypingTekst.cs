using System.Diagnostics;
using System.Text;

public class TypingTest
{
    private Stopwatch stopwatch;
    private string textToType;
    private StringBuilder typedText;
    private bool isTimerRunning;
    private bool isTypingFinished;

    public void StartTest()
{
    Console.Write("Enter your name: ");
    string name = Console.ReadLine();

    stopwatch = new Stopwatch();
    typedText = new StringBuilder();

    SetupTimer();
    GenerateTextToType();

    Console.WriteLine("Type the following text:");
    Console.WriteLine(textToType);

    StartTyping();

    while (isTimerRunning && !isTypingFinished)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        HandleKeyPress(keyInfo);
    }

    stopwatch.Stop();

    int charactersTyped = typedText.ToString().Length;
    double minutes = stopwatch.Elapsed.TotalMinutes;

    int charactersPerMinute = (int)(charactersTyped / minutes);
    int charactersPerSecond = (int)(charactersTyped / stopwatch.Elapsed.TotalSeconds);

    User user = new User
    {
        Name = name,
        CharactersPerMinute = charactersPerMinute,
        CharactersPerSecond = charactersPerSecond
    };

    Leaderboard.AddUser(user);

    PrintLeaderboard();
}

private void GenerateTextToType()
{
    textToType = "Новый тариф от МТС - «похмельный» - платишь только за те звонки, которые помнишь.";
}

private void SetupTimer()
{
    isTimerRunning = true;
    isTypingFinished = false;

    stopwatch.Start();

    var timerThread = new System.Threading.Thread(() =>
    {
        while (stopwatch.Elapsed.TotalMinutes < 1 && !isTypingFinished)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Time elapsed: {stopwatch.Elapsed:mm\\:ss}");
        }

        isTimerRunning = false;
    });

    timerThread.Start();
}

private void StartTyping()
{
    Console.SetCursorPosition(0, 2);

    while (!isTypingFinished)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        HandleKeyPress(keyInfo);
    }
}

private void HandleKeyPress(ConsoleKeyInfo keyInfo)
{
    if (keyInfo.Key == ConsoleKey.Enter)
    {
        isTypingFinished = true;
    }
    else if (!char.IsControl(keyInfo.KeyChar))
    {
        typedText.Append(keyInfo.KeyChar);
    }
}

private void PrintLeaderboard()
{
    Console.WriteLine("Leaderboard:");

    List<User> topUsers = Leaderboard.GetTopUsers();

    foreach (User user in topUsers)
    {
        Console.WriteLine($"{user.Name}, Characters per minute: {user.CharactersPerMinute}, Characters per second: {user.CharactersPerSecond}");
    }
}
}
