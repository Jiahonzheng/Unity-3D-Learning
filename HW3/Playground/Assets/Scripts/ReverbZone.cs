using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverbZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * 1 * Time.deltaTime;
    }
}
