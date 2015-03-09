App42 Unity PushNotification Widget on iOS and Android
================================================

This widget uses App42 Unity SDK internally, so first you have to add the same and initialize App42API in your project. 

Step-1: Follow the Steps from the Unity getting started page (http://api.shephertz.com/tutorial/Getting-Started-Unity/?index=gs-unitysdk)
		- Register/Login (https://apphq.shephertz.com/register) and create app
        
Step-2: Import this Unity Package in your Unity IDE.

Step 3: Go through with Android Prerequisites Settings "http://api.shephertz.com/tutorial/gcm-settings/?index=pn-cocos2dx-android_gcm_settings"

Step 4: Go through with iOS Prerequisites Settings "http://api.shephertz.com/tutorial/apns-settings/?index=pn-cocos2dx-ios_apns_settings".

##  For Running Sample
==============================================

Step-5: Add PushSample.cs file on MainCamera.

Step-6: Add APIKey, Secret Key you have received in Step 1 at line no 22 ,23.

Step-7: Change UserID variable which you want to register for Push Notification on APPHQ.

Step-8 : If you are building application for Android place your GoogleProjectNo at line no 24 received while Android Settings in Step 3.

Step-9 : Build and run application on iOS or Android platform.

Step-10 : If you are building on Android, change "com.test.push" sample package Name with you Bundle Idetifier in AndroidManifest.xml file of Android\plugin 
folder.

Step :11: For sample testing you can also send message to desired user by specifying userName and message in UI and click on send button.

Step :12 : Whenever a message is received or any callback receives from App42 side you are able to see it on Screen as message text. 
		
Note : *****For Android in AndroidManifest.xml packageName should be same as your application BundleIdentifier.****

Adding plug-in to an existing Unity project
==============================================

Step-1 : Import this Unity Package in your Unity IDE.

Step-2 : Android Configuration :
        
		 2.1 Replace fully qualified name "com.test.push" with your bundle identifier it AndroidManifest.xml in Android plug-in folder.
		
         2.2 You can also change Push Notification title by replacing "App42PushUnity" with your title at line no 64 in AndroidManifest.xml file. 
		 
		 2.3 Ensure your Android plug-in folder contains support and googlePlayService jar files from plug-in.
		 
		 2.3 If you are creating you own AndroidManifest.xml file add following things from plug-in AndroidManifest.xml file.
		 
		    2.3.1 Add following permissions in AndroidManifest.xml file, also change "com.test.push" with you android Bundle Identifier or package name.
			
			  /************************************************************************/
				<permission
				android:name="com.test.push.permission.C2D_MESSAGE"
				android:protectionLevel="signature" />
				<uses-permission android:name="com.test.push.permission.C2D_MESSAGE" />
				<!-- This app has permission to register and receive data message. -->
				<uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
				<uses-permission android:name="android.permission.WAKE_LOCK" />
				<!-- GCM requires a Google account. -->
				<uses-permission android:name="android.permission.GET_ACCOUNTS" />
				<uses-permission android:name="android.permission.INTERNET" />
				<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
				<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
				<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
				<uses-permission android:name="android.permission.HARDWARE_TEST" />
				<uses-permission android:name="android.permission.VIBRATE" />
				
			 /**************************************************************************/
			 
		    2.3.2 Add App42GCMService :
			  /**************************************************************************/
			 
	          <service android:name="com.shephertz.app42.push.plugin.App42GCMService" > 
			  </service>
			  
			   /**************************************************************************/
		    2.3.3 Add App42GCMReceiver :
			   /**************************************************************************/
			   
              	<receiver
					android:name="com.shephertz.app42.push.plugin.App42GCMReceiver"
					android:permission="com.google.android.c2dm.permission.SEND" >
					<intent-filter>
						<!-- Receives the actual messages. -->
						<action android:name="com.google.android.c2dm.intent.RECEIVE" />
						<!-- Receives the registration id. -->
						<action android:name="com.google.android.c2dm.intent.REGISTRATION" />
						<!-- For customize with your app change below line 51 "com.test.push".with your app package name-->
						<category android:name="com.test.push" />
					</intent-filter>
				</receiver>
		
		      /**************************************************************************/
				
		    2.3.4 Add Meta data Info :
		      /**************************************************************************/
		     
				<meta-data android:name="push_title" android:value="App42PushUnity"/>
				<meta-data android:name="com.google.android.gms.version" android:value="6587000" />
				<meta-data android:name="onMessageOpen"
					android:value="com.unity3d.player.UnityPlayerActivity" />
			  
			  /**************************************************************************/
		
Step-3 : Integration within your script file.
           
		   3.1 Add following code in Start() function of your script file :
		  
		       /**************************************************************************/
			   
				App42API.Initialize("App42 API Key","App42 Secret Key");
				//which you want to register for Push Notification
				App42API.SetLoggedInUser("App42 userId");
				//prior implement App42NativePushListener in you script file
				App42Push.setApp42PushListener (this);
				#if UNITY_ANDROID
				App42Push.registerForPush ("Your Google Project No");
				message=App42Push.getLastPushMessage();
				#endif 
				#if UNITY_IPHONE 
				App42Push.registerForPush ("");
				#endif 
				
			   /**************************************************************************/
		   3.2	 Add following callBack in script file as well :
		      
			  /**************************************************************************/
			    public void onDeviceToken(String deviceToken){
					Debug.Log ("Device token from native: "+deviceToken);
					String deviceType = App42Push.getDeviceType ();
					if(deviceType!=null&&deviceToken!=null&& deviceToken.Length!=0)
						App42API.BuildPushNotificationService().StoreDeviceToken(App42API.GetLoggedInUser(),deviceToken,
                                                    deviceType,new Callback());
				}
				
			    public void onMessage(String msg){
				 Debug.Log ("Message From native: "+msg);
				}
				public void onError(String error){
					Debug.Log ("Error From native: "+error);
				}
				
			  /**************************************************************************/
			  
Step-4: Build and run application on iOS or Android platform.	
 
For more info go through with "http://api.shephertz.com/app42-docs/push-notification-service/?sdk=unity"

Note : while building on iOS please ensure compatibility between xCode and Unity.

Contact us
——————————
In case of any issues, write us on http://forum.shephertz.com/