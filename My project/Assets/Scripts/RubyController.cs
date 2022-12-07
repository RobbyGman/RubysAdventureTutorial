using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public GameObject projectilePrefab;
    public AudioClip hitSound;
    public AudioClip throwSound;
    public AudioClip Music;
    public AudioClip Win;
    public AudioClip Lose;
    public AudioClip ammoSound;

    public int health
    {
        get 
        { 
            return currentHealth; 
        }
    }
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rb;

    Vector2 currentInput;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(0, -1);
    AudioSource audioSource;
    public ParticleSystem hitParticle;
    public ParticleSystem ammoParticle;
    public TextMeshProUGUI fixedText;
    public TextMeshProUGUI cogsText;
    private int fixCount;
    private int cogsCount;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    bool gameOver;
    public static int level = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
                
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;
        
        animator = GetComponent<Animator>();
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Music;
        audioSource.Play();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        gameOver = false;
        
        SetFixedText();

        cogsCount = 4;
        SetCogText();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        currentInput = move;

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  

                if (fixCount == 4)
                {
                    SceneManager.LoadScene("Level 2");
                    level = 2;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                speed = 3.0f;
                gameOver = false;
            }
        }
 
    }

    void FixedUpdate()
    {
        Vector2 position = rb.position;
        
        position = position + currentInput * speed * Time.deltaTime;
        
        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        { 
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(hitSound);

            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if (currentHealth == 0)
        {
            loseTextObject.SetActive(true);
            gameOver = true;
            audioSource.Stop();

            audioSource.PlayOneShot(Lose);

            Destroy(gameObject.GetComponent<SpriteRenderer>());
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            speed = 0;
        }
        
        UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void FixedText(int amount)
    {
        fixCount += amount;

        SetFixedText();

        if (fixCount >= 4 && level == 1)
        {
            level = 2;
        }
        else if (level == 2 && fixCount == 4)
        {
            audioSource.Stop();
            winTextObject.SetActive(true);

            audioSource.PlayOneShot(Win);

            Destroy(gameObject.GetComponent<SpriteRenderer>());
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            
            gameOver = true;
            level = 1;
            speed = 0;
        } 
    }

    void Launch()
    {
        if (cogsCount > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
        
            animator.SetTrigger("Launch");
            audioSource.PlayOneShot(throwSound);

            cogsCount -= 1;
            SetCogText();
        }
    }
    public void SetFixedText()
    {
        fixedText.text = "Fixed: " + fixCount.ToString() + "/4";
    }



    void SetCogText()
    {
        cogsText.text = "Cogs: " + cogsCount.ToString();
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ammo")
        {
            audioSource.PlayOneShot(ammoSound);
            cogsCount += 4;
            SetCogText();
            Destroy(other.gameObject);
        }
    }

}
