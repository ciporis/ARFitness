using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Activities))]
public class ActivitiesManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnCard;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Activities activities;
    public void CreateActivitesUi()
    {
        foreach (var activiti in activities.ActivitiesData)
        {
            var card = Instantiate(_cardPrefab, _spawnCard);
            card.transform.SetParent(_spawnCard);
            CardActivity cardActiviti = card.GetComponent<CardActivity>();
            cardActiviti.Initialize(activiti); 
        }
        foreach (var challenge in activities.ChallengesData)
        {
            var card = Instantiate(_cardPrefab, _spawnCard);
            card.transform.SetParent(_spawnCard);
            CardActivity cardActiviti = card.GetComponent<CardActivity>();
            cardActiviti.Initialize(challenge);
        }
    }
    private void Start()
    {
        CreateActivitesUi();
    }
}
