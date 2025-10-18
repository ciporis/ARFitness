using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardActivity : MonoBehaviour, IDataCard<ActivityData>, IInitializable<ChallengeData>
{
    private ActivityData _activityData;
    private ChallengeData _challengeData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _distance;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private TMP_Text _type;
    [SerializeField] private TMP_Text _date; 
    [SerializeField] private Button _actionButton;

    private bool _isChallenge = false;

    private void Awake()
    {
        if (_actionButton != null)
        {
            _actionButton.onClick.AddListener(OnActionButtonClick);
        }
    }
    public void Initialize(ActivityData activityData)
    {
        _activityData = activityData;
        _challengeData = null;
        _isChallenge = false;
        SetData();
    }

    public void Initialize(ChallengeData challengeData)
    {
        _challengeData = challengeData;
        _activityData = null;
        _isChallenge = true;
        SetData();
    }

    public void SetData()
    {
        if (_isChallenge)
        {
            SetChallengeData();
        }
        else
        {
            SetActivityData();
        }

        UpdateActionButton();
    }

    private void SetActivityData()
    {
        if (_activityData == null)
        {
            Debug.LogError("Activity data is null");
            return;
        }

        if (_name != null) _name.text = _activityData.name;
        if (_description != null) _description.text = _activityData.description;
        if (_distance != null) _distance.text = $"{_activityData.distance:F1} км";
        if (_duration != null) _duration.text = _activityData.duration.ToString(@"hh\:mm");
        if (_duration != null) _date.text = _activityData.date.ToString();
        if (_type != null) _type.text = GetActivityTypeText(_activityData.type);
    }

    private void SetChallengeData()
    {
        if (_challengeData == null)
        {
            Debug.LogError("Challenge data is null");
            return;
        }

        if (_name != null) _name.text = _challengeData.name;
        if (_description != null) _description.text = _challengeData.description;
        if (_distance != null) _distance.text = "Челлендж";
        if (_duration != null) _duration.text = _challengeData.duration.ToString();
        if (_duration != null) _date.text = "";
        if (_type != null) _type.text = GetActivityTypeText(_challengeData.type);
    }

    private void UpdateActionButton()
    {
        if (_actionButton != null)
        {
            var buttonText = _actionButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = _isChallenge ? "Участвовать" : "Записаться";
            }
        }
    }

    private void OnActionButtonClick()
    {
        if (_isChallenge)
        {
            RegisterForChallenge();
        }
        else
        {
            RegisterForRegularActivity();
        }
    }

    private void RegisterForChallenge()
    {
        if (ModalManager.Instance == null)
        {
            Debug.LogError("ModalManager is not available");
            return;
        }

        var request = new ModalRequest
        {
            title = "Участие в челлендже",
            message = $"Хотите участвовать в челлендже \"{_challengeData.name}\"?",
            approveText = "Участвовать",
            cancelText = "Отмена",
            onApprove = CompleteChallengeRegistration,
            onCancel = OnRegistrationCancelled
        };

        ModalManager.Instance.ShowModal(request);
    }

    private void RegisterForRegularActivity()
    {
        if (ModalManager.Instance == null)
        {
            Debug.LogError("ModalManager is not available");
            return;
        }

        var request = new ModalRequest
        {
            title = "Запись на тренировку",
            message = $"Выберите дату для \"{_activityData.name}\":",
            approveText = "Записаться",
            cancelText = "Отмена",
            showDatePicker = true,
            initialDate = DateTime.Today,
            onApprove = () => OnRegularActivityApproved(ModalManager.Instance.GetSelectedDate()),
            onCancel = OnRegistrationCancelled
        };

        ModalManager.Instance.ShowModalWithDatePicker(request);
    }

    private void OnRegularActivityApproved(DateTime selectedDate)
    {
        if (selectedDate < DateTime.Today)
        {
            ModalManager.Instance?.ShowAlertModal("Нельзя записаться на прошедшую дату!");
            return;
        }

        CompleteRegularRegistration(selectedDate);
    }

    private void CompleteChallengeRegistration()
    {
        try
        {
            if (ActivityDataManager.Instance != null && _challengeData != null)
            {
                ActivityDataManager.Instance.JoinChallenge(_challengeData.id);
            }

            ModalManager.Instance?.ShowAlertModal(
                $"🎉 Вы участвуете в челлендже!\n\"{_challengeData.name}\"",
                closeText: "Отлично!"
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
            ModalManager.Instance?.ShowAlertModal("Ошибка при записи на челлендж");
        }
    }

    private void CompleteRegularRegistration(DateTime selectedDate)
    {
        try
        {
            var newActivity = new ActivityData
            {
                id = Guid.NewGuid().ToString(),
                name = _activityData.name,
                description = _activityData.description,
                date = selectedDate,
                type = _activityData.type,
                distance = _activityData.distance,
                duration = _activityData.duration,
                isCompleted = false
            };

            if (CalendarManager.Instance != null)
            {
                CalendarManager.Instance.AddActivity(newActivity);
            }

            ModalManager.Instance?.ShowAlertModal(
                $"✅ \"{_activityData.name}\" записана на {selectedDate:dd.MM.yyyy}",
                closeText: "Отлично!"
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
            ModalManager.Instance?.ShowAlertModal("Ошибка при записи");
        }
    }

    private void OnRegistrationCancelled()
    {
        Debug.Log("Регистрация отменена");
    }

    private string GetActivityTypeText(ActivityType type)
    {
        return type switch
        {
            ActivityType.Running => "🏃 Бег",
            ActivityType.Walking => "🚶 Ходьба",
            _ => "🎯 Активность"
        };
    }
}