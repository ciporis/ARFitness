using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DayDetailsPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private Transform _activitiesContent;
    [SerializeField] private Transform _challengesContent;
    [SerializeField] private GameObject _activityItemPrefab;
    [SerializeField] private GameObject _challengeItemPrefab;
    [SerializeField] private Button _repeatButton;
    [SerializeField] private Button _planButton;
    [SerializeField] private Button _closeButton;

    private DateTime _currentDate;

    private void Start()
    {
        _closeButton.onClick.AddListener(HidePanel);
        _repeatButton.onClick.AddListener(OnRepeatClicked);
        _planButton.onClick.AddListener(OnPlanClicked);

        CalendarManager.OnDateSelected += OnDateSelected;
        _panel.SetActive(false);
    }

    private void OnDestroy()
    {
        CalendarManager.OnDateSelected -= OnDateSelected;
    }

    private void OnDateSelected(DateTime date)
    {
        _currentDate = date;
        ShowDayDetails(date);
    }

    private void ShowDayDetails(DateTime date)
    {
        _panel.SetActive(true);
        _dateText.text = date.ToString("dd MMMM yyyy");

        // Очищаем контент
        ClearContent(_activitiesContent);
        ClearContent(_challengesContent);

        // Загружаем активности
        var activities = CalendarManager.Instance.GetActivitiesForDate(date);
        foreach (var activity in activities)
        {
            CreateActivityItem(activity);
        }

        // Загружаем челленджи
        var challenges = CalendarManager.Instance.GetChallengesForDate(date);
        foreach (var challenge in challenges)
        {
            CreateChallengeItem(challenge);
        }

        // Настраиваем видимость кнопок
        bool hasCompletedActivities = activities.Any(a => a.isCompleted);
        _repeatButton.gameObject.SetActive(hasCompletedActivities);
        _planButton.gameObject.SetActive(date >= DateTime.Today);
    }

    private void CreateActivityItem(ActivityData activity)
    {
        var itemGO = Instantiate(_activityItemPrefab, _activitiesContent);
        var item = itemGO.GetComponent<ActivityListItem>();

        if (item != null)
        {
            item.Initialize(activity);
        }
    }

    private void CreateChallengeItem(ChallengeData challenge)
    {
        var itemGO = Instantiate(_challengeItemPrefab, _challengesContent);
        var item = itemGO.GetComponent<ChallengeListItem>();

        if (item != null)
        {
            item.Initialize(challenge);
        }
    }

    private void ClearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnRepeatClicked()
    {
        var activities = CalendarManager.Instance.GetActivitiesForDate(_currentDate)
            .Where(a => a.isCompleted).ToList();

        if (activities.Count > 0)
        {
            // Предлагаем выбрать активность для повторения
            var activity = activities[0]; // Берем первую завершенную
            CalendarManager.Instance.RepeatActivity(activity.id);
        }
    }

    private void OnPlanClicked()
    {
        // Открываем окно планирования тренировки
        FindObjectOfType<PlanningPanel>()?.ShowForDate(_currentDate);
    }

    private void HidePanel()
    {
        _panel.SetActive(false);
    }
}