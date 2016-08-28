using UnityEngine;

using System;
using System.Linq;
using System.Collections;

using IBM.Watson.DeveloperCloud.Widgets;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;

enum RequestStatus
{
	FOUND,
	NOT_FOUND,
	NOT_SURE
}

public class Controller : MonoBehaviour {

	#region Private Data
	[SerializeField]
	private WebCamWidget m_WebCamWidget;

	[SerializeField]
	private float threshold = 0f;

	[SerializeField]
	private GameObject queenSpades;

	[SerializeField]
	private GameObject queenClubs;

	[SerializeField]
	private GameObject queenHearts;

	[SerializeField]
	private GameObject queenDiamonds;

	[SerializeField]
	private GameObject findCulprit;

	[SerializeField]
	private GameObject tryAgainGameObj;

	private string classifier_Id = "cards_785616624";

	private bool processingImage = false;
	//[SerializeField]
	//private WebCamDisplayWidget m_WebCamDisplayWidget;

	VisualRecognition m_VisualRecognition;
	#endregion

	public void SummonCulprit()
	{
		//Color32[] colors = m_WebCamWidget.WebCamTexture.GetPixels32();
		//byte[] rawImageData = Utility.Color32ArrayToByteArray(colors);

		Debug.Log ("[Controller]: Summoning cuplrit");

		Texture2D image = new Texture2D(m_WebCamWidget.WebCamTexture.width, m_WebCamWidget.WebCamTexture.height, TextureFormat.RGB24, false);
		image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());

		byte[] imageData = image.EncodeToPNG();

		/*
		 * request from Watson service which card it is
		 *  - if found - summon the card 
		 *  - if could not detect - try again
		 *  - if not 
		 */
		AskWatson (imageData);
		Card card = null;
		RequestStatus status = IdentityCulprit (imageData, out card);

	//	switch (status) {
	//	case RequestStatus.FOUND:
	//		HandleQueenSummon (card);
	//		break;
	//	case RequestStatus.NOT_FOUND:
	//		HandleQueenSummon (card);
	//		break;
	//	case RequestStatus.NOT_SURE:
	//		default:
	//		HandleNotSureCard();
	//	}

		// releasing imageData
		imageData = null;
	}

	#region Watson methods

	private void AskWatson(byte[] imageData)
	{
		Debug.Assert (m_VisualRecognition != null, "[Controller]: VisualRecognition is not recognized");

		if (!processingImage) {
			processingImage = true;

			string[] classifierIds = { classifier_Id };
			string[] owners = { "IBM", "me" };
			if (!m_VisualRecognition.Classify (OnClassify, imageData, owners, classifierIds)) {
				Debug.Log ("Classigy image failed");
				processingImage = false;
			}
		} else {
			Debug.Log ("Dr Watson is processing the previous image. Have Patience");
		}
	}

	private RequestStatus IdentityCulprit(byte[] imageData, out Card card)
	{
		card = new Card();
		/*
		 * Request watson with image data.
		 * parse result to identify which card
		 * infer result
		 */

		return RequestStatus.FOUND;
	}

	private void OnClassify(ClassifyTopLevelMultiple classify, string data)
	{
		processingImage = false;

		if (classify != null)
		{
			Debug.Log("WebCamRecognition" + " images processed: " + classify.images_processed);

			//foreach (ClassifyTopLevelSingle image in classify.images)
			{
				if (classify.images_processed >= 1 && classify.images.Length >= 1) {
					ClassifyTopLevelSingle image = classify.images [0];

					Debug.Log ("WebCamRecognition" + " \tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url);

					CardClassResult result = new CardClassResult ();

					foreach (ClassifyPerClassifier classifier in image.classifiers) {
						Debug.Log ("WebCamRecognition" + " \t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name);
						if (classifier.name == "cards") {
							foreach (ClassResult classResult in classifier.classes) {
								result.AddResult (classResult.m_class, classResult.score);
								Debug.Log ("WebCamRecognition" + " \t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy);
							}
						}

						InferWatsonResult (result);
					}
				} else {
					tryAgainGameObj.SetActive (true);
				}
			}
		}
		else
		{
			Debug.Log("WebCamRecognition" + " Classification failed!");
			tryAgainGameObj.SetActive (true);
		}
	}

	private void InferWatsonResult(CardClassResult result)
	{
		int maxIndex = 0;

		double maxScore = 0;

		for (int i = 0; i < result.QueenScore.Length; ++i) {
			if (result.QueenScore [i] > maxScore) {
				maxIndex = i;
				maxScore = result.QueenScore [i];
			}
		}

		if (maxScore > 0)
			EnableQueen ((QueenIndex)maxIndex);
		
		// result.QueenScore
		result = null;
	}
	#endregion

	#region Queen Methods

	private void EnableQueen(QueenIndex queenIndex)
	{
		Debug.Log ("Enabling queen: " + queenIndex);
		if (queenIndex != QueenIndex.NOT_FOUND) {
			switch (queenIndex) {
			case QueenIndex.CLUB:
				queenClubs.SetActive (true);
				break;
			case QueenIndex.SPADES:
				queenSpades.SetActive (true);
				break;
			case QueenIndex.HEARTS:
				queenHearts.SetActive (true);
				break;
			case QueenIndex.DIAMONDS:
				queenDiamonds.SetActive (true);
				break;
			}
		}

		if (queenClubs.activeInHierarchy && queenSpades.activeInHierarchy && queenHearts.activeInHierarchy && queenDiamonds.activeInHierarchy)
			findCulprit.SetActive (true);
	}

	#endregion

	#region Mono Methods

	void Awake()
	{
		Debug.Assert (m_WebCamWidget != null, "[Controller]: Web camera widget is not assigned");

		m_VisualRecognition = new VisualRecognition();
	}

	void Start()
	{
		queenSpades.SetActive (false);
		queenClubs.SetActive (false);
		queenHearts.SetActive (false);
		queenDiamonds.SetActive (false);
		findCulprit.SetActive (false);
		tryAgainGameObj.SetActive (false);
	}

	#endregion
}

public class Card
{
}

public class CardClassResult
{
	public CardClassResult()
	{
		QueenScore = new double[4];
		for (int i = 0; i < 4; ++i) {
			QueenScore [i] = 0;
		}
	}

	public void AddResult(string classType, double score)
	{
		QueenIndex index = ConvertClassType (classType);

		if (index != QueenIndex.NOT_FOUND)
			QueenScore [(int)index] = score;
	}

	public double[] QueenScore { get; private set; }

	private QueenIndex ConvertClassType(string classType)
	{
		switch (classType) {
		case "queen_spades":
			return QueenIndex.SPADES;
		case "queen_clubs":
			return QueenIndex.CLUB;
		case "queen_hearts":
			return QueenIndex.HEARTS;
		case "queen_diamonds":
			return QueenIndex.DIAMONDS;
		default:
			Debug.LogWarning ("watson returned: " + classType);
			return QueenIndex.NOT_FOUND;
		}

	}
}

enum QueenIndex
{
	HEARTS = 0 ,
	DIAMONDS = 1,
	CLUB = 2,
	SPADES = 3,
	NOT_FOUND
}
