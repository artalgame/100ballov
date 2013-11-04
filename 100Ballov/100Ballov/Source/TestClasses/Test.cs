using System;
using Android.Content.Res;
using System.Collections.Generic;
using System.IO;
using System.Json;
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

		public int GetATaskCount
		{
			get {
				return aTasks.Count;
			}
		}

		public BTask GetBTask(int num)
		{
			if ((num >= 0) && (num < bTasks.Count))
				return bTasks [num];
			else
				return null;
		}

		public int GetBTaskCount
		{
			get {
				return bTasks.Count;
			}
		}

		public int[][] GetARightAnswers()
		{
			int[][] rightAnswers = new int[aTasks.Count][];
			for (int i=0; i<aTasks.Count; i++) {
				rightAnswers[i] =aTasks [i].RightVariants.ToArray();
			}
			return rightAnswers; 
		}

		public string[] GetBRightAnswers()
		{
			string[] rightAnswers = new string[bTasks.Count];
			for (int i=0; i<bTasks.Count; i++) {
				rightAnswers[i] = bTasks [i].RightAnswer;
			}
			return rightAnswers;
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

