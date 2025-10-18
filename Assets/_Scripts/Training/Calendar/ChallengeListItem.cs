using Data;
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
        _typeText.text = "Челлендж";

        SetupRegistrationButton();
    }

    private void SetupRegistrationButton()
    {
        if (_challengeData.isRegistered)
        {
            _registerButton.gameObject.SetActive(false);
            _registeredBadge.SetActive(true);
        }
        else
        {
            _registerButton.gameObject.SetActive(true);
            _registerButton.GetComponentInChildren<TMP_Text>().text = "Участвовать";
            _registerButton.onClick.RemoveAllListeners();
            _registerButton.onClick.AddListener(OnRegisterClicked);
        }
    }

    private void OnRegisterClicked()
    {
        ModalManager.Instance.ShowChoiceModal(
            $"Участвовать в челлендже \"{_challengeData.name}\"?",
            onApprove: () => RegisterForChallenge(),
            onCancel: null,
            approveText: "Участвовать",
            cancelText: "Отмена"
        );
    }

    private void RegisterForChallenge()
    {
        CalendarManager.Instance.RegisterForChallenge(_challengeData.id);
        _registeredBadge.SetActive(true);
        _registerButton.gameObject.SetActive(false);

        ModalManager.Instance.ShowAlertModal(
            $"🎉 Вы участвуете в челлендже!\n\"{_challengeData.name}\"",
            closeText: "Отлично!"
        );
    }
}