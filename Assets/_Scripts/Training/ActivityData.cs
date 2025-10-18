namespace Data
{
    [System.Serializable]
    public class ActivityData
    {
        public string id;
        public string name;
        public string description;
        public System.DateTime date;
        public ActivityType type;
        public float distance;
        public System.TimeSpan duration;
        public string challengeId;
        public bool isCompleted;
    }

    [System.Serializable]
    public class ChallengeData
    {
        public string id;
        public string name;
        public string description;
        public System.DateTime startDate;
        public System.DateTime endDate;
        public bool isRegistered;
        public bool isInvited;
        public ChallengeType type;
    }

    public enum ActivityType
    {
        Running,
        Walking
    }
    public enum ChallengeType
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

    [System.Serializable]
    public class RouteData
    {
        public string id;
        public string name;
        public string description;
        public float distance;
        public RouteDifficulty difficulty;
        public ActivityType activityType;
        public RouteType routeType;
        // ... остальные поля
    }

    public enum RouteDifficulty { Easy, Medium, Hard, Expert }
    public enum RouteType { Individual, Group, Competition, AR }

    [System.Serializable]
    public class Checkpoint
    {
        public int number;
        public string name;
        public CheckpointType type;
        public bool arPreview;
    }

    public enum CheckpointType { Start, Finish, Sprint, Rest, Info, ARTask }

    [System.Serializable]
    public class PersonalBest
    {
        public System.TimeSpan bestTime;
        public float bestSpeed;
    }

    [System.Serializable]
    public class GroupStats
    {
        public float averageTime;
        public string ageGroup;
    }

    [System.Serializable]
    public class PerformanceMetrics
    {
        public float averageSpeed;
        public float maxSpeed;
        public int heartRate;
        public bool personalBest;
    }

    [System.Serializable]
    public class ActivityAttempt
    {
        public System.DateTime date;
        public System.TimeSpan duration;
        public float distance;
    }

    [System.Serializable]
    public class ChallengeInvitation
    {
        public string id;
        public string challengeId;
        public string challengeName;
        public System.DateTime startDate;
        public System.DateTime endDate;
        public string inviterName;
        public int participantsCount;
        public bool isAccepted;
    }
}