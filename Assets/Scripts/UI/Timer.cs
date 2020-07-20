using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField]
	private Text timer;
	[SerializeField]
	private float elapsed = 0f;
	[SerializeField]
	private float endTime = 480f;
	[SerializeField]
	private Light Sun;
	[SerializeField]
	private AnimationCurve SunBrightness;
	[SerializeField]
	private GameObject endgamescreen;
    // Start is called before the first frame update
    void Start() {
        elapsed = 0f;
    }

    // Update is called once per frame
    void Update() {
        elapsed += Time.deltaTime;
		if (Sun != null) Sun.intensity = SunBrightness.Evaluate(elapsed / 60);
		timer.text = GetTime(elapsed, 2, 0) + " PM\nClosing at: "+GetTime(endTime, 2, 0)+" PM";
		if (elapsed >= endTime) {
			TimeUp();
			endgamescreen.SetActive(true);
			Destroy(this);
		}
    }

	public static string GetTime(float elapsed, int hoursOffset = 0, int minOffset = 0) {
		int hours = Mathf.FloorToInt(elapsed / 60) + hoursOffset;
		int minutes = Mathf.FloorToInt(elapsed) % 60 + minOffset;
		return hours.ToString("00") +":"+ minutes.ToString("00");
	}

	private void TimeUp() {
		var AIs = Resources.FindObjectsOfTypeAll<CustomerAI>();
		foreach (var AI in AIs) {
			AI.TimeUp();
		}
	}
}
