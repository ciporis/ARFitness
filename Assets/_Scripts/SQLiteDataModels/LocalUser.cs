using Firebase.Firestore;
using SQLite;
using System.Collections.Generic;

public class LocalUser
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
    public UserStats Stats { get; set; }
    public UserGameProgress GameProgress { get; set; }
    public UserSettings Settings { get; set; }
    public Timestamp CreatedAt { get; set; }
    public Timestamp LastLogin { get; set; }
    public string Role { get; set; }
}

public class LocalUserProfile
{
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Level { get; set; }
    public string Experience { get; set; }
    public List<string> Goals { get; set; }
    public string ParentEmail { get; set; }
}