using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLoggerWebApp.Models;

namespace WaterLoggerWebApp.Pages;

public class CreateModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;

    [BindProperty]
    public WaterRecord WaterRecord { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString"));
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO drinking_water (Date, Quantity) VALUES (@Date, @Quantity)";
        command.Parameters.AddWithValue("@Date", WaterRecord.Date);
        command.Parameters.AddWithValue("@Quantity", WaterRecord.Quantity);
        command.ExecuteNonQuery();

        connection.Close();

        return RedirectToPage("/Index");
    }
}