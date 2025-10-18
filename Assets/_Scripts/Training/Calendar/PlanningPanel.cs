using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Data;
public class PlanningPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Dropdown _activityTypeDropdown;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _distanceInput;
    [SerializeField] private TMP_InputField _durationInput;
    [SerializeField] private Toggle _notificationToggle;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _cancelButton;

    private DateTime _selectedDate;

    private void Start()
    {
        _saveButton.onClick.AddListener(OnSaveClicked);
        _cancelButton.onClick.AddListener(OnCancelClicked);

        // ��������� dropdown ������ �����������
        _activityTypeDropdown.ClearOptions();
        var activityTypes = Enum.GetNames(typeof(ActivityType));
        _activityTypeDropdown.AddOptions(new System.Collections.Generic.List<string>(activityTypes));

        _panel.SetActive(false);
    }

    public void ShowForDate(DateTime date)
    {
        _selectedDate = date;
        _dateText.text = $"������������ �� {date:dd.MM.yyyy}";
        _panel.SetActive(true);

        // ����� �����
        _nameInput.text = "";
        _distanceInput.text = "";
        _durationInput.text = "";
        _notificationToggle.isOn = true;
    }

    private void OnSaveClicked()
    {
        if (string.IsNullOrEmpty(_nameInput.text))
        {
            ModalManager.Instance.ShowAlertModal("������� �������� ����������!");
            return;
        }

        var newActivity = new ActivityData
        {
            id = Guid.NewGuid().ToString(),
            name = _nameInput.text,
            date = _selectedDate,
            type = (ActivityType)_activityTypeDropdown.value,
            distance = float.TryParse(_distanceInput.text, out float dist) ? dist : 0,
            duration = TimeSpan.TryParse(_durationInput.text, out TimeSpan dur) ? dur : TimeSpan.Zero,
            isCompleted = false
        };

        CalendarManager.Instance.AddActivity(newActivity);

        if (_notificationToggle.isOn)
        {
            // ������ ��������� �����������
            ScheduleNotification(newActivity);
        }

        _panel.SetActive(false);

        ModalManager.Instance.ShowAlertModal("���������� �������������!");
    }

    private void OnCancelClicked()
    {
        _panel.SetActive(false);
    }

    private void ScheduleNotification(ActivityData activity)
    {
        // ������ ������������ �����������
        Debug.Log($"����������� ����������� ���: {activity.name} �� {activity.date:dd.MM.yyyy}");
    }
}