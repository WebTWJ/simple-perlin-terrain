using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float speed = 0.5f;
    public float sensitivity = 5.0f;
    public float height = 2.5f;

    // Update is called once per frame
    void Update()
    {
        /*        transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
                transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        */

        float verticalMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad);
        float horizontalMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);

        verticalMovement += Input.GetAxis("Horizontal") * speed * Time.deltaTime * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        horizontalMovement += Input.GetAxis("Horizontal") * speed * Time.deltaTime * -Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad);

        TerrainScript obj = GameObject.Find("MeshGenerator").GetComponent<TerrainScript>();

        obj.offsetX += verticalMovement;
        obj.offsetY += horizontalMovement;
        transform.position = new Vector3(obj.width / 2, obj.vertices[obj.size / 2][1] + height, obj.height / 2);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
    }
}