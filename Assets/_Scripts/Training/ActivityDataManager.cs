using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivityDataManager : MonoBehaviour
{
    public static ActivityDataManager Instance { get; private set; }

    private List<ActivityData> _activities = new List<ActivityData>();
    private List<ChallengeData> _challenges = new List<ChallengeData>();

    public static event Action<ActivityData> OnActivityAdded;
    public static event Action<ChallengeData> OnChallengeJoined;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateTestData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Generate Test Data")]
    private void GenerateTestData()
    {
        GenerateTestChallenges();
    }

    private void GenerateTestChallenges()
    {
        _challenges = new List<ChallengeData>
        {
            new ChallengeData
            {
                id = "challenge_1",
                name = "������� �������",
                description = "������� 50�� �� ������",
                startDate = DateTime.Today.AddDays(7),
                endDate = DateTime.Today.AddDays(14),
                isRegistered = false
            }
        };
    }

    public void JoinChallenge(string challengeId)
    {
        var challenge = _challenges.FirstOrDefault(c => c.id == challengeId);
        if (challenge != null)
        {
            challenge.isRegistered = true;
            OnChallengeJoined?.Invoke(challenge);
        }
    }

    public void AddActivity(ActivityData activity)
    {
        _activities.Add(activity);
        OnActivityAdded?.Invoke(activity);
    }

    public ChallengeData GetChallengeById(string challengeId)
    {
        return _challenges.FirstOrDefault(c => c.id == challengeId);
    }

    public List<ActivityData> GetActivitiesForDate(DateTime date, ActivityFilter filter = ActivityFilter.All)
    {
        var activities = _activities.Where(a => a.date.Date == date.Date);

        return filter switch
        {
            ActivityFilter.MyTrainings => activities.Where(a => string.IsNullOrEmpty(a.challengeId)).ToList(),
            ActivityFilter.Competitions => activities.Where(a => !string.IsNullOrEmpty(a.challengeId)).ToList(),
            _ => activities.ToList()
        };
    }
}