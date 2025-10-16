using UnityEngine;

public class ObjectsStateController : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;

    public void ChangeState()
    {
        foreach (var obj in _objects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
