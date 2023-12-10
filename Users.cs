using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

public class User
{
    public string Name { get; set; }
    public int CharactersPerMinute { get; set; }
    public int CharactersPerSecond { get; set; }
}

public static class Leaderboard
{
    private const string leaderboardFilePath = "leaderboard.json";
    private static List<User> leaderboard;

    static Leaderboard()
    {
        leaderboard = LoadLeaderboard();
    }

    public static void AddUser(User user)
    {
        leaderboard.Add(user);
        SaveLeaderboard();
    }

    public static List<User> GetTopUsers()
    {
        return leaderboard.OrderByDescending(u => u.CharactersPerMinute).ToList();
    }

    private static void SaveLeaderboard()
    {
        string json = JsonSerializer.Serialize(leaderboard);
        File.WriteAllText(leaderboardFilePath, json);
    }

    private static List<User> LoadLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string json = File.ReadAllText(leaderboardFilePath);
            return JsonSerializer.Deserialize<List<User>>(json);
        }
        else
        {
            return new List<User>();
        }
    }
}

