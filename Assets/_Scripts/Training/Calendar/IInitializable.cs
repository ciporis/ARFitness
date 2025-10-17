using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable<T>
{
    void Initialize(T data);
}