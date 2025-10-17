using System;
using UnityEngine;

[System.Serializable]
public class ActivityData
{
    public string id;
    public string name;
    public string description;
    public DateTime date;
    public ActivityType type;
    public float distance;
    public TimeSpan duration;
    public int calories;
    public int points;
    public string routeId;
    public string[] participants;
    public ChallengeData challenge;
    public bool isCompleted;

    public enum ActivityType
    {
        Running,
        Cycling,
        Swimming,
        Gym,
        Yoga,
        Walking
    }
}

[System.Serializable]
public class ChallengeData
{
    public string id;
    public string name;
    public string description;
    public DateTime startDate;
    public DateTime endDate;
    public ChallengeType type;
    public string[] participants;
    public bool isInvited;
    public bool isRegistered;

    public enum ChallengeType
    {
        Personal,
        Competition,
        Team
    }
}