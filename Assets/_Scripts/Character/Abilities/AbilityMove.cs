using UnityEngine;

public class AbilityMove : Ability
{
    public override AbilityData Data => Data as AbilityMoveData;

    private float movespeed;  
    private float rotatespeed;

    float horz, vert;
    private Transform cameraTransform;
    private Vector3 camForward, camRight;
    private Vector3 movement;


    public AbilityMove(CharacterControl owner, float movespeed, float rotatespeed) : base(owner)
    {        
        this.movespeed = movespeed;
        this.rotatespeed = rotatespeed;

        cameraTransform = Camera.main.transform;    
    }
    
    public override void Update()
    {
        InputKeyboard();
        Rotate();
        Movement();
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    void InputKeyboard()
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        
        movement = (camForward * vert + camRight * horz).normalized;
    }

    void Movement()
    {
        owner.rb.AddForce(movement * movespeed * Time.deltaTime * 10f);
    }

    Quaternion targetRotation = Quaternion.identity;
    void Rotate()
    {
        if (movement != Vector3.zero)
            targetRotation = Quaternion.LookRotation(movement);

        owner.transform.rotation = Quaternion.Slerp(owner.rb.rotation, targetRotation, rotatespeed * Time.deltaTime);
    }
}
