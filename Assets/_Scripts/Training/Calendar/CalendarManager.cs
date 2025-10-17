using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private ActivityFilter _currentFilter = ActivityFilter.All;

    private List<ActivityData> _allActivities = new List<ActivityData>();
    private List<ChallengeData> _allChallenges = new List<ChallengeData>();
    private DateTime _currentMonth;
    private DateTime _selectedDate;

    public static event Action<DateTime> OnMonthChanged;
    public static event Action<DateTime> OnDateSelected;
    public static event Action<ActivityFilter> OnFilterChanged;
    public static event Action<List<ActivityData>> OnActivitiesUpdated;

    public enum ActivityFilter  
    {
        All,
        MyTrainings,
        Competitions
    }

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
            ScheduleNotification(challenge);
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
                routeId = originalActivity.routeId,
                isCompleted = false
            };

            AddActivity(newActivity);

            ModalManager.Instance.ShowConfirmModal(
                "Тренировка запланирована на сегодня! Хотите установить напоминание?",
                onConfirm: () => ScheduleActivityReminder(newActivity)
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
                return _allActivities.FindAll(a => a.challenge == null);
            case ActivityFilter.Competitions:
                return _allActivities.FindAll(a => a.challenge != null);
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

    private void ScheduleNotification(ChallengeData challenge)
    {
        Debug.Log($"Уведомление запланировано для челленджа: {challenge.name}");

#if UNITY_ANDROID || UNITY_IOS
        // ScheduleLocalNotification(challenge);
#endif
    }

    private void ScheduleActivityReminder(ActivityData activity)
    {
        Debug.Log($"Напоминание установлено для: {activity.name}");
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