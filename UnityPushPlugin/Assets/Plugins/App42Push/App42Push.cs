

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using AssemblyCSharpfirstpass;


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
			
		}

		public static string getLastPushMessage ()
		{
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

		public static string getDeviceType ()
		{
				#if UNITY_ANDROID
		return "Android";
				#endif
				#if UNITY_IPHONE
		return "IOS";
				#endif

		}

		public static void setApp42PushListener (App42NativePushListener listener)
		{
				app42Listener = listener;
				setNativeCallback ();
		}
	
		public  void onPushNotificationsReceived (String message)
		{ 
				Debug.Log (message);
				if (app42Listener != null && message != null) {
						app42Listener.onMessage (message);

				}
		}

		public  void onErrorFromNative (string error)
		{
				Debug.Log (error);
				if (app42Listener != null && error != null) {
						app42Listener.onError (error);
		
				}
		}

		public  void onDeviceTokenFromNative (String deviceToken)
		{ 
				Debug.Log (deviceToken);
				if (app42Listener != null) {
						if (deviceToken != null && deviceToken.Length != 0) {
								app42Listener.onDeviceToken (deviceToken);
						}
				}
		}

#if UNITY_ANDROID
		public static void registerOnGCM(string projectNo) {
      if(Application.platform != RuntimePlatform.Android) return;
      AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42.push.plugin.App42UnityHelper")) { 
			jc.CallStatic("registerOnGCM", projectNo);
          }
        }
		public static string getLastAndroidMessage() {
			if(Application.platform != RuntimePlatform.Android) return null;
			AndroidJNIHelper.debug = false; 
		using (AndroidJavaClass jc = new AndroidJavaClass("com.shephertz.app42.push.plugin.App42UnityHelper")) { 
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

}
	