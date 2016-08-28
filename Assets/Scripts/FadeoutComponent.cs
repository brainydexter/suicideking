using UnityEngine;
using System.Collections;

public class FadeoutComponent : MonoBehaviour {

	#region Mono Methods

	void OnEnable()
	{
		Debug.Log ("Enabling fadeout");
		startTimer = true;
	}

	void Update()
	{
		if (startTimer) {
			timeAccumulated += Time.deltaTime;

			if (timeAccumulated >= threshold) {
				startTimer = false;
				timeAccumulated = 0;

				gameObject.SetActive (false);
			}
		}
	}

	#endregion

	#region private data

	private float timeAccumulated;

	private bool startTimer = false;

	[SerializeField]
	private float threshold = 5f;

	#endregion
}
