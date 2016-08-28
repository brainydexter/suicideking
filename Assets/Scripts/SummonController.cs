using UnityEngine;
using System.Collections;

using IBM.Watson.DeveloperCloud.Widgets;

public class SummonController : MonoBehaviour {

	void OnEnable(){
		webcamWidget.ActivateWebCam ();

		mirror.gameObject.SetActive (true);

		meshRenderer.material.mainTexture = texture;

		cameraController.gameObject.SetActive (true);
	}

	void OnDisable(){
		webcamWidget.DeactivateWebCam ();

		mirror.gameObject.SetActive (false);

		cameraController.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Awake () {
		webcamWidget.DeactivateWebCam ();
		meshRenderer.material.mainTexture = texture;
	}

	void Start(){
	}

	// Update is called once per frame
	void Update () {
	}

	#region Mono Methods
	[SerializeField] private Texture texture;
	[SerializeField] StoryController storyController;

	[SerializeField] MeshRenderer meshRenderer;

	[SerializeField] GameObject mirror;
	[SerializeField] WebCamWidget webcamWidget;
	[SerializeField] Controller cameraController;
	#endregion
}
