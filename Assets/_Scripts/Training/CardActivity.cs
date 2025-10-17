using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardActivity : MonoBehaviour, IDataCard<ActivityData>
{
    private ActivityData _activityData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private Button _actionButton;

    private void Awake()
    {
        _actionButton.onClick.AddListener(OnActionButtonClick);
    }

    public void Initialize(ActivityData activityData)
    {
        _activityData = activityData;
        SetData();
    }

    public void SetData()
    {
        if (_activityData == null)
        {
            Debug.LogError("Activity data is null");
            return;
        }
        _name.text = _activityData.Name;
        _description.text = _activityData.Description;
        _date.text = _activityData.Date.ToString();
    }

    private void OnActionButtonClick()
    {
        // ������ ������������� ������ ������������� ModalRequest
        var request = new ModalRequest(
            $"�� ������ ������������������ �� ���������� \"{_activityData.Name}\"?",
            onApprove: OnRegistrationApproved,
            onCancel: OnRegistrationCancelled
        )
        {
            approveText = "������������������",
            cancelText = "������"
        };

        ModalManager.Instance.ShowModal(request);
    }

    private void OnRegistrationApproved()
    {
        Debug.Log($"����������� �� {_activityData.Name} ������������!");

        ModalManager.Instance.ShowAlertModal(
            "����������� ������ �������!",
            onClose: () => Debug.Log("Alert closed"),
            closeText: "�������!"
        );

        RegisterForActivity();
    }

    private void OnRegistrationCancelled()
    {
        Debug.Log("����������� �������� �������������");
    }

    private void RegisterForActivity()
    {
        // �������� ������ �����������
        // ActivityManager.Instance.Register(_activityData.Id);
    }
}