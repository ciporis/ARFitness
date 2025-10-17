using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeListItem : MonoBehaviour, IInitializable<ChallengeData>
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private Button _registerButton;
    [SerializeField] private GameObject _registeredBadge;

    private ChallengeData _challengeData;

    public void Initialize(ChallengeData data)
    {
        _challengeData = data;

        _nameText.text = data.name;
        _dateText.text = $"{data.startDate:dd.MM} - {data.endDate:dd.MM}";
        _typeText.text = GetChallengeTypeText(data.type);

        SetupRegistrationButton();
    }

    private void SetupRegistrationButton()
    {
        if (_challengeData.isRegistered)
        {
            _registerButton.gameObject.SetActive(false);
            _registeredBadge.SetActive(true);
        }
        else if (_challengeData.isInvited)
        {
            _registerButton.gameObject.SetActive(true);
            _registerButton.GetComponentInChildren<TMP_Text>().text = "Записаться";
            _registerButton.onClick.RemoveAllListeners();
            _registerButton.onClick.AddListener(OnRegisterClicked);
        }
        else
        {
            _registerButton.gameObject.SetActive(false);
        }
    }

    private void OnRegisterClicked()
    {
        ModalManager.Instance.ShowChoiceModal(
            $"Записаться на челлендж \"{_challengeData.name}\"?",
            onApprove: () => RegisterForChallenge(),
            onCancel: null,
            approveText: "Записаться",
            cancelText: "Отмена"
        );
    }

    private void RegisterForChallenge()
    {
        CalendarManager.Instance.RegisterForChallenge(_challengeData.id);
        _registeredBadge.SetActive(true);
        _registerButton.gameObject.SetActive(false);
    }

    private string GetChallengeTypeText(ChallengeData.ChallengeType type)
    {
        return type switch
        {
            ChallengeData.ChallengeType.Personal => "Личный",
            ChallengeData.ChallengeType.Competition => "Соревнование",
            ChallengeData.ChallengeType.Team => "Командный",
            _ => "Челлендж"
        };
    }
}