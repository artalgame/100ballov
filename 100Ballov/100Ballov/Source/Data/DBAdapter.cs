using System;
using Android.Database.Sqlite;
using Android.Content;
using Android.Util;

namespace MR.Android.Data
{
	public class DBAdapter
	{
		public const string DataBaseName = "mrdb.db";
		public const string TaskTable = "tasksTable";
		public const int DataBaseVersion = 1;

		public const string TaskTableKeyId = "id";

		public const string TaskTableTaskNameKeyName="name";
		public const int TaskTableTaskNameNameColumn=1;

		public const string TaskTableOverallAttemptsKeyName="overall";
		public const int TaskTableOverallAttemptsNameColumn=2;

		public const string TaskTableRightAttemptsKeyName="right";
		public const int TaskTableRightAttemptsNameColumn=3;

		public static readonly string DATABASE_CREATE = "create table " + TaskTable + " (" + TaskTableKeyId + " integer primary key autoincrement, " +
		                                       TaskTableTaskNameKeyName + " string not null, " +
		                                       TaskTableOverallAttemptsKeyName + " int not null, " + 
		                                       TaskTableRightAttemptsKeyName + " int not null);";

		private SQLiteDatabase db;
		private readonly Context context;
		private DBHelper dbHelper; 

		public DBAdapter (Context context)
		{
			this.context = context;
			dbHelper = new DBHelper (context, DataBaseName, DataBaseVersion);
		}

		public DBAdapter Open()
		{
			try
			{
				db = dbHelper.WritableDatabase;
			}
			catch (SQLiteException ex) {
				db = dbHelper.ReadableDatabase; 
			}
			return this;
		}

		public void Close()
		{
			db.Close ();
		}

		public string InsertEntry(TaskDBData taskInfo)
		{
			ContentValues newValues = GetContentValues (taskInfo);
			db.Insert (TaskTable, null, newValues);
			return taskInfo.Name;
		}

		public SQLiteCursor GetAllEntries()
		{
			return (SQLiteCursor)db.Query (TaskTable, new String[] {
				TaskTableKeyId,
				TaskTableTaskNameKeyName,
				TaskTableOverallAttemptsKeyName,
				TaskTableRightAttemptsKeyName
			}, null, null, null, null, null);
		}

		public TaskDBData GetEntry(string taskName)
		{
			String[] resultColumns = new string[] {
				TaskTableKeyId,
				TaskTableTaskNameKeyName,
				TaskTableOverallAttemptsKeyName,
				TaskTableRightAttemptsKeyName
			};

			string where = GetWhereForTaskName (taskName);
			SQLiteCursor cursor = (SQLiteCursor)db.Query (TaskTable, resultColumns, where, null, null, null,null);


			var taskDBData = new TaskDBData () {Name =
					cursor.GetString (TaskTableTaskNameNameColumn),
				OverallAttempts = cursor.GetInt (TaskTableOverallAttemptsNameColumn),
				RightAttempts = cursor.GetInt (TaskTableRightAttemptsNameColumn)
			};
			return taskDBData;
		}

		public bool UpdateEntry(TaskDBData taskData)
		{
			ContentValues updateValues = GetContentValues (taskData);
			String where = GetWhereForTaskName (taskData.Name);
			db.Update (TaskTable, updateValues, where, null);
			return true;
		}
		
		public bool DeleteEntry(TaskDBData taskData)
		{
			String where = GetWhereForTaskName (taskData.Name);
			db.Delete (TaskTable, where, null);
			return true;
		}

		private String GetWhereForTaskName(string taskName)
		{
			return TaskTableTaskNameKeyName + "=" + taskName;
		}

		private ContentValues GetContentValues(TaskDBData taskInfo)
		{
			ContentValues newValues = new ContentValues ();
			newValues.Put (TaskTableTaskNameKeyName, taskInfo.Name);
			newValues.Put (TaskTableOverallAttemptsKeyName, taskInfo.OverallAttempts);
			newValues.Put (TaskTableRightAttemptsKeyName, taskInfo.RightAttempts);
			return newValues;
		}

		class DBHelper:SQLiteOpenHelper
		{
			public DBHelper(Context context, String name, int version):base(context, name, null, version)
			{

			}

			public override void OnCreate(SQLiteDatabase db)
			{
				db.ExecSQL (DATABASE_CREATE);
			}

			public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
			{
				Log.Warn ("TaskDBAdapter", "Upgrating from version " + oldVersion + " to " + newVersion + ", which will destroy all old data");

				db.ExecSQL ("DROP TABLE IF EXIST " + TaskTable);
				OnCreate (db);
			}
		}
	}
}

