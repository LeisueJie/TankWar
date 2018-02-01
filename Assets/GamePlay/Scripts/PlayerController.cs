using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	PlayerCharacter character;

	// Use this for initialization
	void Start () {
		character = GetComponent<PlayerCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		//攻击
		if (Input.GetButtonDown("Fire1")) {
			character.Attack();
		}
		//移动
		var h = Input.GetAxis ("Horizontal");
		var v = Input.GetAxis ("Vertical");
		character.Move (new Vector3(h,0,v));
		//旋转
		var lookDir = Vector3.forward * v + Vector3.right * h;
		if (lookDir.magnitude != 0) {
			character.Rotate (lookDir);
		}
	}
}
