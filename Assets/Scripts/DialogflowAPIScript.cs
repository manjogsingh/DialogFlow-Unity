using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using JsonData;
using UnityEngine.UI;
using TMPro;

enum DialogType { Bot, Human }

public class DialogflowAPIScript : MonoBehaviour
{
	public ServiceSetting accessSettings = null;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	void Update()
	{
		if (Input.GetKeyDown("return"))
		{
			InputFieldControl();
		}
	}

	IEnumerator PostRequest()
	{
		string session = "unityClient";
		string accessToken = string.Empty;
		while (!JWTAuth.TryGetToken(
			accessSettings.ServiceAccount, out accessToken))
			yield return JWTAuth.GetToken(
				accessSettings.CredentialsFileName,
				accessSettings.ServiceAccount);

		string url = string.Format("https://dialogflow.googleapis.com/v2/projects/{0}/agent/sessions/{1}:detectIntent",
			accessSettings.ProjectId,
			session);

		UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
		postRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);
		postRequest.SetRequestHeader("Content-Type", "application/json");


		RequestBody requestBody = new RequestBody();
		requestBody.queryInput = new QueryInput();
		requestBody.queryInput.text = new TextInput();
		requestBody.queryInput.text.text = dialogString;
		requestBody.queryInput.text.languageCode = "en-US";

		string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
		Debug.Log(jsonRequestBody);

		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
		//Debug.Log(bodyRaw);
		postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

		yield return postRequest.SendWebRequest();

		if (postRequest.isNetworkError || postRequest.isHttpError)
		{
			Debug.Log(postRequest.responseCode);
			Debug.Log(postRequest.error);
		}
		else
		{
			// Show results as text
			Debug.Log("Response: " + postRequest.downloadHandler.text);

			// Or retrieve results as binary data
			byte[] resultbyte = postRequest.downloadHandler.data;
			string result = System.Text.Encoding.UTF8.GetString(resultbyte);
			ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);

			CreateMessageDialogBox(content.queryResult.fulfillmentText, DialogType.Bot);

			Debug.Log(content.queryResult.fulfillmentText);
			//ResultText.text = content.queryResult.fulfillmentText;
		}
	}

	public GameObject dialogBotPrefab;
	public GameObject dialogHumanPrefab;
	public Transform scrollViewContent;
	public Transform scrollRect;
	private void CreateMessageDialogBox(string fulfillmentText, DialogType dialogType)
	{
		GameObject dialog;
		if (dialogType == DialogType.Bot)
		{
			dialog = Instantiate(dialogBotPrefab);
		}
		else
		{
			dialog = Instantiate(dialogHumanPrefab);
		}

		dialog.GetComponentInChildren<TMP_Text>().text = fulfillmentText;
		dialog.transform.SetParent(scrollViewContent);
		ScrollToBottom(scrollRect);
	}

	public TMP_InputField EventText = null;
	//public TMP_Text ResultText = null;
	public static string dialogString = null;
	public static string replyString = null;

	public void InputFieldControl()
	{
		if (EventText.text.Length != 0)
		{
			dialogString = EventText.text;
			CreateMessageDialogBox(dialogString, DialogType.Human);
			ScrollToBottom(scrollRect);
			StartCoroutine(PostRequest());
			EventText.text = "";
		}
		else
		{
			EventText.ActivateInputField();
		}
	}

	public void ScrollToBottom(Transform scrollView)
	{
		scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
	}
}
