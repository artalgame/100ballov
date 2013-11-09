using System;

namespace MR.Core.TestEntities
{
	public class Task
	{
		public String QuestionText {get;set;}
		public String QuestionImageLink{ get; set; }

		public String SolutionTxt{ get; set; }
		public String SolutionImageLink{ get; set; }
		public String SolutionInetLink{ get; set; }
		public String SolutionLocalLink{ get; set; }

		public int TaskNum{ get; set; }

		public Task(int taskNum)
		{
			TaskNum = taskNum;
		}
		public Task()
		{
		}
	}
}

