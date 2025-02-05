using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    private float mouseX = 0f; //좌우 회전값을 담을 변수
    [SerializeField] private Rigidbody rb;

    private Animator animator; //애니메이터 변수

    //추후 애니메이터 작업 시 Trigger값에 "Fall"항목 추가
    //Fall 트리거로 인한 낙법 애니메이션 재생

    [SerializeField]
    private float moveSpeed; //기본 이동속도

    [SerializeField]
    private float jump; //점프 값

    private float jumpStack; //추가 점프를 위한 변수

    private float stamina; //스태미나

    [SerializeField]
    private float extraSpeedValue; //추가 이동속도(달리기) 값

    private float extraSpeed; //추가 이동속도(달리기 속도)

    private float speed; //속도 (기본속도 + 달리기 속도)

    private bool isGround; //땅에 닿아있는지 여부

    private bool isJump; //점프를 하고있는지 여부

    private bool isPossibleToClimbing; //벽을 탈수 있는지 여부

    [SerializeField]
    private float fallVelocity; //낙법 기준 속도

    private bool isPossibleToFall; //낙법 가능한지 여부

    [SerializeField]
    private float climbingSpeed; //벽타기 속도

    [SerializeField]
    private float climbingJumpSpeed; //벽타기 취소 시 점프 속도\


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

        #region 점프
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

        //추가 점프
        if (Input.GetKey(KeyCode.Space) && jumpStack < 5 && isJump)
        {
            jumpStack += 0.1f;
            rb.AddForce(0, jumpStack, 0); //증가하는 jumpStack만큼 추가로 점프
        }

        //점프 중지
        if(Input.GetKeyUp(KeyCode.Space))
        {
            //jumpStack 초기화 후 점프 상태 변경
            jumpStack = 0;
            isJump = false;
        }
        #endregion

        #region 달리기
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
        {
            stamina -= 0.01f;
            Debug.Log("달림" + stamina);
            extraSpeed = extraSpeedValue;
        }

        else
        {
            extraSpeed = 0;
            if (stamina <= 100)
            {
                Debug.Log("멈춤" + stamina);
                stamina += 0.01f;
            }
        }
        #endregion

        #region 낙법
        //플레이어의 속도가 낙법 기준 속도를 넘었을 때
        if (rb.velocity.y >= fallVelocity)
        {
            isPossibleToFall = true; //낙법 활성화
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
