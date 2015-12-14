﻿using UnityEngine;
using System.Collections;

public class WhirlwindBelt : MonoBehaviour {
	[Range(1, 5)]
	public int level;

	float radius;
	float height;
	float speed;

	float prevMouseX;

	WhirlwindObject[] wwObjs;

	// Use this for initialization
	void Start () {
		height = (float)level * 2f + 1f;
		radius = height / 9f * 8f;
		speed = 5f;

		wwObjs = GetComponentsInChildren<WhirlwindObject>();
		for (int i = 0; i < wwObjs.Length; i++) {
			wwObjs[i].speed = speed;
			wwObjs[i].height = height;
			wwObjs[i].radius = radius;
		}
	}

	void OnmouseDown () {
		prevMouseX = Input.mousePosition.x;
	}

	// change orbiting speed of this belt
	void OnMouseDrag () {
		// TODO make sure the state is right for user interaction
		float mouseX = Input.mousePosition.x;
		float d = mouseX - prevMouseX;
		prevMouseX = mouseX;
		float s = Mathf.Max(Mathf.Min(Mathf.Abs(d/10f), 5f), 1f);
		print(d);

		if (wwObjs[0].currentState == WhirlwindObject.State.Orbit) {
			for (int i = 0; i < wwObjs.Length; i++) {
				if (d > 1f) {
					wwObjs[i].speed = speed * s;
				} else if (d < -1f) {
					wwObjs[i].speed = speed * -s;
				} else {
					wwObjs[i].speed = 0f;
				}
				
			}
		}
	}

	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
}
