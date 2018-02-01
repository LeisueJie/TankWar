using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour {

	public float speed;

	CharacterController cc;
	Animator animator;

	bool isAlive = true;

	public float turnSpeed;//旋转速度

	public Rigidbody shell;//弹药
	public Transform muzzle;
	public float launchForce = 10f;

	public AudioSource shootAudioSource;

	bool attacking = false;//是否在攻击中
	public float attackTime;//攻击间隔时间

	float hp;
	public float hpMax = 100;

	public Slider hpSlider;
	public Image hpFillImage;
	public Color hpColorFull = Color.green;
	public Color hpColorNull = Color.red;

	public ParticleSystem explosionEffect;

	public void Start()
	{
		cc = GetComponent<CharacterController>();
		animator = GetComponentInChildren<Animator> ();

		hp = hpMax;
		RefreshHealthHUD ();
	}

	void FixedUpdate()
	{
		if (transform.position.y < -1f) {
			Die ();
		}
	}

	public void Move(Vector3 v)//移动
	{
		//Debug.Log ("1");
		if(!isAlive) return;
		if (attacking) return;

		Vector3 movement = v * speed;
		cc.SimpleMove (movement);
		if (animator) 
		{
			animator.SetFloat ("Speed", cc.velocity.magnitude);
		}
	}

	public void Attack()//攻击
	{
		if (!isAlive) return;
		if (attacking) return;

		var shellInstance = Instantiate (shell, muzzle.position, muzzle.rotation) as Rigidbody;
		shellInstance.velocity = launchForce * muzzle.forward;//给弹药一个冲力
		if (animator) {
			animator.SetTrigger ("Attack");
		}
		attacking = true;
		shootAudioSource.Play ();
		Invoke ("RefreshAttack",attackTime);
	}
	void RefreshAttack()
	{
		attacking = false;
	}

	public void Rotate(Vector3 lookDir)//旋转
	{
		if(!isAlive) return;

		var targetPos = transform.position + lookDir;
		var characterPos = transform.position;
		characterPos.y = 0;
		targetPos.y = 0;

		var faceToTargetDir = targetPos - characterPos;

		var faceToQuat = Quaternion.LookRotation (faceToTargetDir);
		                                     //起点              //终点
		Quaternion slerp = Quaternion.Slerp (transform.rotation, faceToQuat, turnSpeed * Time.deltaTime);//求面差值
		transform.rotation = slerp;
	}

	public void TakeDamage(float count)//承受伤害
	{
		hp -= count;
		RefreshHealthHUD ();
		if (hp <= 0f && isAlive) {
			Die ();
		}

	}
	public void RefreshHealthHUD()
	{
		hpSlider.value = hp;
		hpFillImage.color = Color.Lerp (hpColorNull,hpColorFull,hp/100);
	}

	public void Die()//死亡
	{
		isAlive = false;
		explosionEffect.transform.parent = null;
		gameObject.gameObject.SetActive (true);
		ParticleSystem.MainModule mainModule = explosionEffect.main;
		Destroy (explosionEffect.gameObject,mainModule.duration);
		gameObject.SetActive (false);
		UnityEditor.EditorApplication.isPlaying = false;

	}
}
