using System;
using UnityEngine;

[System.Serializable]
public class ModalRequest
{
    public string title;
    public string message;
    public string approveText = "��";
    public string cancelText = "���";
    public Action onApprove;
    public Action onCancel;
    public Action onClose;

    public ModalRequest() { }

    public ModalRequest(string message, Action onApprove = null, Action onCancel = null)
    {
        this.message = message;
        this.onApprove = onApprove;
        this.onCancel = onCancel;
    }

    public ModalRequest(string title, string message, string approveText = "��", string cancelText = "���")
    {
        this.title = title;
        this.message = message;
        this.approveText = approveText;
        this.cancelText = cancelText;
    }
}