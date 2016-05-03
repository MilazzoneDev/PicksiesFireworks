using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollingViewer : MonoBehaviour {

	public float speed;
	public float separation;

	private ArrayList scrollingObjects;

	// Use this for initialization
	void Start () {
		//fill the objects we need to move
		scrollingObjects = new ArrayList();
		for(int i = 0; i<this.transform.childCount; i++)
		{
			scrollingObjects.Add(this.transform.GetChild(i));
		}
		SetUp();
	}

	void SetUp()
	{
		for(int i = 0; i<scrollingObjects.Count; i++)
		{
			Transform objectToScroll = (Transform)scrollingObjects[i];
			RectTransform rectToScroll = objectToScroll.gameObject.GetComponent<RectTransform>();

			Vector2 anchorMax = rectToScroll.anchorMax;
			Vector2 anchorMin = rectToScroll.anchorMin;

			if(i < 1)
			{
				float anchorWidth = anchorMax.x - anchorMin.x;
				anchorMin.x = 0;
				anchorMax.x = anchorMin.x + anchorWidth;
			}
			else
			{
				int lastObject;
				lastObject = i-1;

				float anchorWidth = anchorMax.x - anchorMin.x;
				anchorMin.x = ((Transform)scrollingObjects[lastObject]).gameObject.GetComponent<RectTransform>().anchorMax.x + separation;
				anchorMax.x = anchorMin.x + anchorWidth;
			}

			anchorMin.y = 0;
			anchorMax.y = 1;

			rectToScroll.anchorMax = anchorMax;
			rectToScroll.anchorMin = anchorMin;

			rectToScroll.offsetMin = Vector2.zero;
			rectToScroll.offsetMax = Vector2.zero;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(this.transform.parent.gameObject.activeSelf)
		{
			float dt = Time.deltaTime;
			for(int i = 0; i<scrollingObjects.Count; i++)
			{
				Transform objectToScroll = (Transform)scrollingObjects[i];
				RectTransform rectToScroll = objectToScroll.gameObject.GetComponent<RectTransform>();

				Vector2 anchorMax = rectToScroll.anchorMax;
				Vector2 anchorMin = rectToScroll.anchorMin;
				if(anchorMax.x < 0)
				{
					int lastObject;
					if(i < 1)
					{
						lastObject = scrollingObjects.Count-1;
					}
					else
					{
						lastObject = i-1;
					}
					float anchorWidth = anchorMax.x - anchorMin.x;
					anchorMin.x = ((Transform)scrollingObjects[lastObject]).gameObject.GetComponent<RectTransform>().anchorMax.x + separation;
					anchorMax.x = anchorMin.x + anchorWidth;

				}
				anchorMax.x -= dt*speed;
				anchorMin.x -= dt*speed;
				rectToScroll.anchorMax = anchorMax;
				rectToScroll.anchorMin = anchorMin;
			}
		}
	}
}
