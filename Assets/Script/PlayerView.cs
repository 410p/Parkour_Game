using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    //[SerializeField] private float mouseSpeed = 4f; //회전속도
    private float mouseX = 0f; //좌우 회전값을 담을 변수
    private float mouseY = 0f; //위아래 회전값을 담을 변수

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");

        gameObject.transform.rotation = Quaternion.Euler(mouseY*-1, mouseX, 0);
    }
}
