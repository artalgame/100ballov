using System;
using Android.Database.Sqlite;
using Android.Content;
using Android.Util;
using System.Linq;

namespace MR.Android.Data
{
	public class DBAdapter
	{
		public const string DataBaseName = "mrdb.db";
		public const string TaskTable = "taskTable";
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
		                                                TaskTableOverallAttemptsKeyName + " integer not null, " + 
		                                                TaskTableRightAttemptsKeyName + " integer not null);";

		private SQLiteDatabase db;
		private readonly Context context;
		private DBHelper dbHelper; 

		public DBAdapter (Context context)
		{
			this.context = context;
			dbHelper = new DBHelper (context, DataBaseName, DataBaseVersion);
			Open ();
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
			try{
				long t = db.InsertOrThrow (TaskTable, null, newValues);
			}
			catch {
				return null;
			}
			return taskInfo.Name;
		}

		public ISQLiteCursorDriver GetAllEntries()
		{
			return (ISQLiteCursorDriver) db.Query (TaskTable, new String[] {
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

			try {
				var cursor = db.Query (TaskTable, resultColumns, null, null, null, null, null);

				cursor.MoveToFirst ();
				do {
					var idNum = cursor.GetInt (cursor.GetColumnIndex (TaskTableKeyId));
					var nameNum = cursor.GetString (cursor.GetColumnIndex (TaskTableTaskNameKeyName));
					var overallNum = cursor.GetInt (cursor.GetColumnIndex (TaskTableOverallAttemptsKeyName));
					var rightNum = cursor.GetInt (cursor.GetColumnIndex (TaskTableRightAttemptsKeyName));
					if (nameNum == taskName) {
						var taskDBData = new TaskDBData () {
							ID = idNum,
							Name =	nameNum,
							OverallAttempts = overallNum,
							RightAttempts = rightNum
						};
						return taskDBData;
					}
				} while(cursor.MoveToNext ());
				return null;
			} catch (Exception ex) {
				return null;
			}
		}

		public bool UpdateEntry(TaskDBData taskData)
		{
			ContentValues updateValues = GetContentValues (taskData);
			String where = GetWhereForTaskName (taskData.ID);
			db.Update (TaskTable, updateValues, where, null);
			return true;
		}
		
		public bool DeleteEntry(TaskDBData taskData)
		{
			String where = GetWhereForTaskName (taskData.ID);
			db.Delete (TaskTable, where, null);
			return true;
		}

		public bool DeleteAllEntry()
		{
			db.Delete (TaskTable, null, null);
			return true;
		}

		private String GetWhereForTaskName(int taskID)
		{
			return TaskTableKeyId + " = " + taskID;
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
				Log.Debug ("db", db.ToString());
				Log.Debug ("path", db.Path);
			}

			public override void OnOpen(SQLiteDatabase db)
			{
				return;
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

