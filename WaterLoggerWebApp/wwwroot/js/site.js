// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function calculate() {
    var records = document.getElementById("records");
    var total = 0;
    for (var i = 1; i < records.rows.length; i++) {
        var row = records.rows[i];
        var cells = row.cells;
        var cell = cells[1];
        var value = parseInt(cell.innerHTML);
        total += value;
    }

    var result = document.getElementById("result");
    result.append(`Total: ${total}`)
}