using UnityEngine;
using System;
using System.Collections.Generic;

public class ModalManager : MonoBehaviour
{
    public static ModalManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private GameObject _modalPrefab;
    [SerializeField] private Transform _modalParent;
    [SerializeField] private bool _dontDestroyOnLoad = true;
    [SerializeField] private int _maxModals = 3;

    private Queue<ModalRequest> _modalQueue = new Queue<ModalRequest>();
    private bool _isShowingModal = false;

    [Header("Date Selection")]
    [SerializeField] private GameObject _datePickerPrefab;

    private DateTime _selectedDate = DateTime.Today;
    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void CreateDatePickerModal(ModalRequest request)
    {
        var modalGO = Instantiate(_modalPrefab, _modalParent);
        var modalWindow = modalGO.GetComponent<ModalWindow>();

        if (modalWindow != null)
        {
            // Настраиваем окно с выбором даты
            modalWindow.InitializeWithDatePicker(request, _selectedDate, OnDateSelected);
        }
    }

    private void OnDateSelected(DateTime date)
    {
        _selectedDate = date;
    }

    #region Public Methods
    public void ShowModalWithDatePicker(ModalRequest request)
    {
        _selectedDate = DateTime.Today;

        if (request.showDatePicker)
        {
            CreateDatePickerModal(request);
        }
        else
        {
            ShowModal(request);
        }
    }
    public DateTime GetSelectedDate()
    {
        return _selectedDate;
    }
    public void ShowModal(ModalRequest request)
    {
        if (_isShowingModal)
        {
            if (_modalQueue.Count < _maxModals)
            {
                _modalQueue.Enqueue(request);
            }
            return;
        }

        CreateModalWindow(request);
    }

    public void ShowConfirmModal(string message, Action onConfirm, string confirmText = "Да", string cancelText = "Нет")
    {
        var request = new ModalRequest
        {
            message = message,
            approveText = confirmText,
            cancelText = cancelText,
            onApprove = onConfirm
        };
        ShowModal(request);
    }

    public void ShowChoiceModal(string message, Action onApprove, Action onCancel, string approveText = "Да", string cancelText = "Нет")
    {
        var request = new ModalRequest
        {
            message = message,
            approveText = approveText,
            cancelText = cancelText,
            onApprove = onApprove,
            onCancel = onCancel
        };
        ShowModal(request);
    }

    public void ShowAlertModal(string message, Action onClose = null, string closeText = "OK")
    {
        var request = new ModalRequest
        {
            message = message,
            approveText = closeText,
            cancelText = "",
            onApprove = onClose
        };
        ShowModal(request);
    }

    #endregion

    #region Private Methods

    private void CreateModalWindow(ModalRequest request)
    {
        if (_modalPrefab == null)
        {
            Debug.LogError("Modal prefab is not assigned!");
            return;
        }

        var modalGO = Instantiate(_modalPrefab, _modalParent);
        var modalWindow = modalGO.GetComponent<ModalWindow>();

        if (modalWindow == null)
        {
            Debug.LogError("ModalWindow component not found on prefab!");
            Destroy(modalGO);
            return;
        }

        // Добавляем обработчик закрытия для очереди
        var originalOnClose = request.onClose;
        request.onClose = () =>
        {
            originalOnClose?.Invoke();
            OnModalClosed();
        };

        var originalOnApprove = request.onApprove;
        request.onApprove = () =>
        {
            originalOnApprove?.Invoke();
            OnModalClosed();
        };

        var originalOnCancel = request.onCancel;
        request.onCancel = () =>
        {
            originalOnCancel?.Invoke();
            OnModalClosed();
        };

        modalWindow.Initialize(request);
        _isShowingModal = true;
    }

    private void OnModalClosed()
    {
        _isShowingModal = false;

        if (_modalQueue.Count > 0)
        {
            var nextRequest = _modalQueue.Dequeue();
            ShowModal(nextRequest);
        }
    }

    #endregion

    #region Utility Methods

    public bool IsModalActive()
    {
        return _isShowingModal;
    }

    public void ClearQueue()
    {
        _modalQueue.Clear();
    }

    public int GetQueueCount()
    {
        return _modalQueue.Count;
    }

    #endregion
}