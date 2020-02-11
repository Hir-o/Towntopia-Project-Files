// =======================================
// Poki Publishing Wrapper for Unity WebGL
// Beta version - Copyright Poki 2016
// =======================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class PokiException : System.Exception {
	public PokiException(string message) : base(message){}
}

public class PokiUnitySDK : MonoBehaviour {
	// Singleton magic
	private static PokiUnitySDK _instance;

	public static PokiUnitySDK Instance {
		get {
			if (_instance == null) {
				_instance = (PokiUnitySDK) FindObjectOfType(typeof(PokiUnitySDK));

				if (FindObjectsOfType(typeof(PokiUnitySDK)).Length > 1) {
					Debug.LogError("[Singleton] Something went really wrong " +
						" - there should never be more than 1 singleton!" +
						" Reopening the scene might fix it.");
					return _instance;
				}

				if (_instance == null) {
					GameObject singleton = new GameObject();
					_instance = singleton.AddComponent<PokiUnitySDK>();
					singleton.name = "(singleton) "+ typeof(PokiUnitySDK).ToString();

					DontDestroyOnLoad(singleton);

					Debug.Log("[Singleton] An instance of " + typeof(PokiUnitySDK) +
						" is needed in the scene, so '" + singleton +
						"' was created with DontDestroyOnLoad.");
				} else {
					Debug.Log("[Singleton] Using instance already created: " +
						_instance.gameObject.name);
				}
			}

			return _instance;
		}
	}

	public PokiUnitySDK () {}

	private bool initialized = false;
	private bool adblocked = false;
	public delegate void CommercialBreakDelegate();
 	public CommercialBreakDelegate commercialBreakCallBack;
	public delegate void RewardedBreakDelegate(bool withReward);
 	public RewardedBreakDelegate rewardedBreakCallBack;

	public void init(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: Initializing");
		return;
		#endif
		if (initialized) {
			throw new PokiException ("PokiUnitySDK is already initialized");
		}
		checkInit();
		Application.ExternalEval ("window.initPokiBridge('" + PokiUnitySDK.Instance.name + "');");
	}
	public bool isInitialized(){
		return initialized;
	}
	public bool adsBlocked(){
		return adblocked;
	}

	public void gameLoadingStart(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameLoadingStart");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.gameLoadingStart();");
	}

	public void gameLoadingFinished (){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameLoadingFinished");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.gameLoadingFinished();");
	}
	public void roundStart(string indentifier){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: roundStart");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.roundStart('"+indentifier+"');");
	}
	public void roundEnd (string indentifier){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: roundEnd");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.roundEnd('"+indentifier+"');");
	}
	public void customEvent (string eventNoun, string eventVerb, ScriptableObject eventData){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: roundEnd");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.customEvent('"+eventNoun+"','"+eventVerb+"',"+JsonUtility.ToJson(eventData ,true)+");");
	}
	public void setPlayerAge(int age){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: set player age"+age.ToString());
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.setPlayerAge("+age.ToString()+");");
	}
	public void togglePlayerAdvertisingConsent(bool consent){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: set advertising consent"+consent.ToString());
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.togglePlayerAdvertisingConsent("+consent.ToString().ToLower()+");");
	}

	public class LoadingProgressData : ScriptableObject {
		public float percentageDone;
		public int kbLoaded;
		public int kbTotal;
		public String fileNameLoaded;
		public int filesLoaded;
		public int filesTotal;
	}
	public void gameLoadingProgress(LoadingProgressData data){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameLoadingProgress");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.gameLoadingProgress("+JsonUtility.ToJson(data ,true)+");");
	}

	public void gameplayStart() {
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameplayStart");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.gameplayStart();");
	}

	public void gameplayStop() {
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameplayStop");
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.gameplayStop();");
	}
	public void commercialBreak() {
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: commercialBreak");
		commercialBreakCompleted();
		return;
		#endif
		if(adblocked){
			commercialBreakCompleted();
			return;
		}
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("window.commercialBreak();");

	}

	public void rewardedBreak(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: rewardedBreak");
		rewardedBreakCompleted("true");
		return;
		#endif
		if(adblocked){
			rewardedBreakCompleted("false");
			return;
		}
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("window.rewardedBreak();");
	}

	public void happyTime(float intensity){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: happyTime with intensity"+intensity.ToString());
		return;
		#endif
		if (!initialized) {
			throw new PokiException ("PokiUnitySDK is not yet initialized, make sure you call PokiUnitySDK.Instance.Init() first");
		}
		Application.ExternalEval ("PokiSDK.happyTime("+intensity.ToString()+");");
	}

	public void checkInit(){
		if(Application.isEditor) return; //czMtZXUtd2VzdC0xLmFtYXpvbmF3cy5jb20va2lsb28v
		string[] hosts = {"cWEtZmlsZXMucG9raS5jb20=", "Z2FtZS1jZG4ucG9raS5jb20=", "bG9jYWxob3N0Og=="};
		bool allowed = false;
		for(int i = 0; i<hosts.Length; i++){
			byte[] decodedBytes = Convert.FromBase64String (hosts[i]);
			string host = Encoding.UTF8.GetString (decodedBytes);
			string liveHost = Application.absoluteURL;
			string[] splitString = liveHost.Split(new string[] {"//"}, StringSplitOptions.None);
			liveHost = splitString[1];


			liveHost.Replace("www.", "");
			if(liveHost.Length > host.Length) liveHost = liveHost.Substring(0, host.Length);
			if (host == liveHost){
				allowed = true;
				break;
			}
		}
		if(!allowed){
			string targetURL = "aHR0cDovL3BvLmtpL3NpdGVsb2NrcmVkaXJlY3Q=";
			byte[] decodedBytes = Convert.FromBase64String (targetURL);
			string url = Encoding.UTF8.GetString (decodedBytes);
			Application.OpenURL(url);
		}
	}

	// ============================================================================
	// Below methods are called from the Javavascript SDK and should not be touched
	// ============================================================================

	public void ready(){
		initialized = true;
	}
	public void adblock(){
		initialized = true;
		adblocked = true;
	}
	public void commercialBreakCompleted(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: commercialBreak completed");
		#endif
		commercialBreakCallBack();
	}
	public void rewardedBreakCompleted(string withReward){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: rewardedBreak completed, received reward:"+withReward);
		#endif
		rewardedBreakCallBack((withReward == "true"));
	}
}