using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour {

	void OnEnable(){
		startTimer = true;
		timerAccumulated = 0f;

		textureIndex = 0;
		meshRenderer.material.mainTexture = textures [textureIndex];
	}

	// Use this for initialization
	void Awake () {
		meshRenderer.material.mainTexture = textures [textureIndex];
	}
	
	// Update is called once per frame
	void Update () {

		if (startTimer) {
			timerAccumulated += Time.deltaTime;

			if (timerAccumulated > interval) {
				AdvanceTexture ();

				timerAccumulated = 0f;
			}
		}
	}

	void AdvanceTexture()
	{
		++textureIndex;

		if (textureIndex == textures.Length) {
			storyController.TransitionToSummon ();

			startTimer = false;
			this.gameObject.SetActive (false);
		} else {
			meshRenderer.material.mainTexture = textures [textureIndex];
		}
	}

	#region Mono Methods
	[SerializeField] Texture[] textures;
	[SerializeField] StoryController storyController;

	private int textureIndex = 0;

	[SerializeField] MeshRenderer meshRenderer;

	bool startTimer = false;
	float timerAccumulated = 0;

	[SerializeField] float interval = 5f;
	#endregion
}
