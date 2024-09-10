using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLoggerWebApp.Models;

namespace WaterLoggerWebApp.Pages;

public class EditModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;

    [BindProperty]
    public WaterRecordModel WaterRecord { get; set; }

    public IActionResult OnGet(int id)
    {
        WaterRecord = GetByID(id);

        return Page();
    }

    public IActionResult OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString"));
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE drinking_water SET Date = @Date, Quantity = @Quantity WHERE Id = @Id";
        command.Parameters.AddWithValue("@Date", WaterRecord.Date);
        command.Parameters.AddWithValue("@Quantity", WaterRecord.Quantity);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery();

        connection.Close();

        return RedirectToPage("/Index");
    }

    private WaterRecordModel? GetByID(int id)
    {
        if (!ModelState.IsValid)
        {
            return null;
        }

        using var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString"));
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM drinking_water WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        using var reader = command.ExecuteReader();
        WaterRecordModel? ret = null;
        if (reader.Read())
        {
            ret = new WaterRecordModel
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentCulture.DateTimeFormat),
                Quantity = reader.GetInt32(2)
            };
        }

        connection.Close();

        return ret;
    }

}
