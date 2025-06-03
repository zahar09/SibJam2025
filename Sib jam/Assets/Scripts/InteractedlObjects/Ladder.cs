using UnityEngine;

public class Ladder : Interactable
{
    [SerializeField] private float climbSpeed = 2.5f;
    [SerializeField] private LadderUpperBound upperBound;
    [SerializeField] private GameObject upperPlatform;
    private bool isClimbing = false;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private bool playerInUpperTrigger;
    private int diration = 1;



    public override void Interact()
    {
        print("dmoasmd");
        //base.Interact();
        



        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null) return;
        }

        isClimbing = !isClimbing;
        playerController.SetClimbing(isClimbing, this);
        playerController.gameObject.transform.position = new Vector3(
                                                                        transform.position.x,
                                                                        playerController.transform.position.y,
                                                                        playerController.transform.position.z
                                                                        );
        diration = 1;
        upperPlatform.SetActive(true);
        playerInUpperTrigger = upperBound.GetisPlayerInTrigger();
        if (playerInUpperTrigger && isClimbing)
        {
            diration = -1;
            upperPlatform.SetActive(false);
        }
    }

    public void Climb()
    {
        if (playerRb == null) return;

        

        // Убираем зависимость от ввода игрока
        float vertical = diration; // всегда вверх

        // Устанавливаем вертикальную скорость
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

    public int GetDiration()
    {
        return diration;
    }
}