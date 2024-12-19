using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConceptZeeWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private const string DeletePassword = "*******"; // Hard-coded password

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "ExportedData.xlsx");

            using (_context)
            {
                var Contacts = await _context.Contacts.ToListAsync();

                using (var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
{
                    var workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    var sheet = new Sheet
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Contacts"
                    };
                    sheets.Append(sheet);

                    var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                    // Write Header Row
                    var headerRow = new Row();
                    headerRow.Append(
                        new Cell { CellValue = new CellValue("Id"), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue("Name"), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue("Email"), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue("DOB"), DataType = CellValues.String },
                        new Cell { CellValue = new CellValue("Remarks"), DataType = CellValues.String }
                    );
                    sheetData.AppendChild(headerRow);

                    // Write the data rows
                    foreach (var contact in Contacts)
                    {
                        var row = new Row();
                        row.Append(
                            new Cell { CellValue = new CellValue(contact.Id), DataType = CellValues.Number },
                            new Cell { CellValue = new CellValue(contact.ContactName), DataType = CellValues.String },
                            new Cell { CellValue = new CellValue(contact.ContactEmail.ToLower()), DataType = CellValues.String },
                            new Cell { CellValue = new CellValue(contact.ContactDOB.ToOADate().ToString()), DataType = CellValues.Number, StyleIndex = 1 },
                            new Cell { CellValue = new CellValue(contact.ContactRemarks), DataType = CellValues.Date }
                        );
                        sheetData.AppendChild(row);
                    }
                    // Add styles to the workbook
                    var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                    stylesPart.Stylesheet = CreateStylesheet();
                    stylesPart.Stylesheet.Save();
                    workbookPart.Workbook.Save(); // Save the workbook
                }
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
        }

        private Stylesheet CreateStylesheet()
        {
            return new Stylesheet(new Fonts(
                new Font(  // Default font
                    new FontSize { Val = 11 },
                    new Color { Rgb = "000000" }
                    )
                ),
                new Fills(
                    new Fill(
                        new PatternFill { PatternType = PatternValues.None }
                        ),
                    new Fill(new PatternFill { PatternType = PatternValues.Gray125 }
                    )
                ),
                new Borders(
                    new Border(
                        new LeftBorder(), new RightBorder(), new TopBorder(), new BottomBorder(), new DiagonalBorder()
                        )
                ),
                new CellFormats(
                    new CellFormat(), // Default
                    new CellFormat { NumberFormatId = 14, ApplyNumberFormat = true } // Date format (NumberFormatId = 14)
                    )
                );
        }

        [HttpPost]
        public async Task<IActionResult> ClearContacts([FromBody] DeleteRequest request)
        {
            if (request.Password != DeletePassword)
            {
                return Unauthorized("Invalid password.");
            }
            _context.Contacts.RemoveRange(_context.Contacts);
            await _context.SaveChangesAsync();
            return Ok("All contacts have been deleted.");
        }
        public class DeleteRequest
        {
            public required string Password { get; set; }
        }
    }
}
