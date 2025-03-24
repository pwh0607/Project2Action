using UnityEngine;

public class AbilityMoveKeyBoard : Ability<AbilityMoveKeyBoardData>
{
    float horz, vert;
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 direction;
    private float _velocity;

    public AbilityMoveKeyBoard(AbilityMoveKeyBoardData data, CharacterControl owner) : base(data, owner)
    {
        cameraTransform = Camera.main.transform; 
        _velocity = data.rotatePerSec;   
    }
    
    public override void FixedUpdate()
    {
        InputKeyboard();
        Rotate();
        Movement();
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    void InputKeyboard()
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        direction = (camForward * vert + camRight * horz).normalized;
    }

    void Movement()
    {
        // owner.rb.AddForce(movement * data.moveSpeed * Time.deltaTime * 10f);
        owner.cc.Move(direction * data.movePerSec * Time.deltaTime);
    }

    void Rotate()
    {
        if (direction == Vector3.zero) return;

        //Atan2의 역할 : Vector2(x,z)가 있을 시 해당 각도를 알려준다.(radian)
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothAngle = Mathf.SmoothDampAngle(owner.transform.eulerAngles.y, angle, ref _velocity, 0.1f);
        owner.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}