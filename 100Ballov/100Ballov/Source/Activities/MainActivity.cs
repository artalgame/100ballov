using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Ballov.TestClasses;
using MR.Core.TestEntities;

namespace com.flaxtreme.CT.Activites
{
	[Activity (Label = "MobileRepetitor", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var mathButton = FindViewById<Button> (Resource.Id.MathButton);
			mathButton.Click += (object sender, EventArgs e) => {
				StartActivity(GetSubjectIntent(SubjectsEnumeration.Math));
			};

			(FindViewById<ImageButton> (Resource.Id.marketButton)).Click += MarketButtonClick;
			(FindViewById<TextView> (Resource.Id.marketTextView)).Click += MarketButtonClick;

			(FindViewById<ImageButton> (Resource.Id.vkButton)).Click += VkButtonClick;
			(FindViewById<TextView> (Resource.Id.vkTextView)).Click += VkButtonClick;

			(FindViewById<ImageButton> (Resource.Id.shareButton)).Click += ShareButtonClick;
			(FindViewById<TextView> (Resource.Id.shareTextView)).Click += ShareButtonClick;
		}

		private Intent GetSubjectIntent(SubjectsEnumeration subjectType)
		{
			var intent = new Intent (this, typeof(SubjectActivity));
			intent.PutExtra ("SubjectType", subjectType.ToString());
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


