using System.Collections.Generic;
using UnityEngine;
public class Activities : MonoBehaviour
{
    [SerializeField] private List<ActivityData> _data;
    public List<ActivityData> Data {  get { return _data; } set { _data = value; } }
}
