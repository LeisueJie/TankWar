using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	public float explosionRadius;

	public LayerMask damageMask;

	public float damage = 20f;

	public AudioSource explosionAudioSource;
	public ParticleSystem explosionEffect;

	public bool isRotate = false;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 4);
		if (isRotate) {
			GetComponent<Rigidbody> ().AddTorque (transform.right * 1000);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var colliders = Physics.OverlapSphere (transform.position, explosionRadius, damageMask);
		foreach (var collider in colliders) {
			var target = collider.GetComponent<PlayerCharacter> ();
			if (target) {
				target.TakeDamage (damage);
			}
		}

		explosionAudioSource.Play ();
		explosionEffect.transform.parent = null;
		explosionEffect.Play ();

		ParticleSystem.MainModule mainModule = explosionEffect.main;
		Destroy (explosionEffect.gameObject,mainModule.duration);

		Destroy (gameObject);

	}

}
