using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageHandler : MonoBehaviour
{

    void Start()
    {
        transform.localPosition += new Vector3(0, 0.5f,0);
        Destroy(gameObject, 0.8f);
    }
}
