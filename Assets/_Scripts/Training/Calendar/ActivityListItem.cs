using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityListItem : MonoBehaviour, IInitializable<ActivityData>
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _detailsText;
    [SerializeField] private TMP_Text _metricsText;
    [SerializeField] private Image _typeIcon;
    [SerializeField] private Button _actionButton;

    private ActivityData _activityData;

    public void Initialize(ActivityData data)
    {
        _activityData = data;

        _nameText.text = data.name;
        _detailsText.text = $"{data.type} • {data.duration:hh\\:mm} • {data.distance:F1} км";

        _typeIcon.color = GetActivityColor(data.type);

        _actionButton.onClick.RemoveAllListeners();
        if (data.isCompleted)
        {
            _actionButton.GetComponentInChildren<TMP_Text>().text = "Повторить";
            _actionButton.onClick.AddListener(OnRepeatClicked);
        }
        else
        {
            _actionButton.GetComponentInChildren<TMP_Text>().text = "Начать";
            _actionButton.onClick.AddListener(OnStartClicked);
        }
    }

    private void OnRepeatClicked()
    {
        CalendarManager.Instance.RepeatActivity(_activityData.id);
    }

    private void OnStartClicked()
    {
        Debug.Log($"Начинаем тренировку: {_activityData.name}");
    }

    private Color GetActivityColor(ActivityType type)
    {
        return type switch
        {
            ActivityType.Running => Color.red,
            ActivityType.Walking => Color.gray,
            _ => Color.white
        };
    }
}