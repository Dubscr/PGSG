using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float z;
    [SerializeField] Transform cam;
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cam.position.x, cam.position.y, z);
        cam.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
