using System;
using System.Collections.Generic;
using UnityEngine;

public class Activities : MonoBehaviour
{
    [SerializeField] private List<ActivityData> _activitiesData;
    [SerializeField] private List<ChallengeData> _challengesData;

    public List<ActivityData> ActivitiesData { get => _activitiesData; set => _activitiesData = value; }
    public List<ChallengeData> ChallengesData { get => _challengesData; set => _challengesData = value; }

    [Header("UI Reference")]
    [SerializeField] private Transform _activitiesContent;
    [SerializeField] private GameObject _activityCardPrefab;

    private void Start()
    {
        if ((_activitiesData == null || _activitiesData.Count == 0) &&
            (_challengesData == null || _challengesData.Count == 0))
        {
            LoadTestData();
        }

        RefreshActivitiesList();
    }

    [ContextMenu("Load Test Data")]
    private void LoadTestData()
    {
        _activitiesData = new List<ActivityData>
        {
            new ActivityData
            {
                id = "activity_1",
                name = "Утренняя пробежка 5км",
                description = "Легкая пробежка по парку",
                type = ActivityType.Running,
                date = DateTime.Parse("12.12.2025"),
                distance = 5.0f,
                duration = new System.TimeSpan(0, 30, 0)
            },
            new ActivityData
            {
                id = "activity_2",
                name = "Вечерняя прогулка",
                description = "Спокойная прогулка",
                type = ActivityType.Walking,
                distance = 3.0f,
                date = DateTime.Parse("12.12.2025"),
                duration = new System.TimeSpan(0, 45, 0)
            }
        };

        _challengesData = new List<ChallengeData>
        {
            new ChallengeData
            {
                id = "challenge_1",
                name = "Осенний марафон",
                description = "Пробеги 50км за неделю"
            }
        };
    }

    [ContextMenu("Refresh Activities List")]
    public void RefreshActivitiesList()
    {
        if (_activitiesContent == null || _activityCardPrefab == null)
        {
            Debug.LogError("UI references not assigned!");
            return;
        }

        foreach (Transform child in _activitiesContent)
        {
            Destroy(child.gameObject);
        }

        if (_activitiesData != null)
        {
            foreach (var activity in _activitiesData)
            {
                CreateCard(activity);
            }
        }

        if (_challengesData != null)
        {
            foreach (var challenge in _challengesData)
            {
                CreateCard(challenge);
            }
        }
    }

    private void CreateCard(ActivityData activityData)
    {
        var cardGO = Instantiate(_activityCardPrefab, _activitiesContent);
        var card = cardGO.GetComponent<CardActivity>();

        if (card != null)
        {
            card.Initialize(activityData);
        }
        else
        {
            Debug.LogError("CardActivity component not found on prefab!");
        }
    }

    private void CreateCard(ChallengeData challengeData)
    {
        var cardGO = Instantiate(_activityCardPrefab, _activitiesContent);
        var card = cardGO.GetComponent<CardActivity>();

        if (card != null)
        {
            card.Initialize(challengeData);
        }
        else
        {
            Debug.LogError("CardActivity component not found on prefab!");
        }
    }

    public void RegisterForActivity(ActivityData activityData, System.DateTime selectedDate)
    {
        var registeredActivity = new ActivityData
        {
            id = System.Guid.NewGuid().ToString(),
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
}