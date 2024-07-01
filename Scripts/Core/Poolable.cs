using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool IsUsing = false;

    public void Destroy()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
