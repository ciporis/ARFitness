using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActivityData
{
    public int Id;
    public string Name;
    public string Description;
    public string Date;
}

public class Activities : MonoBehaviour
{
    [SerializeField] private List<ActivityData> _data;
    public List<ActivityData> Data {  get { return _data; } set { _data = value; } }
}
