using System;
using System.Collections.Generic;
using MR.Core.TestEntities;

namespace MR.Core.Interfaces
{
	public interface ISubjectRetriver
	{
		List<Task> GetTasks (string themeNum);
	}
}

