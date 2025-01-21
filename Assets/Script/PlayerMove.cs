using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    private float mouseX = 0f; //좌우 회전값을 담을 변수
    [SerializeField] private Rigidbody rb;

    public float moveSpeed;

    public float jump;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 MoveDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("j");
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }

        rb.velocity = MoveDirection * moveSpeed;

        mouseX += Input.GetAxis("Mouse X");

        gameObject.transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
