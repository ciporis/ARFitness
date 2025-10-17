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
    [SerializeField] private Animator _animator;

    private Action _onApprove;
    private Action _onCancel;
    private Action _onClose;

    private void Awake()
    {
        _approveButton.onClick.AddListener(OnApprove);
        _cancelButton.onClick.AddListener(OnCancel);
        _closeButton.onClick.AddListener(OnClose);
    }

    public void Initialize(ModalRequest request)
    {
        _titleText.text = string.IsNullOrEmpty(request.title) ? "Подтверждение" : request.title;
        _messageText.text = request.message;
        _approveButtonText.text = request.approveText;
        _cancelButtonText.text = request.cancelText;

        _onApprove = request.onApprove;
        _onCancel = request.onCancel;
        _onClose = request.onClose;

        _approveButton.gameObject.SetActive(_onApprove != null);
        _cancelButton.gameObject.SetActive(_onCancel != null);

        ShowAnimation();
    }

    private void OnApprove()
    {
        _onApprove?.Invoke();
        Close();
    }

    private void OnCancel()
    {
        _onCancel?.Invoke();
        Close();
    }

    private void OnClose()
    {
        _onClose?.Invoke();
        Close();
    }

    private void ShowAnimation()
    {
    }

    private void Close()
    {
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClose();
        }
    }
}