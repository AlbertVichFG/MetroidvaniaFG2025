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

                break;

            case "ExtraDmg":


                break;
            case "Crouch":


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
            }
        }
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

                break ;

            case "ExtraDmg":


                break ;

            case "Crouch":


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
}
