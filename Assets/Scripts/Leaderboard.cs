using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Needed for Dropdown

[Serializable]
public class UserScore
{
    public string username;
    public int money;
    public int enemiesDefeated;
    public int totalTime;
    public int distanceRan;
}

[Serializable]
public class ScoreData
{
    public List<UserScore> score;
}

public class Leaderboard : MonoBehaviour
{
    public ScoreData scoreData;
    public TextMeshProUGUI[] leaderboardTexts; // Array to hold the 5 TextMeshProUGUI components
    public TMP_Dropdown categoryDropdown; // Dropdown menu for category selection

    private void Start()
    {
        LoadScoresFromFile("Score"); // Use the name without file extension
        // Add listener for dropdown selection changes
        categoryDropdown.onValueChanged.AddListener(OnCategoryChanged);
    }

    private void LoadScoresFromFile(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName); // Load the JSON file

        if (jsonFile != null)
        {
            LoadScores(jsonFile.text); // Call your existing LoadScores method
        }
        else
        {
            Debug.LogError("File not found in Resources: " + fileName);
        }
    }

    // Method to load your JSON data into scoreData
    public void LoadScores(string json)
    {
        scoreData = JsonUtility.FromJson<ScoreData>(json);
        UpdateLeaderboard("Most Enemies Defeated"); // Default category
    }

    // Method to update the leaderboard based on selected category
    private void OnCategoryChanged(int index)
    {
        string selectedCategory = categoryDropdown.options[index].text;
        UpdateLeaderboard(selectedCategory);
    }

    private void UpdateLeaderboard(string criteria)
    {
        // Accumulate scores for each user
        var accumulatedScores = new Dictionary<string, UserScore>();

        foreach (var userScore in scoreData.score)
        {
            if (!accumulatedScores.ContainsKey(userScore.username))
            {
                accumulatedScores[userScore.username] = new UserScore
                {
                    username = userScore.username,
                    money = userScore.money,
                    enemiesDefeated = userScore.enemiesDefeated,
                    totalTime = userScore.totalTime,
                    distanceRan = userScore.distanceRan
                };
            }
            else
            {
                accumulatedScores[userScore.username].money += userScore.money;
                accumulatedScores[userScore.username].enemiesDefeated += userScore.enemiesDefeated;
                accumulatedScores[userScore.username].totalTime += userScore.totalTime;
                accumulatedScores[userScore.username].distanceRan += userScore.distanceRan;
            }
        }

        // Sort users based on the chosen criteria
        List<UserScore> sortedUsers = SortScores(accumulatedScores, criteria);

        // Update the TextMeshProUGUI components with the top 5 users
        for (int i = 0; i < leaderboardTexts.Length; i++)
        {
            if (i < sortedUsers.Count)
            {
                leaderboardTexts[i].text = $"{sortedUsers[i].username} - {GetScoreValue(sortedUsers[i], criteria)}";
            }
            else
            {
                leaderboardTexts[i].text = "-"; // Placeholder if less than 5 users
            }
        }
    }

    private List<UserScore> SortScores(Dictionary<string, UserScore> accumulatedScores, string criteria)
    {
        switch (criteria.ToLower())
        {
            case "most gold":
                return accumulatedScores.Values.OrderByDescending(u => u.money).ToList();
            case "most enemies defeated":
                return accumulatedScores.Values.OrderByDescending(u => u.enemiesDefeated).ToList();
            case "longest time played":
                return accumulatedScores.Values.OrderByDescending(u => u.totalTime).ToList();
            case "furthest distance travelled":
                return accumulatedScores.Values.OrderByDescending(u => u.distanceRan).ToList();
            default:
                throw new ArgumentException("Invalid criteria: " + criteria);
        }
    }

    private string GetScoreValue(UserScore userScore, string criteria)
    {
        switch (criteria.ToLower())
        {
            case "most gold":
                return userScore.money.ToString();
            case "most enemies defeated":
                return userScore.enemiesDefeated.ToString();
            case "longest time played":
                // Convert total time from seconds to minutes
                float minutesPlayed = userScore.totalTime / 60f;
                return $"{minutesPlayed:F2} min"; // Format to 2 decimal places
            case "furthest distance travelled":
                // Convert distanceRan to meters and format
                float distanceInMeters = userScore.distanceRan / 1000f;
                return $"{distanceInMeters:F2} m"; // Format to 2 decimal places
            default:
                return "0"; // Default case
        }
    }
}
