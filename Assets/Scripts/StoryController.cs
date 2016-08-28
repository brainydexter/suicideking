using UnityEngine;
using System.Collections;

public class StoryController : MonoBehaviour {

	enum StoryState
	{
		Intro,
		Summon,
		Examine,
		Answer
	}

	#region State methods

	public void TransitionToIntro()
	{
		state = StoryState.Intro;
		DisableAll ();

		introGameObj.SetActive (true);
	}

	public void TransitionToSummon()
	{
		state = StoryState.Summon;
		DisableAll ();
		summonGameObj.SetActive (true);
	}

	public void TransitionToExamine()
	{
		state = StoryState.Examine;
		DisableAll ();

		examineGameObj.SetActive (true);
	}

	public void TransitionToAnswer()
	{
		state = StoryState.Examine;
		DisableAll ();

		answerGameObj.SetActive (true);
	}

	private void DisableAll()
	{
		introGameObj.SetActive (false);;
		summonGameObj.SetActive (false);;
		examineGameObj.SetActive (false);;
		answerGameObj.SetActive (false);;
	}

	#endregion

	#region Mono Methods
	// Use this for initialization
	void Awake () {
		TransitionToIntro ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion

	#region private data
	StoryState state;

	[SerializeField]
	GameObject introGameObj;

	[SerializeField]
	GameObject summonGameObj;
	[SerializeField] GameObject examineGameObj;
	[SerializeField] GameObject answerGameObj;
	#endregion
}
