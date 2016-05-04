﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FullscreenSelectionUI : MonoBehaviour {

	Text fields;
	Image itemImage;
	GameObject background;

	BookInfo currentBookInfo;
	SpriteModel spriteModel;
	BillBoardRenderer billBoardRenderer;

	// Use this for initialization
	void Start () {
		fields = transform.Find("Fields/Viewport/Content/Text").GetComponent<Text>();
		itemImage = transform.Find("ItemBackground/ItemImage").GetComponent<Image>();
		background = transform.Find("Background").gameObject;
		spriteModel = GameObject.Find("StaticSpriteModel-Mono").GetComponent<SpriteModel>();
		billBoardRenderer = GameObject.Find("StaticSpriteModel-Mono/BillboardRenderer").GetComponent<BillBoardRenderer>();

		Enable(false);
	}

	public void Enable (bool enabled) {
		if (enabled) {
			Debug.Assert(currentBookInfo != null);
		}

		background.GetComponent<Collider>().enabled = enabled;
		GetComponent<Canvas>().enabled = enabled;

		if (enabled) {
			spriteModel.videoFileName = currentBookInfo.FileName + ".mp4";
			billBoardRenderer.LoadMovie();
		} else {
		}

	}

	public void SetBookInfo (BookInfo bookInfo, Sprite sprite) {
		currentBookInfo = bookInfo;
		fields.text = 
			bookInfo.Title + "\n\n" + 
			bookInfo.Author + "\n\n" + 
			bookInfo.GetData("pub_date") + "\n\n";
		itemImage.sprite = sprite;
	}

	// Update is called once per frame
	void Update () {
	}
}
