using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActivityData
{
    public int Id;
    public string Name;
    public string Description;
    public string Date;
}

public class Activities : MonoBehaviour
{
    [SerializeField] private List<ActivityData> activities;

    public void Add(ActivityData activityData)
    {
        if (activities == null) return;
        activities.Add(activityData);
    }
    public void Remove(ActivityData activityData) 
    { 
        activities.Remove(activityData); 
    }
}
