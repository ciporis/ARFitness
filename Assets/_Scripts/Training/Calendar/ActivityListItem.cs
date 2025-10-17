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
        _detailsText.text = $"{data.type} � {data.duration:hh\\:mm} � {data.distance:F1} ��";
        _metricsText.text = $"{data.calories} ���� � {data.points} ������";

        // ��������� ������ ���� ����������
        _typeIcon.color = GetActivityColor(data.type);

        // ��������� ������ ��������
        _actionButton.onClick.RemoveAllListeners();
        if (data.isCompleted)
        {
            _actionButton.GetComponentInChildren<TMP_Text>().text = "���������";
            _actionButton.onClick.AddListener(OnRepeatClicked);
        }
        else
        {
            _actionButton.GetComponentInChildren<TMP_Text>().text = "������";
            _actionButton.onClick.AddListener(OnStartClicked);
        }
    }

    private void OnRepeatClicked()
    {
        CalendarManager.Instance.RepeatActivity(_activityData.id);
    }

    private void OnStartClicked()
    {
        // ������ ������ ����������
        Debug.Log($"�������� ����������: {_activityData.name}");
    }

    private Color GetActivityColor(ActivityData.ActivityType type)
    {
        return type switch
        {
            ActivityData.ActivityType.Running => Color.red,
            ActivityData.ActivityType.Cycling => Color.blue,
            ActivityData.ActivityType.Swimming => Color.cyan,
            ActivityData.ActivityType.Gym => Color.magenta,
            ActivityData.ActivityType.Yoga => Color.green,
            ActivityData.ActivityType.Walking => Color.gray,
            _ => Color.white
        };
    }
}