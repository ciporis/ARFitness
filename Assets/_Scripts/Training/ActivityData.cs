using System;
using System.Collections.Generic;

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
    public string challengeId;
    public bool isCompleted;
}

[System.Serializable]
public class ChallengeData
{
    public string id;
    public string name;
    public string description;
    public ActivityType type;
    public float distance;
    public TimeSpan duration;
    public string challengeId;
    public bool isCompleted;
    public bool isRegistered;
}

public enum ActivityType
{
    Running,
    Walking
}

public enum ActivityFilter
{
    All,
    MyTrainings,
    Competitions
}