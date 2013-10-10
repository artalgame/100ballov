using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ballov.TestClasses;

namespace Ballov.Activites
{
	[Activity (Label = "100Ballov", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			var mathButton = FindViewById<Button> (Resource.Id.MathButton);
			mathButton.Click += (object sender, EventArgs e) => {
				StartActivity(GetPreTestIntent(TestTypeEnum.Math));
			};

			var rusButton = FindViewById<Button> (Resource.Id.RusButton);
			rusButton.Click += (object sender, EventArgs e) => {
				StartActivity(GetPreTestIntent(TestTypeEnum.Russian));
			};

			var belButton = FindViewById<Button> (Resource.Id.BelButton);
			belButton.Click += (object sender, EventArgs e) => {
				StartActivity(GetPreTestIntent(TestTypeEnum.Belarussian));
			};
		}

		private Intent GetPreTestIntent(TestTypeEnum testType)
		{
			var intent = new Intent (this, typeof(PreTestActivity));
			intent.PutExtra ("TestType", testType.ToString());
			return intent;
		}
	}
}


