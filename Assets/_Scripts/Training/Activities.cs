using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Activities : MonoBehaviour
{
    [SerializeField] private List<ActivityData> _data;
    public List<ActivityData> Data { get { return _data; } set { _data = value; } }

    [Header("UI Reference")]
    [SerializeField] private Transform _activitiesContent;
    [SerializeField] private GameObject _activityCardPrefab;

    private void Start()
    {
        if (_data == null || _data.Count == 0)
        {
            LoadTestActivities();
        }

        RefreshActivitiesList();
    }

    public void RegisterForActivity(ActivityData activityData, DateTime selectedDate)
    {
        var registeredActivity = new ActivityData
        {
            id = Guid.NewGuid().ToString(),
            name = activityData.name,
            description = activityData.description,
            date = selectedDate,
            type = activityData.type,
            distance = activityData.distance,
            duration = activityData.duration,
            isCompleted = false
        };

        CalendarManager.Instance.AddActivity(registeredActivity);

        Debug.Log($"Активность '{activityData.name}' записана на {selectedDate:dd.MM.yyyy}");
    }

    [ContextMenu("Load Test Activities")]
    private void LoadTestActivities()
    {
        _data = new List<ActivityData>
        {
            new ActivityData
            {
                id = "activity_1",
                name = "Утренняя пробежка 5км",
                description = "Легкая пробежка по парку",
                type = ActivityType.Running,
                distance = 5.0f,
                duration = new TimeSpan(0, 30, 0)
            },
            new ActivityData
            {
                id = "activity_2",
                name = "Вечерняя прогулка",
                description = "Спокойная прогулка",
                type = ActivityType.Walking,
                distance = 3.0f,
                duration = new TimeSpan(0, 45, 0)
            },
            new ActivityData
            {
                id = "activity_3",
                name = "Осенний марафон",
                description = "Челлендж: пробеги 50км",
                type = ActivityType.Running,
                distance = 50.0f,
                duration = new TimeSpan(7, 0, 0, 0),
                challengeId = "challenge_1"
            }
        };
    }

    [ContextMenu("Refresh Activities List")]
    public void RefreshActivitiesList()
    {
        foreach (Transform child in _activitiesContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var activity in _data)
        {
            var cardGO = Instantiate(_activityCardPrefab, _activitiesContent);
            var card = cardGO.GetComponent<CardActivity>();

            if (card != null)
            {
                card.Initialize(activity);
            }
        }
    }
}