using PdfSharp.Pdf;
using PdfSharp.Drawing;
using GestionPersonnel.Models.Avances;

public interface IPdfService
{
    Task<byte[]> GenerateAvancePdfAsync(List<Avance> avances, DateTime date);
}

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerateAvancePdfAsync(List<Avance> avances, DateTime date)
    {
        PdfDocument document = new PdfDocument();
        document.Info.Title = $"Rapport des Avances - {date:yyyy-MM-dd}";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Arial", 10);
        XFont headerFont = new XFont("Arial", 12, XFontStyleEx.Bold);

        double xPosition = 20;
        double yPosition = 40;
        double[] columnWidths = { 40, 120, 120, 120, 120};
        double rowHeight = 16;

        // Title
        gfx.DrawString($"Rapport des Avances - {date:yyyy-MM-dd}", headerFont, XBrushes.Black, new XPoint(xPosition, yPosition));
        yPosition += 20;

        // Summary line
        int totalCount = avances.Count;
        double totalAmount = (double)avances.Sum(a => a.Montant);
        gfx.DrawString($"Total: {totalCount} enregistrements | Montant total: {totalAmount:N0} DA", font, XBrushes.Black, new XPoint(xPosition, yPosition));
        yPosition += 20;

        // Headers
        DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], "N°", headerFont, XBrushes.Black, XBrushes.White, XBrushes.LightGray, true);
        DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], "Nom", headerFont, XBrushes.Black, XBrushes.White, XBrushes.LightGray, true);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], "Prénom", headerFont, XBrushes.Black, XBrushes.White, XBrushes.LightGray, true);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], "Valeur Avances", headerFont, XBrushes.Black, XBrushes.White, XBrushes.LightGray, true);
        DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3], yPosition, columnWidths[4], "Date", headerFont, XBrushes.Black, XBrushes.White, XBrushes.LightGray, true);

        yPosition += rowHeight;

        // Rows
        bool isAlternate = false;
        foreach (var avance in avances)
        {
            XBrush backgroundBrush = isAlternate ? new XSolidBrush(XColors.WhiteSmoke) : XBrushes.White;
            isAlternate = !isAlternate;

            DrawTableCell(gfx, xPosition, yPosition, columnWidths[0], avance.AvanceID.ToString(), font, XBrushes.Black, backgroundBrush, XBrushes.LightGray);
            DrawTableCell(gfx, xPosition + columnWidths[0], yPosition, columnWidths[1], avance.NomEmployee, font, XBrushes.Black, backgroundBrush, XBrushes.LightGray);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1], yPosition, columnWidths[2], avance.PrenomEmployee, font, XBrushes.Black, backgroundBrush, XBrushes.LightGray);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2], yPosition, columnWidths[3], $"{avance.Montant:0.00} DA", font, XBrushes.Black, backgroundBrush, XBrushes.LightGray);
            DrawTableCell(gfx, xPosition + columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3], yPosition, columnWidths[4], avance.Date.ToString("dd/MM/yyyy"), font, XBrushes.Black, backgroundBrush, XBrushes.LightGray);

            yPosition += rowHeight;

            // Page break
            if (yPosition > page.Height - 40)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPosition = 40;
            }
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            document.Save(memoryStream, false);
            return memoryStream.ToArray();
        }
    }

    private void DrawTableCell(XGraphics gfx, double xPosition, double yPosition, double width, string text, XFont font, XBrush textBrush, XBrush backgroundBrush, XBrush borderBrush, bool isHeader = false, double padding = 5)
    {
        double height = 16;

        // Background
        gfx.DrawRectangle(backgroundBrush, xPosition, yPosition, width, height);

        // Border
        gfx.DrawRectangle(new XPen(((XSolidBrush)borderBrush).Color, 0.6), xPosition, yPosition, width, height);

        // Text alignment
        double textX = xPosition + padding;

        if (!isHeader && text.Contains("DA"))
        {
            XSize textSize = gfx.MeasureString(text, font);
            textX = xPosition + width - textSize.Width - padding;
        }

        double textY = yPosition + height - 5;

        gfx.DrawString(text, font, textBrush, new XPoint(textX, textY));
    }
}
