using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Data;

public class CardActivity : MonoBehaviour, IDataCard<ActivityData>
{
    private ActivityData _activityData;
    private ChallengeData _challengeData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _distance;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private TMP_Text _type;
    [SerializeField] private Button _actionButton;

    private void Awake()
    {
        _actionButton.onClick.AddListener(OnActionButtonClick);
    }

    public void Initialize(ActivityData activityData)
    {
        _activityData = activityData;

        if (!string.IsNullOrEmpty(activityData.challengeId))
        {
            _challengeData = ActivityDataManager.Instance.GetChallengeById(activityData.challengeId);
        }

        SetData();
    }

    public void SetData()
    {
        if (_activityData == null) return;

        _name.text = _activityData.name;
        _description.text = _activityData.description;
        _distance.text = $"{_activityData.distance:F1} км";
        _duration.text = _activityData.duration.ToString(@"hh\:mm");
        _type.text = GetActivityTypeText(_activityData.type);

        UpdateActionButton();
    }

    private void UpdateActionButton()
    {
        var buttonText = _actionButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = IsChallenge() ? "Участвовать" : "Записаться";
        }
    }

    private void OnActionButtonClick()
    {
        if (IsChallenge())
        {
            RegisterForChallenge();
        }
        else
        {
            RegisterForRegularActivity();
        }
    }

    private bool IsChallenge()
    {
        return _challengeData != null || !string.IsNullOrEmpty(_activityData.challengeId);
    }

    private void RegisterForChallenge()
    {
        var request = new ModalRequest
        {
            title = "Участие в челлендже",
            message = $"Хотите участвовать в челлендже \"{_activityData.name}\"?",
            approveText = "Участвовать",
            cancelText = "Отмена",
            onApprove = CompleteChallengeRegistration,
            onCancel = OnRegistrationCancelled
        };

        ModalManager.Instance.ShowModal(request);
    }

    private void RegisterForRegularActivity()
    {
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
            ModalManager.Instance.ShowAlertModal("Нельзя записаться на прошедшую дату!");
            return;
        }

        CompleteRegularRegistration(selectedDate);
    }

    private void CompleteChallengeRegistration()
    {
        try
        {
            if (_challengeData != null)
            {
                ActivityDataManager.Instance.JoinChallenge(_challengeData.id);
            }

            var challengeActivity = new ActivityData
            {
                id = Guid.NewGuid().ToString(),
                name = _activityData.name,
                description = _activityData.description,
                date = DateTime.Today,
                type = _activityData.type,
                challengeId = _activityData.challengeId,
                distance = _activityData.distance,
                duration = _activityData.duration,
                isCompleted = false
            };

            CalendarManager.Instance.AddActivity(challengeActivity);

            ModalManager.Instance.ShowAlertModal(
                $"🎉 Вы участвуете в челлендже!\n\"{_activityData.name}\"",
                closeText: "Отлично!"
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
            ModalManager.Instance.ShowAlertModal("Ошибка при записи на челлендж");
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

            CalendarManager.Instance.AddActivity(newActivity);

            ModalManager.Instance.ShowAlertModal(
                $"✅ \"{_activityData.name}\" записана на {selectedDate:dd.MM.yyyy}",
                closeText: "Отлично!"
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
            ModalManager.Instance.ShowAlertModal("Ошибка при записи");
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