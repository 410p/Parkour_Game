using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    //[SerializeField] private float mouseSpeed = 4f; //ȸ���ӵ�
    private float mouseX = 0f; //�¿� ȸ������ ���� ����
    private float mouseY = 0f; //���Ʒ� ȸ������ ���� ����

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
