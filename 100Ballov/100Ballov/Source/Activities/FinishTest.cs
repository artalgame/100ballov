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

namespace com.flaxtreme.CT.Activites
{
	[Activity (Label = "FinishTest")]			
	public class FinishTest : Activity
	{

		List<List<int>> aRightAnswers;
		List<int> aAnswers;
		List<string> bRightAnswers;
		List<string> bAnswers;

		int rating = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.FinishTest);

			ReadExtras ();
			rating = CalcRating ();
			var resultTextView =(TextView) FindViewById (Resource.Id.testResultTextView);
			resultTextView.Text ="Ваш результат " + rating + " баллов";

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

		private int CalcRating(){
			//all tasks = 100%, aPart = 50% and bPart = 50%, aTask = 50%/aCount; bTask = 50%/bCount;
			float aPart = 50f / aAnswers.Count;
			float bPart = 50f / bAnswers.Count;

			float sum = 0f;
			int i = 0;
			foreach(var ans in aAnswers)
			{
				if(aRightAnswers[i].Contains(ans+1))
				{
					sum += aPart;
				}
				i++;
			}

			for(i=0; i<bAnswers.Count; i++)
			{
				if(bAnswers[i] == bRightAnswers[i])
				{
					sum+= bPart;
				}
			}
			return (int)Math.Round(sum);
		}

		private void ReadExtras()
		{
			aRightAnswers = new List<List<int>> ();
			aAnswers = Intent.GetIntArrayExtra ("aAnswers").ToList();
					//finishTestIntent.PutExtra ("bAnswers", bAnswers);
			bAnswers = Intent.GetStringArrayExtra ("bAnswers").ToList();
			//finishTestIntent.PutExtra ("aRightAnswers", test.GetARightAnswers);
			var aCurRightAnswers = Intent.GetIntArrayExtra ("aRightAnswers0");
			int i=1;
			while (aCurRightAnswers != null) {
				aRightAnswers.Add (aCurRightAnswers.ToList ());
				aCurRightAnswers = Intent.GetIntArrayExtra ("aRightAnswers" + i);
				i++;
			}
			//finishTestIntent.PutExtra ("bRightAnswers", test.GetBRightAnswers);
			bRightAnswers = Intent.GetStringArrayExtra ("bRightAnswers").ToList ();
		}
		public override void OnBackPressed()
		{
			Intent intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.PutExtra("EXIT", true);
			StartActivity(intent);
		}
		public bool onPrepareOptionsMenu (Menu menu) {
				return false;
		}
	}
}

