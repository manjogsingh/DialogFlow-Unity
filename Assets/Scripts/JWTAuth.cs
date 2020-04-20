using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

struct JWTToken
{
	public string token;
	public float expireTime;

	public JWTToken(string token, float expireTime)
	{
		this.token = token;
		this.expireTime = expireTime;
	}
}

public static class JWTAuth
{
	private static Dictionary<string, JWTToken> tokens;

	static JWTAuth()
	{
		tokens = new Dictionary<string, JWTToken>();
	}

	public static bool TryGetToken(string serviceAccount, out string token)
	{
		if (tokens.TryGetValue(serviceAccount, out JWTToken jwt) && Time.time < jwt.expireTime)
		{
			token = jwt.token;
			return true;
		}

		token = string.Empty;
		return false;
	}

	public static IEnumerator GetToken(string credentialsFileName, string serviceAccount)
	{
		string file = "Assets/Resources/Certificate/" + credentialsFileName;
		var jwt = GoogleJsonWebToken.GetJwt(serviceAccount, file,
			GoogleJsonWebToken.SCOPE_DIALOGFLOWV2);
		UnityWebRequest tokenRequest = GoogleJsonWebToken.GetAccessTokenRequest(jwt);
		yield return tokenRequest.SendWebRequest();
		if (tokenRequest.isNetworkError || tokenRequest.isHttpError)
		{
			Debug.LogError("Error " + tokenRequest.responseCode + ": " + tokenRequest.error);
			yield break;
		}
		string serializedToken = Encoding.UTF8.GetString(tokenRequest.downloadHandler.data);
		var jwtJson = JsonConvert.DeserializeObject<GoogleJsonWebToken.JwtTokenResponse>(serializedToken);
		tokens[serviceAccount] = new JWTToken(jwtJson.access_token,
			Time.time + jwtJson.expires_in);
	}
}
