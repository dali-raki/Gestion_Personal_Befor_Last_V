using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionPersonnel.Models.Avances;

public interface IPdfService
{
    Task<byte[]> GenerateAvancePdfAsync(List<Avance> avances, DateTime date);
}

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerateAvancePdfAsync(List<Avance> avances, DateTime date)
    {
        // Create a new PDF document
        PdfDocument document = new PdfDocument();
        document.Info.Title = $"Avances Report - {date:yyyy-MM-dd}";

        // Create a page
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Arial", 10);
        XFont headerFont = new XFont("Arial", 12, XFontStyleEx.Bold);
        XPen borderPen = new XPen(XColors.Black, 0.5); // Pen for table borders

        // Set the starting position for the table
        double xPosition = 20;
        double yPosition = 40;
        double tableWidth = page.Width - 40;
        double[] columnWidths = { 50, 120, 120, 100 }; // Define column widths
        double rowHeight = 20; // Row height
        double extraSpacing = 5; // Extra space between rows

        // Draw the title
        gfx.DrawString($"Avances Report for {date:yyyy-MM-dd}", headerFont, XBrushes.Black, new XPoint(xPosition, yPosition));
        yPosition += 30;

        // Draw the table header
        DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], "N", headerFont, XBrushes.White, XBrushes.DarkBlue);
        DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], "Nom", headerFont, XBrushes.White, XBrushes.DarkBlue);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], "Prénom", headerFont, XBrushes.White, XBrushes.DarkBlue);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], "Valeur Avances", headerFont, XBrushes.White, XBrushes.DarkBlue);

        yPosition += rowHeight + extraSpacing; // Move down to next row with extra space

        // Draw the table rows
        foreach (var avance in avances)
        {
            DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], avance.AvanceID.ToString(), font, XBrushes.Black, XBrushes.White);
            DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], avance.NomEmployee, font, XBrushes.Black, XBrushes.White);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], avance.PrenomEmployee, font, XBrushes.Black, XBrushes.White);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], $"{avance.Montant:0.00} DA", font, XBrushes.Black, XBrushes.White);

            yPosition += rowHeight + extraSpacing; // Add extra space between rows

            // Add a new page if the content goes beyond the page
            if (yPosition > page.Height - 40)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPosition = 40; // Reset yPosition for new page
            }
        }

        // Save the document to a byte array
        using (MemoryStream memoryStream = new MemoryStream())
        {
            document.Save(memoryStream, false);
            return memoryStream.ToArray();
        }
    }

    private void DrawTableCell(XGraphics gfx, double xPosition, double yPosition, double width, string text, XFont font, XBrush textBrush, XBrush backgroundBrush)
    {
        // Draw background for the cell
        gfx.DrawRectangle(backgroundBrush, xPosition, yPosition, width, 20);

        // Draw the border around the cell
        gfx.DrawRectangle(new XPen(XColors.Black, 0.5), xPosition, yPosition, width, 20);

        // Draw the text inside the cell
        gfx.DrawString(text, font, textBrush, new XPoint(xPosition + 5, yPosition + 4));
    }
}
