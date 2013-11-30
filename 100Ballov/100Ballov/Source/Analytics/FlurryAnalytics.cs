//public class FlurryAnalytics : IAnalytics, IAndroidActivityTracker
//{
//	public const string ApiKeyValue = "QX3XHKK4GD5K3JGB88YT";
//
//	private readonly IntPtr _flurryClass;
//	private readonly IntPtr _flurryOnStartSession;
//	private readonly IntPtr _flurryOnEndSession;
//	private readonly IntPtr _flurryLogEvent;
//
//	public FlurryAnalytics()
//	{
//		_flurryClass = JNIEnv.FindClass("com/flurry/android/FlurryAgent");
//		_flurryOnStartSession = JNIEnv.GetStaticMethodID(_flurryClass, "onStartSession",
//			"(Landroid/content/Context;Ljava/lang/String;)V");
//		_flurryOnEndSession = JNIEnv.GetStaticMethodID(_flurryClass, "onEndSession", "(Landroid/content/Context;)V");
//		_flurryLogEvent = JNIEnv.GetStaticMethodID(_flurryClass, "logEvent", "(Ljava/lang/String;)V");
//	}
//
//	public void StartSession()
//	{
//		// not used in Android - Android relies on Activity tracking instead
//	}
//
//	public void LogEvent(string eventName)
//	{
//		ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryLogEvent, new JValue(new Java.Lang.String(eventName))));
//	}
//
//	private static void ExceptionSafe(Action action)
//	{
//		try
//		{
//			action();
//		}
//		catch (ThreadAbortException)
//		{
//			throw;
//		}
//		catch (Exception exception)
//		{
//			UITrace.Trace("Exception seen in calling Flurry through JNI " + exception.ToLongString());
//		}
//	}
//
//	public void OnStartActivity(Activity activity)
//	{
//		ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryOnStartSession, new JValue(activity), new JValue(new Java.Lang.String(ApiKeyValue))));
//	}
//
//	public void OnStopActivity(Activity activity)
//	{
//		ExceptionSafe(() => JNIEnv.CallStaticVoidMethod(_flurryClass, _flurryOnEndSession, new JValue(activity)));
//	}
//}