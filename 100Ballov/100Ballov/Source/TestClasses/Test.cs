using System;
using System.IO;
using Android.Content.Res;
using System.Json;
using System.Collections.Generic;

namespace Ballov.TestClasses
{
	public class Test
	{
		AssetManager assets = null;
		List<ATask> aTasks = new List<ATask> ();
		List<BTask> bTasks = new List<BTask> ();
		public Test (TestTypeEnum testType, AssetManager assets)
		{
			this.assets = assets;
			string testFileName = null;

			switch( testType)
			{
			case TestTypeEnum.Math:
				testFileName = "Math.txt";
				break;
			case TestTypeEnum.Belarussian:
				testFileName = "Belarussian.txt";
				break;
			case TestTypeEnum.Russian:
				testFileName = "Russian.txt";
				break;
			}
			if(testFileName != null)
			{
				SetTest(testFileName);
			}
		}

		public ATask GetATask(int num)
		{
			if ((num >= 0) && (num < aTasks.Count))
				return aTasks [num];
				else
					return null;
		}

		public BTask GetBTask(int num)
		{
			if ((num >= 0) && (num < bTasks.Count))
				return bTasks [num];
			else
				return null;
		}
		private void SetTest(string testFileName)
		{

			using (StreamReader reader = new  StreamReader(assets.Open(testFileName))) {
				string content = reader.ReadToEnd ();

				var json = JsonObject.Parse (content);

				JsonArray aTasksJson = json ["A_Tasks"] as JsonArray;
				foreach (JsonObject task in aTasksJson) 
				{
					var variants = new List<String> ();
					foreach (JsonValue variant in task["Answers"] as JsonArray) 
					{
						variants.Add (variant);
					}
					var rightAnswers = new List<int> ();
					foreach(JsonValue ra in task["RightAnswer"] as JsonArray)
					{
						rightAnswers.Add(int.Parse(ra));
					}
					var aTask = new ATask () 
					{
						Desc = task["Desc"],
						ImageLink = task["Image"],
						Variants = variants,
						RightVariants = rightAnswers
					};
					aTasks.Add (aTask);
				}
				JsonArray bTasksJson = json ["B_Tasks"] as JsonArray;
				foreach (JsonObject task in bTasksJson) 
				{
					var bTask = new BTask () {
						Desc = task["Desc"],
						ImageLink = task["Image"],
						RightAnswer = task["RightAnswer"]
					};
					bTasks.Add (bTask);
				}
			}
		}
	}
}

