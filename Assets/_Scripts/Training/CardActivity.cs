using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardActivity : MonoBehaviour, IDataCard<ActivityData>
{
    private ActivityData _activityData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _type;
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
        _name.text = _activityData.name;
        _description.text = _activityData.description;
        _date.text = _activityData.date.ToString();
        _type.text = _activityData.type.ToString();
    }

    private void OnActionButtonClick()
    {
        var request = new ModalRequest(
            $"Вы хотите зарегистрироваться на активность \"{_activityData.name}\"?",
            onApprove: OnRegistrationApproved,
            onCancel: OnRegistrationCancelled
        )
        {
            approveText = "Зарегистрироваться",
            cancelText = "Отмена"
        };

        ModalManager.Instance.ShowModal(request);
    }

    private void OnRegistrationApproved()
    {
        Debug.Log($"Регистрация на {_activityData.name} подтверждена!");

        ModalManager.Instance.ShowAlertModal(
            "Регистрация прошла успешно!",
            onClose: () => Debug.Log("Alert closed"),
            closeText: "Отлично!"
        );

        RegisterForActivity();
    }

    private void OnRegistrationCancelled()
    {
        Debug.Log("Регистрация отменена пользователем");
    }

    private void RegisterForActivity()
    {
        // Реальная логика регистрации
        // ActivityManager.Instance.Register(_activityData.Id);
    }
}