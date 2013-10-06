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

namespace Ballov
{
	[Activity (Label = "PreTestActivity")]			
	public class PreTestActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.PreTest);

			TestTypeEnum testType = TestTypeEnum.No;
			Enum.TryParse<TestTypeEnum> (Intent.GetStringExtra ("TestType"), out testType);
			if (testType != TestTypeEnum.No) 
			{
				var test = GetTest (testType);
				var startButton = FindViewById (Resource.Id.StartButton);
				startButton.Click += (object sender, EventArgs e) => 
				{

				};
			} 
			else
			{

			}
		}

		private Test GetTest(TestTypeEnum testType)
		{
			return new Test (testType, Assets);
		}
	}
}

