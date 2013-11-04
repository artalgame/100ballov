using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;

namespace com.flaxtreme.CT
{
	[Application]
	class MRApplication:Application
	{
		private static MRApplication singleton;

		public static MRApplication GetInstance(){

			return singleton;
		}

		public static AssetManager GetAssetManager()
		{
			return singleton.Assets;
		}

		public override void OnCreate(){
			base.OnCreate ();
			singleton = this;
		}

		public MRApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}
	}
}

