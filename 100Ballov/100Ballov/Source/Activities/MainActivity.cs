using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ballov.TestClasses;

namespace com.flaxtreme.CT.Activites
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

			var playStoreButton = FindViewById<ImageButton> (Resource.Id.marketButton);
			playStoreButton.Click += MarketButtonClick;
			var playStoreTextView = FindViewById<TextView> (Resource.Id.marketTextView);
			playStoreTextView.Click += MarketButtonClick;


			var vkButton = FindViewById<ImageButton> (Resource.Id.vkButton);
			vkButton.Click += VkButtonClick;
			var vkTextView = FindViewById<TextView> (Resource.Id.vkTextView);
			vkTextView.Click += VkButtonClick;

			var shareButton = FindViewById<ImageButton> (Resource.Id.shareButton);
			shareButton.Click += ShareButtonClick;
			var shareTextView = FindViewById<TextView> (Resource.Id.shareTextView);
			shareTextView.Click += ShareButtonClick;
		}

		private Intent GetPreTestIntent(TestTypeEnum testType)
		{
			var intent = new Intent (this, typeof(PreTestActivity));
			intent.PutExtra ("TestType", testType.ToString());
			return intent;
		}

		public override void OnBackPressed ()
		{
			Finish ();
		}

		public bool onPrepareOptionsMenu (Menu menu) {
			return false;
		}

		private void MarketButtonClick(Object sender, EventArgs args)
		{
			GoToURL ("https://play.google.com/store/apps/details?id=com.flaxtreme.CT");
			//GoToURL ("https://play.google.com/store/apps/details?id=com.flaxtreme.pahonia");	
		}

		private void VkButtonClick(Object sender, EventArgs args)
		{
			GoToURL ("http://vk.com/topic-50105858_29108685");	
		}

		private void ShareButtonClick(Object sender, EventArgs args)
		{
			Intent sharingIntent = new Intent(Intent.ActionSend);
			sharingIntent.SetType ("text/plain");
			String shareBody = "Тесты для подготовки к ЦТ на смартфонах и планшетах Андроид. Скачай тут: http://goo.gl/38xv4s";
			sharingIntent.PutExtra(Intent.ExtraText, shareBody);
			StartActivity(Intent.CreateChooser(sharingIntent, "Поделиться"));
		}

		private void GoToURL(string url)
		{
			Android.Net.Uri uri = Android.Net.Uri.Parse (url);
			if (uri != null) {
				Intent launchBrowser = new Intent (Intent.ActionView, uri);
				StartActivity (launchBrowser);
			} 
		}
	}
}


