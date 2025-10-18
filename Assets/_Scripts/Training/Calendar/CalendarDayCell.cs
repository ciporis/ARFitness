using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CalendarDayCell : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button _cellButton;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private Image _background;
    [SerializeField] private Image _activityIndicator;
    [SerializeField] private Image _challengeIndicator;
    [SerializeField] private GameObject _todayBadge;

    [Header("Colors")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _todayColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color _selectedColor = new Color(0.3f, 0.8f, 0.3f);
    [SerializeField] private Color _activityColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private Color _challengeColor = new Color(1f, 0.8f, 0.2f);

    public DateTime Date { get; private set; }
    public bool IsEmpty { get; private set; }

    private void Start()
    {
        _cellButton.onClick.AddListener(OnCellClicked);
    }

    public void Initialize(CalendarDayData dayData)
    {
        Date = dayData.date;
        IsEmpty = false;

        _dayText.text = dayData.date.Day.ToString();
        _dayText.gameObject.SetActive(true);

        UpdateAppearance(dayData);
    }

    public void UpdateAppearance(CalendarDayData dayData)
    {
        _background.color = _normalColor;
        _todayBadge.SetActive(false);

        if (dayData.isToday)
        {
            _background.color = _todayColor;
            _todayBadge.SetActive(true);
        }

        if (dayData.isSelected)
        {
            _background.color = _selectedColor;
        }

        _activityIndicator.gameObject.SetActive(dayData.hasActivities);
        _challengeIndicator.gameObject.SetActive(dayData.hasChallenges);

        if (dayData.hasActivities)
        {
            _activityIndicator.color = GetActivityTypeColor(dayData.activities);
        }

        if (dayData.hasChallenges)
        {
            _challengeIndicator.color = _challengeColor;
        }
    }

    public void SetAsEmpty()
    {
        IsEmpty = true;
        _dayText.gameObject.SetActive(false);
        _activityIndicator.gameObject.SetActive(false);
        _challengeIndicator.gameObject.SetActive(false);
        _todayBadge.SetActive(false);
        _cellButton.interactable = false;
    }

    private void OnCellClicked()
    {
        if (!IsEmpty)
        {
            CalendarManager.Instance.SelectDate(Date);
        }
    }

    private Color GetActivityTypeColor(List<ActivityData> activities)
    {
        if (activities.Count == 0) return _activityColor;

        var mainActivity = activities[0];
        return mainActivity.type switch
        {
            ActivityType.Running => new Color(1f, 0.2f, 0.2f),
            ActivityType.Walking => new Color(0.3f, 0.8f, 0.3f),
            _ => _activityColor
        };
    }
}