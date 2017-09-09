using UnityEngine;
using System.Collections;

public class FezMove : MonoBehaviour
{

    private int Horizontal = 0;

    public Animator anim;
    public float MovementSpeed = 5f;
    public float Gravity = 1f;
    public CharacterController charController;
    private FacingDirection _myFacingDirection;
    public float JumpHeight = 0f;
    public bool _jumping = false;
    private float degree = 0;
    public SpriteRenderer spriteRenderer;

    private int direction = 1;


    public FacingDirection CmdFacingDirection
    {

        set
        {
            _myFacingDirection = value;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Horizontal = -1;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Horizontal = 1;
            spriteRenderer.flipX = false;
        }
        else
            Horizontal = 0;

        if (Input.GetKeyDown(KeyCode.Space) && !_jumping)
        {
            _jumping = true;
            StartCoroutine(JumpingWait());
        }

        if (anim)
        {
            anim.SetInteger("Horizontal", Horizontal);

            float moveFactor = MovementSpeed * Time.deltaTime * 10f;
            MoveCharacter(moveFactor);

        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);

    }

    private void MoveCharacter(float moveFactor)
    {
        Vector3 trans = Vector3.zero;
        if (_myFacingDirection == FacingDirection.Front)
        {
            trans = new Vector3(Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if (_myFacingDirection == FacingDirection.Right)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, Horizontal * moveFactor);
        }
        else if (_myFacingDirection == FacingDirection.Back)
        {
            trans = new Vector3(-Horizontal * moveFactor, -Gravity * moveFactor, 0f);
        }
        else if (_myFacingDirection == FacingDirection.Left)
        {
            trans = new Vector3(0f, -Gravity * moveFactor, -Horizontal * moveFactor);
        }
        if (_jumping)
        {
            transform.Translate(Vector3.up * JumpHeight * Time.deltaTime);
        }


        charController.SimpleMove(trans);
    }

    public bool UpdateToFacingDirection(FacingDirection newDirection, float angle, Transform level, float worldUnits)
    {
        Vector3 centerposition = Vector3.zero;

        //transform.position = new Vector3((int)transform.position.x + 1f, transform.position.y, (int)transform.position.z + 1f);
        foreach (Transform platform in level)
        {
            if (Mathf.Abs(platform.transform.position.x - transform.position.x) < worldUnits / 2
                && Mathf.Abs(platform.transform.position.z - transform.position.z) < worldUnits / 2
                && Mathf.Abs(platform.transform.position.y + worldUnits - transform.position.y) < worldUnits / 2)
            {
                centerposition.Set(platform.transform.position.x, transform.position.y, platform.position.z);
                break;
            }
        }

        foreach (Transform platform in level)
        {
            if ((centerposition != Vector3.zero) 
                && (((spriteRenderer.flipX == true && newDirection == FacingDirection.Back && platform.transform.position == new Vector3(centerposition.x + 1, centerposition.y -1, centerposition.z)) ||
                (spriteRenderer.flipX == true && newDirection == FacingDirection.Front && platform.transform.position == new Vector3(centerposition.x - 1, centerposition.y -1, centerposition.z)) ||
                (spriteRenderer.flipX == true && newDirection == FacingDirection.Left && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z + 1)) ||
                (spriteRenderer.flipX == true && newDirection == FacingDirection.Right && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z - 1))) 
                || ((spriteRenderer.flipX == false && newDirection == FacingDirection.Back && platform.transform.position == new Vector3(centerposition.x - 1, centerposition.y - 1, centerposition.z)) ||
                (spriteRenderer.flipX == false && newDirection == FacingDirection.Front && platform.transform.position == new Vector3(centerposition.x + 1, centerposition.y - 1, centerposition.z)) ||
                (spriteRenderer.flipX == false && newDirection == FacingDirection.Left && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z - 1)) ||
                (spriteRenderer.flipX == false && newDirection == FacingDirection.Right && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z + 1)))))
            {
                transform.position = centerposition;
                _myFacingDirection = newDirection;
                degree = angle;
                return true;
            }
        }

        return false;
    }

    public IEnumerator JumpingWait()
    {
        yield return new WaitForSeconds(0.35f);
        //Debug.Log ("Returned jump to false");
        _jumping = false;
    }
}