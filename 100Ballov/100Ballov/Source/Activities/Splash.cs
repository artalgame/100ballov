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
using System.Threading;

namespace com.flaxtreme.CT.Activites
{
	//[Activity (Theme="@style/Theme.Splash",Label = "Splash",MainLauncher = true, NoHistory = true)]			
	public class Splash : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
			Thread.Sleep(3000);
			StartActivity(typeof(MainActivity));
		}
	}
}

