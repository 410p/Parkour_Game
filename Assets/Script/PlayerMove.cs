using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    private float mouseX = 0f; //�¿� ȸ������ ���� ����
    [SerializeField] private Rigidbody rb;

    private Animator animator; //�ִϸ����� ����

    //���� �ִϸ����� �۾� �� Trigger���� "Fall"�׸� �߰�
    //Fall Ʈ���ŷ� ���� ���� �ִϸ��̼� ���

    [SerializeField]
    private float moveSpeed; //�⺻ �̵��ӵ�

    [SerializeField]
    private float jump; //���� ��

    private float jumpStack; //�߰� ������ ���� ����

    private float stamina; //���¹̳�

    [SerializeField]
    private float extraSpeedValue; //�߰� �̵��ӵ�(�޸���) ��

    private float extraSpeed; //�߰� �̵��ӵ�(�޸��� �ӵ�)

    private float speed; //�ӵ� (�⺻�ӵ� + �޸��� �ӵ�)

    private bool isGround; //���� ����ִ��� ����

    private bool isJump; //������ �ϰ��ִ��� ����

    private bool isPossibleToClimbing; //���� Ż�� �ִ��� ����

    [SerializeField]
    private float fallVelocity; //���� ���� �ӵ�

    private bool isPossibleToFall; //���� �������� ����

    [SerializeField]
    private float climbingSpeed; //��Ÿ�� �ӵ�

    [SerializeField]
    private float climbingJumpSpeed; //��Ÿ�� ��� �� ���� �ӵ�\


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 

        stamina = 100;
        extraSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float speedX = Input.GetAxis("Vertical");
        float speedZ = Input.GetAxis("Horizontal");

        Vector3 MoveDirection = transform.right * speedZ + transform.forward * speedX;

        //rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed + new Vector3(0, rb.velocity, 0);

        Debug.Log(speed);

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        #region ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Debug.Log("j");
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
            isJump = true;
            animator.SetTrigger("Jump");
            if (isPossibleToClimbing)
            {
                rb.AddForce(transform.forward * -1 * climbingJumpSpeed);
            }
        }

        //�߰� ����
        if (Input.GetKey(KeyCode.Space) && jumpStack < 5 && isJump)
        {
            jumpStack += 0.1f;
            rb.AddForce(0, jumpStack, 0); //�����ϴ� jumpStack��ŭ �߰��� ����
        }

        //���� ����
        if(Input.GetKeyUp(KeyCode.Space))
        {
            //jumpStack �ʱ�ȭ �� ���� ���� ����
            jumpStack = 0;
            isJump = false;
        }
        #endregion

        #region �޸���
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            stamina -= 0.01f;
            Debug.Log("�޸�" + stamina);
            extraSpeed = extraSpeedValue;
        }

        else
        {
            extraSpeed = 0;
            if (stamina <= 100)
            {
                Debug.Log("����" + stamina);
                stamina += 0.01f;
            }
        }
        #endregion

        #region ����
        //�÷��̾��� �ӵ��� ���� ���� �ӵ��� �Ѿ��� ��
        if (rb.velocity.y >= fallVelocity)
        {
            isPossibleToFall = true; //���� Ȱ��ȭ
        }

        #endregion

        speed = moveSpeed + extraSpeed;
        if (isPossibleToClimbing)
        {
            rb.velocity = new Vector3(0, climbingSpeed, 0);
        }
        rb.velocity = MoveDirection * speed + new Vector3(0, rb.velocity.y, 0);

        //mouseX += Input.GetAxis("Mouse X");

        //gameObject.transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGround = true;
            animator.SetBool("OnGround", true);

            if (isPossibleToFall)
            {
                //animator.SetTrigger("Fall");
                isPossibleToFall = false;
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isPossibleToClimbing = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGround = false;
            animator.SetBool("OnGround", false);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isPossibleToClimbing = false;
        }
    }
}
