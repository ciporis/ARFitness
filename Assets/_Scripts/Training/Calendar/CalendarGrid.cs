using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarGrid : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _monthYearText;
    [SerializeField] private Transform _daysGridParent;
    [SerializeField] private GameObject _dayCellPrefab;
    [SerializeField] private Button _prevMonthButton;
    [SerializeField] private Button _nextMonthButton;
    [SerializeField] private Button _todayButton;

    [Header("Settings")]
    [SerializeField] private Color _todayColor = Color.blue;
    [SerializeField] private Color _selectedColor = Color.green;
    [SerializeField] private Color _activityColor = Color.red;
    [SerializeField] private Color _challengeColor = Color.yellow;

    private List<CalendarDayCell> _dayCells = new List<CalendarDayCell>();
    private DateTime _currentMonth;
    private bool _isInitialized = false;

    private void Start()
    {
        InitializeCalendar();
    }

    private void InitializeCalendar()
    {
        if (!CheckReferences())
        {
            Debug.LogError("CalendarGrid: Not all references are assigned in inspector!");
            return;
        }

        _prevMonthButton.onClick.RemoveAllListeners();
        _nextMonthButton.onClick.RemoveAllListeners();
        _todayButton.onClick.RemoveAllListeners();

        _prevMonthButton.onClick.AddListener(() => OnNavigateMonth(-1));
        _nextMonthButton.onClick.AddListener(() => OnNavigateMonth(1));
        _todayButton.onClick.AddListener(OnTodayClicked);

        CalendarManager.OnMonthChanged += OnMonthChanged;
        CalendarManager.OnDateSelected += OnDateSelected;
        CalendarManager.OnFilterChanged += OnFilterChanged;

        _currentMonth = DateTime.Today;
        GenerateCalendarGrid();
        _isInitialized = true;
    }

    private void OnNavigateMonth(int direction)
    {
        if (!_isInitialized) return;
        CalendarManager.Instance.NavigateMonth(direction);
    }

    private void OnTodayClicked()
    {
        if (!_isInitialized) return;
        CalendarManager.Instance.GoToToday();
    }

    private bool CheckReferences()
    {
        bool allAssigned = true;

        if (_monthYearText == null)
        {
            Debug.LogError("MonthYearText is not assigned!", this);
            allAssigned = false;
        }
        if (_daysGridParent == null)
        {
            Debug.LogError("DaysGridParent is not assigned!", this);
            allAssigned = false;
        }
        if (_dayCellPrefab == null)
        {
            Debug.LogError("DayCellPrefab is not assigned!", this);
            allAssigned = false;
        }

        return allAssigned;
    }

    private void OnDestroy()
    {
        CalendarManager.OnMonthChanged -= OnMonthChanged;
        CalendarManager.OnDateSelected -= OnDateSelected;
        CalendarManager.OnFilterChanged -= OnFilterChanged;
    }

    private void GenerateCalendarGrid()
    {
        if (_daysGridParent == null || _dayCellPrefab == null)
        {
            Debug.LogError("Cannot generate calendar: missing references");
            return;
        }

        ClearCalendarGrid();

        _monthYearText.text = GetMonthYearText(_currentMonth);

        DateTime firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
        int startDay = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

        Debug.Log($"Generating calendar: {_currentMonth:MMMM yyyy}, days: {daysInMonth}, start day: {startDay}");

        for (int i = 0; i < startDay; i++)
        {
            CreateEmptyDayCell();
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            DateTime cellDate = new DateTime(_currentMonth.Year, _currentMonth.Month, day);
            CreateDayCell(cellDate);
        }

        Debug.Log($"Calendar generated: {daysInMonth} days, {_dayCells.Count} cells total");
    }

    private void ClearCalendarGrid()
    {
        for (int i = _daysGridParent.childCount - 1; i >= 0; i--)
        {
            Transform child = _daysGridParent.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                Destroy(child.gameObject);
            }
        }

        _dayCells.Clear();

        Debug.Log($"Cleared calendar grid. Remaining children: {_daysGridParent.childCount}");
    }

    private string GetMonthYearText(DateTime date)
    {
        string[] monthNames = new string[]
        {
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
            "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };

        return $"{monthNames[date.Month - 1]} {date.Year}";
    }

    private void CreateDayCell(DateTime date)
    {
        if (_dayCellPrefab == null || _daysGridParent == null) return;

        try
        {
            var dayCellGO = Instantiate(_dayCellPrefab, _daysGridParent);
            dayCellGO.name = $"DayCell_{date:dd}";

            var dayCell = dayCellGO.GetComponent<CalendarDayCell>();

            if (dayCell != null)
            {
                var dayData = CalendarManager.Instance.GetDayData(date);
                dayCell.Initialize(dayData);
                _dayCells.Add(dayCell);
            }
            else
            {
                Debug.LogError("CalendarDayCell component not found on prefab!");
                Destroy(dayCellGO);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating day cell for {date}: {e.Message}");
        }
    }

    private void CreateEmptyDayCell()
    {
        if (_dayCellPrefab == null || _daysGridParent == null) return;

        try
        {
            var emptyCell = Instantiate(_dayCellPrefab, _daysGridParent);
            emptyCell.name = "EmptyDayCell";

            var dayCell = emptyCell.GetComponent<CalendarDayCell>();
            if (dayCell != null)
            {
                dayCell.SetAsEmpty();
                _dayCells.Add(dayCell);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error creating empty day cell: {e.Message}");
        }
    }

    private void OnMonthChanged(DateTime newMonth)
    {
        if (!_isInitialized) return;

        Debug.Log($"Month changed to: {newMonth:MMMM yyyy}");

        if (_currentMonth.Month == newMonth.Month && _currentMonth.Year == newMonth.Year)
        {
            RefreshDayCellsAppearance();
            return;
        }

        _currentMonth = newMonth;
        GenerateCalendarGrid();
    }

    private void OnDateSelected(DateTime date)
    {
        if (!_isInitialized) return;
        RefreshDayCellsAppearance();
    }

    private void OnFilterChanged(CalendarManager.ActivityFilter filter)
    {
        if (!_isInitialized) return;
        RefreshDayCellsAppearance();
    }

    private void RefreshDayCellsAppearance()
    {
        foreach (var cell in _dayCells)
        {
            if (cell != null && !cell.IsEmpty && cell.gameObject != null)
            {
                try
                {
                    var dayData = CalendarManager.Instance.GetDayData(cell.Date);
                    cell.UpdateAppearance(dayData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error updating cell appearance: {e.Message}");
                }
            }
        }
    }
}