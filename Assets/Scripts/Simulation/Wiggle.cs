using UnityEngine;
using System.Collections;

public class Wiggle : MonoBehaviour {

	public Vector3 distance;
	public float speed;

	private Vector2 startPos, toPos;
	private float timeStart;

	void randomToPos() {
		toPos = startPos;
		toPos.x += Random.Range(-1.0f, +1.0f) * distance.x;
		toPos.y += Random.Range(-1.0f, +1.0f) * distance.y;
		timeStart = Time.time;
	}

	// Use this for initialization
	void Start () {
		startPos = (this.transform as RectTransform).anchoredPosition;
		randomToPos();
	}

	// Update is called once per frame
	void Update () {
		float d = (Time.time - timeStart) / speed;
		if (d > 1) {
			randomToPos();
		} else if (d < 0.5) {
            (this.transform as RectTransform).anchoredPosition = Vector2.Lerp(startPos, toPos, d);
		} else {
            (this.transform as RectTransform).anchoredPosition = Vector2.Lerp(toPos, startPos, d);
		}
	}
}