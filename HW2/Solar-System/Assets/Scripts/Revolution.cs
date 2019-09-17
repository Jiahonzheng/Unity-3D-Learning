using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    public Transform center;
    private float speed;
    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(9, 12);
        x = Random.Range(-50, 50);
        y = Random.Range(-50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        var axis = new Vector3(0, x, y);
        transform.RotateAround(center.position, axis, speed * Time.deltaTime);
    }
}
