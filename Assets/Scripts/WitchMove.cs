using UnityEngine;
using System.Collections;

public class WitchMove : MonoBehaviour
{

    private int Horizontal = 1;

    public Animator anim;
    public float MovementSpeed = 5f;
    public float Gravity = 1f;
    public CharacterController charController;
    private FacedDirection _myFacedDirection;
    public float JumpHeight = 0f;
    public bool _jumping = false;
    private float degree = 0;
    private bool idle = true;

    public GameObject Rig;


    private int direction = 1;


    public FacedDirection CmdFacedDirection
    {

        set
        {
            _myFacedDirection = value;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            idle = false;
            //degree = 180;
            if (Horizontal > 0)
                Rig.transform.Rotate(new Vector3(0, 180, 0));
            Horizontal = -1;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            idle = false;
            //degree = 0;
            if (Horizontal < 0)
                Rig.transform.Rotate(new Vector3(0, 180, 0));

            Horizontal = 1;

        }
        else
        {
            idle = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !_jumping)
        {
            _jumping = true;
            StartCoroutine(JumpingWait());
        }

        if (anim)
        {
            anim.SetBool("Idle", idle);

            float moveFactor = MovementSpeed * Time.deltaTime * 10f;
            MoveCharacter(moveFactor);

        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), 8 * Time.deltaTime);


    }

    private void MoveCharacter(float moveFactor)
    {
        Vector3 trans = Vector3.zero;
        if (!idle)
        {
            if (_myFacedDirection == FacedDirection.Front)
                trans = new Vector3(Horizontal * moveFactor, -Gravity * moveFactor, 0f);
            else if (_myFacedDirection == FacedDirection.Right)
                trans = new Vector3(0f, -Gravity * moveFactor, Horizontal * moveFactor);
            else if (_myFacedDirection == FacedDirection.Back)
                trans = new Vector3(-Horizontal * moveFactor, -Gravity * moveFactor, 0f);
            else if (_myFacedDirection == FacedDirection.Left)
                trans = new Vector3(0f, -Gravity * moveFactor, -Horizontal * moveFactor);
            if (_jumping)
                transform.Translate(Vector3.up * JumpHeight * Time.deltaTime);
        }

        charController.SimpleMove(trans);
    }

    public bool UpdateToFacedDirection(FacedDirection newDirection, float angle, Transform level, float worldUnits)
    {
        Vector3 centerposition = Vector3.zero;

        //transform.position = new Vector3((int)transform.position.x + 1f, transform.position.y, (int)transform.position.z + 1f);
        foreach (Transform platform in level)
        {
            if (Mathf.Abs(platform.transform.position.x - transform.position.x) < worldUnits * 0.6
                && Mathf.Abs(platform.transform.position.z - transform.position.z) < worldUnits * 0.6
                && Mathf.Abs(platform.transform.position.y + worldUnits - transform.position.y) < worldUnits)
            {
                centerposition.Set(platform.transform.position.x, platform.transform.position.y + 1, platform.transform.position.z);
                break;
            }
        }

        foreach (Transform platform in level)
        {

            if ((centerposition != Vector3.zero)
                && (((Horizontal == -1 && newDirection == FacedDirection.Back && platform.transform.position == new Vector3(centerposition.x + 1, centerposition.y - 1, centerposition.z)) ||
                (Horizontal == -1 && newDirection == FacedDirection.Front && platform.transform.position == new Vector3(centerposition.x - 1, centerposition.y - 1, centerposition.z)) ||
                (Horizontal == -1 && newDirection == FacedDirection.Left && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z + 1)) ||
                (Horizontal == -1 && newDirection == FacedDirection.Right && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z - 1)))
                || ((Horizontal == 1 && newDirection == FacedDirection.Back && platform.transform.position == new Vector3(centerposition.x - 1, centerposition.y - 1, centerposition.z)) ||
                (Horizontal == 1 && newDirection == FacedDirection.Front && platform.transform.position == new Vector3(centerposition.x + 1, centerposition.y - 1, centerposition.z)) ||
                (Horizontal == 1 && newDirection == FacedDirection.Left && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z - 1)) ||
                (Horizontal == 1 && newDirection == FacedDirection.Right && platform.transform.position == new Vector3(centerposition.x, centerposition.y - 1, centerposition.z + 1)))))
            {
                transform.position = centerposition;
                _myFacedDirection = newDirection;
                degree = angle;
                return true;
            }
        }

        return false;
    }

    //public bool UpdateToFacedDirection(FacedDirection newDirection, float angle, char[][] level, float worldUnits)
    //{
    //    Vector3 centerposition = Vector3.zero;

    //    //transform.position = new Vector3((int)transform.position.x + 1f, transform.position.y, (int)transform.position.z + 1f);

    //    //if (level[(int)transform.position.x][-((int)transform.position.z)] == 'f')
    //    //    centerposition.Set((int)transform.position.x, 1, transform.position.z);

    //    //foreach (char[] platform in level)
    //    //{
    //    //    if (Mathf.Abs(platform.transform.position.x - transform.position.x) < worldUnits * 0.6
    //    //        && Mathf.Abs(platform.transform.position.z - transform.position.z) < worldUnits * 0.6
    //    //        && Mathf.Abs(platform.transform.position.y + worldUnits - transform.position.y) < worldUnits)
    //    //    {
    //    //        centerposition.Set(platform.transform.position.x, platform.transform.position.y + 1, platform.transform.position.z);
    //    //        break;
    //    //    }
    //    //}

    //    if (((Horizontal == -1 && newDirection == FacedDirection.Back && level[(int)(transform.position.x + 0.1) + 1][-((int)(transform.position.z - 0.1))] == 'f') ||
    //        (Horizontal == -1 && newDirection == FacedDirection.Front && level[(int)(transform.position.x + 0.1) - 1][-((int)(transform.position.z - 0.1))] == 'f') ||
    //        (Horizontal == -1 && newDirection == FacedDirection.Left && level[(int)(transform.position.x + 0.1)][-((int)(transform.position.z - 0.1) + 1)] == 'f') ||
    //        (Horizontal == -1 && newDirection == FacedDirection.Right && level[(int)(transform.position.x + 0.1)][-((int)(transform.position.z - 0.1) - 1)] == 'f'))
    //        || ((Horizontal == 1 && newDirection == FacedDirection.Back && level[(int)(transform.position.x + 0.1) - 1][-((int)(transform.position.z - 0.1))] == 'f') ||
    //        (Horizontal == 1 && newDirection == FacedDirection.Front && level[(int)(transform.position.x + 0.1) + 1][-((int)(transform.position.z - 0.1))] == 'f') ||
    //        (Horizontal == 1 && newDirection == FacedDirection.Left && level[(int)(transform.position.x + 0.1)][-((int)(transform.position.z - 0.1) - 1)] == 'f') ||
    //        (Horizontal == 1 && newDirection == FacedDirection.Right && level[(int)(transform.position.x + 0.1)][-((int)(transform.position.z - 0.1) + 1)] == 'f')))
    //    {
    //        //transform.position = new Vector3((int) transform.position.x, 1.05f, (int)transform.position.z);

    //        _myFacedDirection = newDirection;
    //        degree = angle;
    //        return true;
    //    }

    //    return false;
    //}


    public IEnumerator JumpingWait()
    {
        yield return new WaitForSeconds(0.35f);
        //Debug.Log ("Returned jump to false");
        _jumping = false;
    }

    public int GetHorizontal()
    {
        return Horizontal;
    }
}