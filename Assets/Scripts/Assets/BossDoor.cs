using UnityEngine;

public class BossDoor : MonoBehaviour
{

    private Animator animator;
    private BossLevelManager bossLevelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();    
        bossLevelManager = GameObject.Find("LevelManager").GetComponent<BossLevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.position.x < collision.transform.position.x)
            {
                animator.SetBool("Close", true);
                GetComponent<Collider2D>().isTrigger = false;
                bossLevelManager.StartBattle();
            }
        }
    }

}
