using UnityEngine;

public class Ladder : Interactable
{
    [SerializeField] private float climbSpeed = 2.5f;
    private bool isClimbing = false;
    private Rigidbody2D playerRb;
    private PlayerController playerController;

    public override void Interact()
    {
        print("dmoasmd");
        base.Interact();

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null) return;
        }

        isClimbing = !isClimbing;
        playerController.SetClimbing(isClimbing, this);
    }

    public void Climb()
    {
        if (playerRb == null) return;

        float vertical = Input.GetAxisRaw("Vertical");
        playerRb.velocity = new Vector2(0, vertical * climbSpeed);
    }

    public void SetPlayerRigidbody(Rigidbody2D rb)
    {
        playerRb = rb;
    }

    protected override void OnEnterRange()
    {
        Debug.Log(interactionText);
    }

    protected override void OnExitRange()
    {
        if (isClimbing)
        {
            Interact(); // Автоматически слезть при выходе
        }
    }
}