using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public struct User
{
    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string DisplayName { get; set; }

    [FirestoreProperty]
    public string AvatarUrl { get; set; }

    [FirestoreProperty]
    public UserProfile Profile { get; set; }

    [FirestoreProperty]
    public UserStats Stats { get; set; }

    [FirestoreProperty]
    public UserGameProgress GameProgress { get; set; }

    [FirestoreProperty]
    public UserSettings Settings { get; set; }

    [FirestoreProperty]
    public Timestamp CreatedAt { get; set; }

    [FirestoreProperty]
    public Timestamp LastLogin { get; set; }

    [FirestoreProperty]
    public string Role { get; set; }
}

[FirestoreData]
public struct UserProfile
{
    [FirestoreProperty]
    public int Age { get; set; }

    [FirestoreProperty]
    public string Gender { get; set; }

    [FirestoreProperty]
    public string Level { get; set; }

    [FirestoreProperty]
    public string Experience { get; set; }

    [FirestoreProperty]
    public List<string> Goals { get; set; }

    [FirestoreProperty]
    public string ParentEmail { get; set; }
}

[FirestoreData]
public struct UserStats
{
    [FirestoreProperty]
    public int TotalPoints { get; set; }

    [FirestoreProperty]
    public int TotalXP { get; set; }

    [FirestoreProperty]
    public int CompletedRoutes { get; set; }

    [FirestoreProperty]
    public double TotalDistance { get; set; }

    [FirestoreProperty]
    public double BestPromptScore { get; set; }

    [FirestoreProperty]
    public int StreakDays { get; set; }
}

[FirestoreData]
public struct UserGameProgress
{
    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public int Currency { get; set; }

    [FirestoreProperty]
    public List<string> UnlockedSkins { get; set; }

    [FirestoreProperty]
    public List<string> Achievements { get; set; }
}

[FirestoreData]
public struct UserSettings
{
    [FirestoreProperty]
    public bool Notifications { get; set; }

    [FirestoreProperty]
    public string Privacy { get; set; }

    [FirestoreProperty]
    public string Language { get; set; }
}

public static class UserConstants
{
    public static class Gender
    {
        public const string Male = "male";
        public const string Female = "female";
        public const string Other = "other";
        public const string NotSpecified = "not_specified";
    }

    public static class Level
    {
        public const string Beginner = "beginner";
        public const string Intermediate = "intermediate";
        public const string Advanced = "advanced";
        public const string Athlete = "athlete";
    }

    public static class Experience
    {
        public const string Section = "section";
        public const string RegularTraining = "regular_training";
        public const string PhysicalEducation = "pe";
        public const string None = "none";
    }

    public static class Goals
    {
        public const string WeightLoss = "weight_loss";
        public const string Endurance = "endurance";
        public const string Strength = "strength";
        public const string Flexibility = "flexibility";
        public const string GeneralFitness = "general_fitness";
    }

    public static class Privacy
    {
        public const string Public = "public";
        public const string Friends = "friends";
        public const string Private = "private";
    }

    public static class Role
    {
        public const string Participant = "participant";
        public const string Trainer = "trainer";
        public const string Judge = "judge";
        public const string Admin = "admin";
    }
}