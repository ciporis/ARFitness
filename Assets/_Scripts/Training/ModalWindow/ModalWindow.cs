using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ModalWindow : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _approveButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _approveButtonText;
    [SerializeField] private TMP_Text _cancelButtonText;

    [Header("Date Picker Components")]
    [SerializeField] private GameObject _datePickerPanel;
    [SerializeField] private TMP_Text _selectedDateText;
    [SerializeField] private Button _prevDayButton;
    [SerializeField] private Button _nextDayButton;

    private Action _onApprove;
    private Action _onCancel;
    private Action _onClose;
    private DateTime _currentDate;
    private Action<DateTime> _onDateSelected;
    private bool _hasDatePicker = false;

    private void Awake()
    {
        _approveButton.onClick.AddListener(OnApprove);
        _cancelButton.onClick.AddListener(OnCancel);
        _closeButton.onClick.AddListener(OnClose);
    }

    public void Initialize(ModalRequest request)
    {
        if (_datePickerPanel != null)
            _datePickerPanel.SetActive(false);

        _hasDatePicker = false;
        SetTextImmediately(request);
        SetupCallbacks(request);
        SetupButtonsVisibility(request);
    }

    public void InitializeWithDatePicker(ModalRequest request, DateTime initialDate, Action<DateTime> onDateSelected)
    {
        _currentDate = initialDate;
        _onDateSelected = onDateSelected;
        _hasDatePicker = true;

        if (_datePickerPanel != null)
        {
            _datePickerPanel.SetActive(true);
            SetupDatePicker();
        }
        else
        {
            Debug.LogError("Date Picker Panel is not assigned in inspector!");
        }

        SetTextImmediately(request);
        SetupCallbacks(request);
        SetupButtonsVisibility(request);
    }

    private void SetTextImmediately(ModalRequest request)
    {
        _titleText.text = string.IsNullOrEmpty(request.title) ? "Подтверждение" : request.title;
        _messageText.text = request.message;
        _approveButtonText.text = request.approveText;
        _cancelButtonText.text = request.cancelText;

        if (_hasDatePicker && _selectedDateText != null)
        {
            _selectedDateText.text = _currentDate.ToString("dd.MM.yyyy");
        }
    }

    private void SetupCallbacks(ModalRequest request)
    {
        _onApprove = request.onApprove;
        _onCancel = request.onCancel;
        _onClose = request.onClose;
    }

    private void SetupButtonsVisibility(ModalRequest request)
    {
        _approveButton.gameObject.SetActive(_onApprove != null);
        _cancelButton.gameObject.SetActive(_onCancel != null);
    }

    private void SetupDatePicker()
    {
        if (_selectedDateText == null || _prevDayButton == null || _nextDayButton == null)
        {
            Debug.LogError("Date picker components are not properly assigned!");
            return;
        }

        UpdateDateDisplay();

        _prevDayButton.onClick.RemoveAllListeners();
        _nextDayButton.onClick.RemoveAllListeners();

        _prevDayButton.onClick.AddListener(OnPrevDayClicked);
        _nextDayButton.onClick.AddListener(OnNextDayClicked);
    }

    private void OnPrevDayClicked()
    {
        ChangeDate(-1);
    }

    private void OnNextDayClicked()
    {
        ChangeDate(1);
    }

    private void ChangeDate(int days)
    {
        Debug.Log($"Changing date by {days} days. Current: {_currentDate:dd.MM.yyyy}");

        _currentDate = _currentDate.AddDays(days);
        UpdateDateDisplay();
        _onDateSelected?.Invoke(_currentDate);

        Debug.Log($"New date: {_currentDate:dd.MM.yyyy}");
    }

    private void UpdateDateDisplay()
    {
        if (_selectedDateText != null)
        {
            _selectedDateText.text = _currentDate.ToString("dd.MM.yyyy");

            if (_prevDayButton != null)
            {
                _prevDayButton.interactable = _currentDate > DateTime.Today;
            }
        }
        else
        {
            Debug.LogError("Selected Date Text is not assigned!");
        }
    }

    private void OnApprove()
    {
        Debug.Log("Approve button clicked");

        if (_hasDatePicker)
        {
            _onDateSelected?.Invoke(_currentDate);
        }

        ExecuteAction(_onApprove);
    }

    private void OnCancel()
    {
        Debug.Log("Cancel button clicked");
        ExecuteAction(_onCancel);
    }

    private void OnClose()
    {
        Debug.Log("Close button clicked");
        ExecuteAction(_onClose);
    }

    private void ExecuteAction(Action action)
    {
        action?.Invoke();
        Close();
    }

    private void Close()
    {
        Destroy(gameObject);
    }

    [ContextMenu("Debug Date Picker State")]
    private void DebugDatePickerState()
    {
        Debug.Log($"Has Date Picker: {_hasDatePicker}");
        Debug.Log($"Current Date: {_currentDate:dd.MM.yyyy}");
        Debug.Log($"Selected Date Text: {_selectedDateText != null}");
        Debug.Log($"Prev Button: {_prevDayButton != null}");
        Debug.Log($"Next Button: {_nextDayButton != null}");
    }
}