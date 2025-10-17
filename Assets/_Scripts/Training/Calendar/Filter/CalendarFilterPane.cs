using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarFilterPanel : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _filterDropdown;
    [SerializeField] private Toggle _showNotificationsToggle;

    private void Start()
    {
        // ����������� dropdown
        _filterDropdown.ClearOptions();
        _filterDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "��� ����������",
            "��� ����������",
            "������������"
        });

        _filterDropdown.onValueChanged.AddListener(OnFilterChanged);
        _showNotificationsToggle.onValueChanged.AddListener(OnNotificationsToggled);
    }

    private void OnFilterChanged(int index)
    {
        var filter = (CalendarManager.ActivityFilter)index;
        CalendarManager.Instance.SetFilter(filter);
    }

    private void OnNotificationsToggled(bool isOn)
    {
        Debug.Log($"�����������: {(isOn ? "��������" : "���������")}");
    }
}