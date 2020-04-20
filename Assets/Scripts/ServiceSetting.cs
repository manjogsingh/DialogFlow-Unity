using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DF2", menuName = "DF2/Access Settings")]
public class ServiceSetting : ScriptableObject
{
	[SerializeField]
	private string projectId = "";
	/// <summary>
	/// The GCP project ID.
	/// </summary>
	public string ProjectId { get { return projectId; } }

	[SerializeField]
	private string credentialsFileName = "";
	/// <summary>
	/// The name of the .p12 file that contains the service account credentials.
	/// </summary>
	public string CredentialsFileName { get { return credentialsFileName; } }

	[SerializeField]
	private string serviceAccount = "";
	/// <summary>
	/// The service account address.
	/// </summary>
	public string ServiceAccount { get { return serviceAccount; } }

	[SerializeField]
	private string languageCode = "";
	/// <summary>
	/// The language code of requests and responses.
	/// </summary>
	public string LanguageCode { get { return languageCode; } set { languageCode = value; } }
}
