using Dapper;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class DBHelper
{
    private readonly string _dbPath;

    public DBHelper(string dbPath)
    {
        _dbPath = dbPath;

        if (!File.Exists(dbPath))
        {
            CreateDatabase();
        }
    }

    private SQLiteConnection GetConnection() => new SQLiteConnection($"Data Source={_dbPath};Version=3;");

    private void CreateDatabase()
    {
        using (SQLiteConnection con = GetConnection())
        {
            con.Open();

            var cmd = con.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Event
                (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NULL,
                    StartTime TEXT NULL,
                    EndTime TEXT NULL
                )
            ";
            cmd.ExecuteNonQuery();
        }
    }

    public int GetEventNumOfADay(DateTime dateTime)
    {
        using (SQLiteConnection con = GetConnection())
        {
            con.Open();
            var events = con.Query<Event>("SELECT * FROM Event WHERE date(StartTime) = date(@date)", new { date = dateTime });
            return events.Count();
        }
    }

    public int AddOneRecordToDB(string e, DateTime startTime, DateTime endTime)
    {
        using (var con = GetConnection())
        {
            con.Open();

            int result = con.Execute(
                @"
                INSERT INTO Event 
                (
                    Name, StartTime, EndTime
                ) 
                VALUES 
                (
                    @name, 
                    @start, 
                    @end
                )",
                new { name = e, start = startTime, end = endTime }
            );

            return result;
        }
    }
}
