

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using AssemblyCSharpfirstpass;
using System.Runtime.InteropServices;
#if UNITY_WP8
using UnityPluginForWindowsPhone;
#endif
public class App42Push : MonoBehaviour
{
	private static App42Push mInstance = null;
	private static App42NativePushListener app42Listener;
	public static void registerForPush (string projectNo)
	{
		#if UNITY_ANDROID
		registerOnGCM(projectNo);
		#endif
		#if UNITY_IPHONE
		registerOnApple();
		#endif
		#if UNITY_WP8
		registerOnWindows(projectNo);
		#endif
	}
	public static string getLastPushMessage(){
		string message = "";
		#if UNITY_ANDROID
		message=getLastAndroidMessage();
		#endif
		return message;
		}
	private static void setNativeCallback ()
	{
		if (mInstance == null) {
			GameObject receiverObject = new GameObject ("App42Push");
			DontDestroyOnLoad (receiverObject);
			mInstance = receiverObject.AddComponent<App42Push> ();
		}
	}
	public static string getDeviceType(){
		#if UNITY_ANDROID
		return "Android";
		#endif
		#if UNITY_IPHONE
		return "IOS";
		#endif
		#if UNITY_WINDOWS
		return "WP7";
		#endif
		return null;
	}
	public static void setApp42PushListener (App42NativePushListener listener)
	{
		  app42Listener = listener;
		  setNativeCallback ();
	}
	
	public static void onPushNotificationsReceived (String message)
	{ 
		Debug.Log (message);
		if (app42Listener != null) {
			app42Listener.onMessage(message);

		}
	}
public static void onErrorFromNative(string error)
	{
		Debug.Log(error);
		if (app42Listener != null) {
		if (error != null && error.Length!=0) 
		{
			app42Listener.onError(error);
		}
		}
	}
	public static void onDeviceTokenFromNative (String deviceToken)
	{ 
		Debug.Log (deviceToken);
		if (app42Listener != null) {
		if (deviceToken != null && deviceToken.Length!=0) 
		{
			app42Listener.onDeviceToken(deviceToken);
		}
		}
	}

#if UNITY_ANDROID
		public static void registerOnGCM(string projectNo) {
      if(Application.platform != RuntimePlatform.Android) return;
      AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42Push.App42UnityHelper")) { 
			jc.CallStatic("registerOnGCM", projectNo);
          }
        }
		public static string getLastAndroidMessage() {
			if(Application.platform != RuntimePlatform.Android) return null;
			AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42Push.App42UnityHelper")) { 
			return jc.CallStatic<string>("getLastMessage");
			}
	}
	#endif

	#if UNITY_IPHONE

	public static void registerOnApple() {
		[System.Runtime.InteropServices.DllImport("__Internal")]
	 registerForRemoteNotifications();
	}
	#endif

	#if UNITY_WP8
	public static void registerOnWindows(string userName) {
	    App42PushService pushService = new App42PushService();
		pushService.CreatePushChannel (Constants.UserId, PushChannelRegistrationCallback, PushChannelMessageCallback);
	}
	public static void PushChannelRegistrationCallback(object msg,bool IsError)
	{	
		if(!IsError)
		{
			onDeviceTokenFromNative((string)msg);
		}
		else{
		onErrorFromNative((string)msg);
		}
	}
	public static PushChannelMessageCallback(object sender,Dictionary<string,string> e)
	{	
		// Parse out the information that was part of the message.
		StringBuilder msg = new StringBuilder();
		foreach (string key in e.Keys)
		{
			msg.AppendFormat("{0}: {1}\n", key, e[key]);	
		    //if (string.Compare(
		    //   key,
		    //  "wp:Param",
		    //  System.Globalization.CultureInfo.InvariantCulture,
		    //  System.Globalization.CompareOptions.IgnoreCase) == 0)
		    //{
		    //    relativeUri = e.Collection[key];
		    //}
		}
		onPushNotificationsReceived(msg.ToString());
	}
	#endif 
}
	