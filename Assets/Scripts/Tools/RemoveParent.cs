using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    public void Remove()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
    }
}
