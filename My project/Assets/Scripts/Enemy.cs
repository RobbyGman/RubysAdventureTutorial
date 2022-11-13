using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
	public float speed;
	public float timeToChange;
	public bool horizontal;

	public ParticleSystem smokeEffect;
	public ParticleSystem fixedEffect;
	public AudioClip hitSound;
	private int fixCount;
	public AudioClip fixedSound;
	Rigidbody2D rigidbody2d;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;
	bool broken = true;
	
	Animator animator;
	private RubyController rubyController;
	AudioSource audioSource;

	// Start is called before the first frame update
	void Start ()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		remainingTimeToChange = timeToChange;

		direction = horizontal ? Vector2.right : Vector2.down;

		animator = GetComponent<Animator>();

		audioSource = GetComponent<AudioSource>();
		GameObject rubyControllerObject = GameObject.FindWithTag("Player");
		rubyController = rubyControllerObject.GetComponent<RubyController>();
	}
	
    // Update is called once per frame
	void Update()
	{
		if(!broken)
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
		if(!broken)
			return;
		
		RubyController controller = other.collider.GetComponent<RubyController>();
		
		if(controller != null)
		{
			controller.ChangeHealth(-1);
		}

		
	}

	public void Fix()
	{
		animator.SetTrigger("Fixed");
		broken = false;
		smokeEffect.Stop();

		if (rubyController != null)
		{
			rubyController.FixedText(1);
		}

		rigidbody2d.simulated = false;
		Instantiate(fixedEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
		
		audioSource.Stop();
		audioSource.PlayOneShot(hitSound);
		audioSource.PlayOneShot(fixedSound);
	}
}
