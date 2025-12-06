using Unity.VisualScripting;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private bool inTrigger;
    [SerializeField]
    private GameObject iconUI;
    private Animator animator;

    [SerializeField]
    private ParticleSystem particl;
    [SerializeField]
    private string gemmName;
    [SerializeField]
    private Sprite openChestSprite;
    [SerializeField]
    private bool chestIsOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        switch (gemmName)
        {
            case "DobleJump":
                if (GameManager.instance.GetGameData.MaxJumps > 1)
                {
                    GetComponent<Collider2D>().enabled = false;
                }
                break;

            case "Dash":
                if (GameManager.instance.GetGameData.CanDash == true)
                {
                    GetComponent<Collider2D>().enabled = false;
                }
                break;

            case "FireBall":
                if (GameManager.instance.GetGameData.HasFireBall == true)
                {
                    GetComponent<Collider2D>().enabled = false;
                }

                break;
            case "Crouch":
                if (GameManager.instance.GetGameData.CanCrouch == true)
                {
                    GetComponent<Collider2D>().enabled = false;
                }

                break;
            case "GrabWall":
                if (GameManager.instance.GetGameData.CanGrabWall == true)
                {
                    GetComponent<Collider2D>().enabled = false;
                    LetChestOpen();
                }

                break;
            case "Heal":
                if (GameManager.instance.GetGameData.CanHeal == true)
                {
                    GetComponent<Collider2D>().enabled = false;
                    LetChestOpen();
                }

                break;

            default:

                break;

        }

        


        
    }

    private void Update()
    {
        if (inTrigger == true)
        {
            if (Input.GetButtonDown("Action"))
            {
                animator.SetTrigger("OpenChest");
               // animator.updateMode = AnimatorUpdateMode.Normal;
                iconUI.SetActive(false);
                Time.timeScale = 0;
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                chestIsOpen = true;
            }
        }
/*
        if (chestIsOpen == true)
        {
            GetComponent<SpriteRenderer>().sprite = openChestSprite; 

        }*/
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" )
        {
            inTrigger = true;
            iconUI.SetActive(true);
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inTrigger = false;
            iconUI.SetActive(false);
        }
    }


    public void ObtenGemm()
    {
        switch (gemmName)
        {
            case "DobleJump":
                GameManager.instance.GetGameData.MaxJumps = 2;
                break;

            case "Dash":
                GameManager.instance.GetGameData.CanDash = true;
                break ;

            case "FireBall":
                GameManager.instance.GetGameData.HasFireBall = true;

                break ;

            case "Crouch":
                GameManager.instance.GetGameData.CanCrouch = true;

                break;

            case "GrabWall":
                GameManager.instance.GetGameData.CanGrabWall = true;

                break;

            case "Heal":
                GameManager.instance.GetGameData.CanHeal = true;

                    break;
            default:

                break;

        }
        
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        
    }

    public void ActivateParticles()
    {
        particl.Play();
    }

    public void LetChestOpen()
    {
        GetComponent<SpriteRenderer>().sprite = openChestSprite;
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite);

    }
}
