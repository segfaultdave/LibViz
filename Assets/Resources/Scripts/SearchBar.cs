﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SearchBar : MonoBehaviour, IBeginDragHandler, IDropHandler {
	Whirlwind whirlwind;
	Transform content;

	// keep slot sorted
	List<SearchSlot> slots;

	// Use this for initialization
	void Start () {
		whirlwind = GameObject.Find("WhirlwindCenter").GetComponent<Whirlwind>();
		content = transform.Find("Viewport/Content");
		slots = new List<SearchSlot>();
	}

	public void OnBeginDrag(PointerEventData eventData) {
		
	}
	
	public void OnDrop(PointerEventData eventData) {
		if (whirlwind.IsDraggingSearchItem) {

			print(eventData.pressPosition);

			// create a new slot
			SearchSlot newSlot = ((GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/SearchSlot"))).GetComponent<SearchSlot>();
			newSlot.transform.SetParent(content);
			newSlot.transform.localScale = Vector3.one;

			// TODO search through all the slots, find the index to put this guy in
			int i = 0;
			for (i = 0; i < slots.Count; i++) {

			}

			newSlot.transform.SetSiblingIndex(i);
			newSlot.SetDraggedSearchItem(whirlwind.DraggedSearchItem);
			slots.Insert(i, newSlot);
		}
		
	}

}
