using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLoggerWebApp.Models;

namespace WaterLoggerWebApp.Pages;

public class IndexModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;

    public List<WaterRecordModel> WaterRecords { get; set; }

    public void OnGet()
    {
        WaterRecords = GetAllRecords();
        ViewData["Total"] = $"Total: {WaterRecords.Sum(x => x.Quantity)}";
    }

    private List<WaterRecordModel> GetAllRecords()
    {

        if (!ModelState.IsValid)
        {
            return [];
        }

        var records = new List<WaterRecordModel>();
        using var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString"));
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM drinking_water";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            records.Add(new WaterRecordModel
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentCulture.DateTimeFormat),
                Quantity = reader.GetInt32(2)
            });
        }

        connection.Close();

        return records;
    }
}
