using UnityEngine;
using System.Collections;

public class ExamineController : MonoBehaviour {

	void OnEnable()
	{
		meshRenderer.material.mainTexture = examineTexture;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region private data

	[SerializeField] Texture examineTexture;

	[SerializeField] Texture[] hintTextures;

	[SerializeField] MeshRenderer meshRenderer;

	#endregion
}
