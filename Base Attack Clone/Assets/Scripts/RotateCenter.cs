using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCenter : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 5 * Time.deltaTime, 0);
    }

    
}
