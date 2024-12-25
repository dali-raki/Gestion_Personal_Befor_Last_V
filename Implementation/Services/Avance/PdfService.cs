using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionPersonnel.Models.Avances;
using System;

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

        // Set starting position and table width
        double xPosition = 20;
        double yPosition = 40;
        double tableWidth = page.Width - 40;
        double[] columnWidths = { 50, 120, 120, 100 }; // Define column widths
        double rowHeight = 16; // Increased row height for better readability

        // Draw the title
        gfx.DrawString($"Avances Report for {date:yyyy-MM-dd}", headerFont, XBrushes.Black, new XPoint(xPosition, yPosition));
        yPosition += 30;

        // Draw the table header with improved design (centered text and alternating colors)
        DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], "N", headerFont, XBrushes.White, XBrushes.DarkBlue, XBrushes.Black, true);
        DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], "Nom", headerFont, XBrushes.White, XBrushes.DarkBlue, XBrushes.Black, true);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], "Prénom", headerFont, XBrushes.White, XBrushes.DarkBlue, XBrushes.Black, true);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], "Valeur Avances", headerFont, XBrushes.White, XBrushes.DarkBlue, XBrushes.Black, true);

        yPosition += rowHeight;

        // Draw the table rows with alternating colors for readability
        bool isAlternate = false;
        foreach (var avance in avances)
        {
            XBrush backgroundBrush = isAlternate ? XBrushes.LightGray : XBrushes.White;
            isAlternate = !isAlternate;

            DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], avance.AvanceID.ToString(), font, XBrushes.Black, backgroundBrush, XBrushes.Black);
            DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], avance.NomEmployee, font, XBrushes.Black, backgroundBrush, XBrushes.Black);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], avance.PrenomEmployee, font, XBrushes.Black, backgroundBrush, XBrushes.Black);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], $"{avance.Montant:0.00} DA", font, XBrushes.Black, backgroundBrush, XBrushes.Black);

            yPosition += rowHeight;

            // Add a new page if needed
            if (yPosition > page.Height - 40)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPosition = 40;
            }
        }

        // Save the document to a byte array
        using (MemoryStream memoryStream = new MemoryStream())
        {
            document.Save(memoryStream, false);
            return memoryStream.ToArray();
        }
    }

    private void DrawTableCell(XGraphics gfx, double xPosition, double yPosition, double width, string text, XFont font, XBrush textBrush, XBrush backgroundBrush, XBrush borderBrush, bool isHeader = false, double padding = 5)
    {
        // Draw background for the cell
        gfx.DrawRectangle(backgroundBrush, xPosition, yPosition, width, 16);

        // Draw the border around the cell
        gfx.DrawRectangle(new XPen(XColors.Black, 0.8), xPosition, yPosition, width, 16);

        // Measure text size to center it horizontally
        XSize textSize = gfx.MeasureString(text, font);
        double textX = xPosition + (width - textSize.Width) / 2;
        double textY = yPosition + (28 - textSize.Height) -5; // Position text near the bottom

        // Adjust padding if the cell is a header
        if (isHeader)
        {
            textX = xPosition + padding; // Left align text for headers
        }

        // Draw the text inside the cell with padding
        gfx.DrawString(text, font, textBrush, new XPoint(textX, textY));
    }
}
