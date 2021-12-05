using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("數值")]

    [SerializeField] private int BallsCtrlCount = 3;

    [SerializeField] private float Speed;

    [SerializeField] private float TriggerRange;

    [SerializeField] private bool IsFacingRight;

    [SerializeField] private bool IsCtrlBall;

    [SerializeField] private LayerMask GroundMask;

    [Header("座標位置")]

    [SerializeField] private Rigidbody2D rigidbody;

    [SerializeField] private Transform PlayerSpriteTransform;

    [SerializeField] private Transform ShotPos;

    [SerializeField] private Transform GroundTriggerPos;

    [SerializeField] private Transform WallRightTriggerPos , WallLeftTriggerPos;

    [Header("攻擊")]

    [SerializeField] private GameObject BallsPrefab;

    [SerializeField] private Rigidbody2D BallsCtrlRig;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveBehaviour();
        PlayerSpriteTransformCtrl();
        PlayerAttackBehaviour();
    }

    private void PlayerAttackBehaviour()
    {
        if(Input.GetButtonDown("Fire1") && !IsCtrlBall)
        {
            IsCtrlBall = true;

            Rigidbody2D BallsRig = Instantiate(BallsPrefab , ShotPos.position , Quaternion.identity).GetComponent<Rigidbody2D>();
            
            if(IsFacingRight)
            {
                BallsRig.AddForce(Vector2.right * 10 , ForceMode2D.Impulse);
            }
            else
            {
                BallsRig.AddForce(-Vector2.right * 10 , ForceMode2D.Impulse);
            }

            BallsCtrlRig = BallsRig;
        }

        if(Input.GetAxisRaw("LT") != 0 && IsCtrlBall)
        {
            ControllBallMode();
        }

        if(Input.GetButtonDown("Fire2") && IsCtrlBall)
        {
            Destroy(BallsCtrlRig.gameObject);
            IsCtrlBall = false;
        }
    }

    private void ControllBallMode()
    {
        if(Input.GetAxisRaw("RightControllHorizontal") != 0)
        {
            //Debug.Log("X:" + Input.GetAxisRaw("RightControllHorizontal") + "Y:" + Input.GetAxisRaw("RightControllVertical"));
            //Debug.Log(BallsCtrlRig.transform.GetChild(0).name);
            BallsCtrlRig.transform.GetChild(0).transform.gameObject.SetActive(true);
            BallsCtrlRig.transform.GetChild(0).transform.rotation = Quaternion.LookRotation(new Vector2(Input.GetAxisRaw("RightControllHorizontal") , -Input.GetAxisRaw("RightControllVertical")), Vector3.back) * Quaternion.Euler(270, 0, 0);

            if(Input.GetButtonDown("Fire1"))
            {
                //
                BallsCtrlRig.AddForce(new Vector2(Input.GetAxisRaw("RightControllHorizontal") , -Input.GetAxisRaw("RightControllVertical")) * 10 , ForceMode2D.Impulse);
            }
        }
        else
        {
            BallsCtrlRig.transform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }

    private void PlayerMoveBehaviour()
    {

        rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Speed , rigidbody.velocity.y);


        if(Input.GetButtonDown("Jump") && Physics2D.OverlapCircle(GroundTriggerPos.position , TriggerRange , GroundMask))
        {
            rigidbody.AddForce(new Vector2(rigidbody.velocity.x , Speed * 2) , ForceMode2D.Impulse);
        }

        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            IsFacingRight = true;
        }

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            IsFacingRight = false;
        }
    }

    private void PlayerSpriteTransformCtrl()
    {
        if(IsFacingRight)
        {
           PlayerSpriteTransform.localScale = new Vector3(Mathf.Abs(PlayerSpriteTransform.localScale.x) , PlayerSpriteTransform.localScale.y , PlayerSpriteTransform.localScale.z); 
        }
        else
        {
            PlayerSpriteTransform.localScale = new Vector3(-Mathf.Abs(PlayerSpriteTransform.localScale.x) , PlayerSpriteTransform.localScale.y , PlayerSpriteTransform.localScale.z);
        }
    }


    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(GroundTriggerPos.position , TriggerRange);    
    }
}
