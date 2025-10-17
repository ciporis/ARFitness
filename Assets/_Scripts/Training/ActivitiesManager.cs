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
        foreach (var activiti in activities.Data)
        {
            var card = Instantiate(_cardPrefab, _spawnCard);
            card.transform.SetParent(_spawnCard);
            CardActiviti cardActiviti = card.GetComponent<CardActiviti>();
            cardActiviti.Initilize(activiti); 
        }
    }
    private void Start()
    {
        CreateActivitesUi();
    }
}
