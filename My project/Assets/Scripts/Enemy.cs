using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed;
	public float timeToChange;
	public bool horizontal;

	public ParticleSystem smokeEffect;
	public AudioClip hitSound;
	public AudioClip fixedSound;
	
	Rigidbody2D rigidbody2d;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;
	bool repaired = false;
	
	Animator animator;
	
	AudioSource audioSource;

	// Start is called before the first frame update
	void Start ()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		remainingTimeToChange = timeToChange;

		direction = horizontal ? Vector2.right : Vector2.down;

		animator = GetComponent<Animator>();

		audioSource = GetComponent<AudioSource>();
	}
	
    // Update is called once per frame
	void Update()
	{
		if(repaired)
		{
			return;
		}

		remainingTimeToChange -= Time.deltaTime;

		if (remainingTimeToChange <= 0)
		{
			remainingTimeToChange += timeToChange;
			direction *= -1;
		}

		animator.SetFloat("ForwardX", direction.x);
		animator.SetFloat("ForwardY", direction.y);
	}

	void FixedUpdate()
	{
		rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		if(repaired)
			return;
		
		RubyController controller = other.collider.GetComponent<RubyController>();
		
		if(controller != null)
			controller.ChangeHealth(-1);
	}

	public void Fix()
	{
		animator.SetTrigger("Fixed");
		repaired = true;
		
		smokeEffect.Stop();

		rigidbody2d.simulated = false;
		
		audioSource.Stop();
		audioSource.PlayOneShot(hitSound);
		audioSource.PlayOneShot(fixedSound);
	}
}
