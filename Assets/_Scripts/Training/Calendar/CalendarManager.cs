using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private ActivityFilter _currentFilter = ActivityFilter.All;
    public enum ActivityFilter
    {
        All,
        MyTrainings,
        Competitions
    }
    private List<ActivityData> _allActivities = new List<ActivityData>();
    private List<ChallengeData> _allChallenges = new List<ChallengeData>();
    private DateTime _currentMonth;
    private DateTime _selectedDate;

    public static event Action<DateTime> OnMonthChanged;
    public static event Action<DateTime> OnDateSelected;
    public static event Action<ActivityFilter> OnFilterChanged;
    public static event Action<List<ActivityData>> OnActivitiesUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _currentMonth = DateTime.Today;
            _selectedDate = DateTime.Today;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Public Methods

    public void SetFilter(ActivityFilter filter)
    {
        _currentFilter = filter;
        OnFilterChanged?.Invoke(filter);
        UpdateCalendarDisplay();
    }

    public void SelectDate(DateTime date)
    {
        _selectedDate = date;
        OnDateSelected?.Invoke(date);
    }

    public void NavigateMonth(int direction)
    {
        _currentMonth = _currentMonth.AddMonths(direction);
        OnMonthChanged?.Invoke(_currentMonth);
        UpdateCalendarDisplay();
    }

    public void GoToToday()
    {
        _currentMonth = DateTime.Today;
        _selectedDate = DateTime.Today;
        OnMonthChanged?.Invoke(_currentMonth);
        OnDateSelected?.Invoke(_selectedDate);
    }

    public void AddActivity(ActivityData activity)
    {
        _allActivities.Add(activity);
        OnActivitiesUpdated?.Invoke(GetFilteredActivities());
        UpdateCalendarDisplay();
    }

    public void RegisterForChallenge(string challengeId)
    {
        var challenge = _allChallenges.Find(c => c.id == challengeId);
        if (challenge != null)
        {
            challenge.isRegistered = true;
            Debug.Log($"Зарегистрировались на челлендж: {challenge.name}");
        }
    }

    public void RepeatActivity(string activityId)
    {
        var originalActivity = _allActivities.Find(a => a.id == activityId);
        if (originalActivity != null)
        {
            var newActivity = new ActivityData
            {
                id = Guid.NewGuid().ToString(),
                name = $"Повтор: {originalActivity.name}",
                date = DateTime.Today,
                type = originalActivity.type,
                distance = originalActivity.distance,
                duration = originalActivity.duration,
                isCompleted = false
            };

            AddActivity(newActivity);

            ModalManager.Instance.ShowConfirmModal(
                "Тренировка запланирована на сегодня! Хотите установить напоминание?",
                onConfirm: () => Debug.Log("Напоминание установлено"),
                confirmText: "Да",
                cancelText: "Нет"
            );
        }
    }

    #endregion

    #region Data Getters

    public List<ActivityData> GetActivitiesForDate(DateTime date)
    {
        return _allActivities.FindAll(a => a.date.Date == date.Date);
    }

    public List<ChallengeData> GetChallengesForDate(DateTime date)
    {
        return _allChallenges.FindAll(c =>
            date.Date >= c.startDate.Date && date.Date <= c.endDate.Date);
    }

    public List<ActivityData> GetFilteredActivities()
    {
        switch (_currentFilter)
        {
            case ActivityFilter.MyTrainings:
                return _allActivities.FindAll(a => string.IsNullOrEmpty(a.challengeId));
            case ActivityFilter.Competitions:
                return _allActivities.FindAll(a => !string.IsNullOrEmpty(a.challengeId));
            default:
                return _allActivities;
        }
    }

    public bool HasActivitiesOnDate(DateTime date)
    {
        return GetActivitiesForDate(date).Count > 0;
    }

    public CalendarDayData GetDayData(DateTime date)
    {
        var activities = GetActivitiesForDate(date);
        var challenges = GetChallengesForDate(date);

        return new CalendarDayData
        {
            date = date,
            activities = activities,
            challenges = challenges,
            hasActivities = activities.Count > 0,
            hasChallenges = challenges.Count > 0,
            isToday = date.Date == DateTime.Today,
            isSelected = date.Date == _selectedDate.Date
        };
    }

    #endregion

    #region Private Methods

    private void UpdateCalendarDisplay()
    {
        OnActivitiesUpdated?.Invoke(GetFilteredActivities());
    }

    #endregion

    #region Test Data Methods

    [ContextMenu("Add Test Activities")]
    public void AddTestActivities()
    {
        _allActivities.AddRange(new List<ActivityData>
        {
            new ActivityData
            {
                id = "test_1",
                name = "Утренняя пробежка",
                date = DateTime.Today.AddDays(-1),
                type = ActivityType.Running,
                distance = 5.2f,
                duration = TimeSpan.FromMinutes(28),
                isCompleted = true
            },
            new ActivityData
            {
                id = "test_2",
                name = "Соревнование по бегу",
                date = DateTime.Today,
                type = ActivityType.Running,
                distance = 10.0f,
                duration = TimeSpan.FromMinutes(55),
                isCompleted = true,
                challengeId = "challenge_1"
            }
        });

        _allChallenges.AddRange(new List<ChallengeData>
        {
            new ChallengeData
            {
                id = "challenge_1",
                name = "Осенний марафон",
                description = "Пробеги 50км за неделю",
                startDate = DateTime.Today.AddDays(-7),
                endDate = DateTime.Today.AddDays(7),
                isRegistered = false
            }
        });

        UpdateCalendarDisplay();
        Debug.Log("Test data added!");
    }

    [ContextMenu("Clear All Data")]
    public void ClearAllData()
    {
        _allActivities.Clear();
        _allChallenges.Clear();
        UpdateCalendarDisplay();
        Debug.Log("All data cleared!");
    }

    #endregion
}

[System.Serializable]
public class CalendarDayData
{
    public DateTime date;
    public List<ActivityData> activities;
    public List<ChallengeData> challenges;
    public bool hasActivities;
    public bool hasChallenges;
    public bool isToday;
    public bool isSelected;
}