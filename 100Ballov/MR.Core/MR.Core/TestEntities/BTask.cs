using System;

namespace MR.Core.TestEntities
{
	public class BTask:Task
	{
		public string Variant{ get; set; }
		public BTask(int taskNum, SubjectTheme theme, SubjectsEnumeration subject):base(taskNum, theme, subject)
		{
		}
	}
}

