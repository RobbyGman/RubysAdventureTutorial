using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scientist : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private Rigidbody2D rb; 
    private float distance;
    Animator animator;
    private RubyController rubyController;
    public AudioClip contact;
    AudioSource audioSource;
    public ParticleSystem hit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        Vector2 direction = player.transform.position - transform.position;

        direction.Normalize();

        float angle = Mathf.Atan(direction.x) * Mathf.Rad2Deg;

        if (distance < 7)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        
    }
    void OnCollisionStay2D(Collision2D other)
	{
		RubyController controller = other.collider.GetComponent<RubyController>();
		
		if(controller != null)
		{
			controller.ChangeHealth(-1);
            audioSource.PlayOneShot(contact);
            Instantiate(hit, transform.position + Vector3.up * 0.5f, Quaternion.identity);
		}

	}
}
