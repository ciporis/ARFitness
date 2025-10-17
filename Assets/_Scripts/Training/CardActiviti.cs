using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardActiviti : MonoBehaviour
{
    private ActivityData _activityData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _date;

    public void Initilize(ActivityData activityData)
    {
        _activityData = activityData;
        SetData();
    }
    private void SetData()
    {
        if(_activityData == null) 
            Debug.LogErrorFormat("Activity data is null");
        _name.text = _activityData.Name; 
        _description.text = _activityData.Description; 
        _date.text = _activityData.Date; 
    }
}
