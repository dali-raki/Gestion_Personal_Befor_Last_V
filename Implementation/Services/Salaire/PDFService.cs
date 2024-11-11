using PdfSharp.Drawing;
using PdfSharp.Pdf;
using GestionPersonnel.Models.Salaires;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace GestionPersonnel.Services
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> GenerateSalairePDFAsync(SalaireDetail salaireDetail)
        {
            using (var stream = new MemoryStream())
            {
                
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Fiche de Paie";

                
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

               
                XFont titleFont = new XFont("Verdana", 20, XFontStyleEx.Bold);
                XFont headerFont = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont regularFont = new XFont("Verdana", 10, XFontStyleEx.Regular);

               
                double margin = 40;
                double yPoint = margin;

                
                gfx.DrawString("Fiche de Paie", titleFont, XBrushes.Black,
                    new XRect(0, yPoint, page.Width, page.Height),
                    XStringFormats.TopCenter);
                yPoint += 40;

                
                gfx.DrawString($"Date de génération: {DateTime.Now:dd/MM/yyyy}", regularFont, XBrushes.Black,
                    new XRect(0, yPoint, page.Width - margin, page.Height),
                    XStringFormats.TopRight);
                yPoint += 30;

              
                gfx.DrawString($"Nom: {salaireDetail.NomEmploye}", headerFont, XBrushes.Black, margin, yPoint);
                yPoint += 20;
                gfx.DrawString($"Prénom: {salaireDetail.PrenomEmploye}", regularFont, XBrushes.Black, margin, yPoint);
                yPoint += 20;
                gfx.DrawString($"Fonction: {salaireDetail.NomFonction}", regularFont, XBrushes.Black, margin, yPoint);
                yPoint += 20;
                gfx.DrawString($"Type de Paiement: {salaireDetail.TypePaiement}", regularFont, XBrushes.Black, margin, yPoint);
                yPoint += 30;

              
                gfx.DrawLine(XPens.Black, margin, yPoint, page.Width - margin, yPoint);
                yPoint += 20;

                
                gfx.DrawString("Description", headerFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString("Montant", headerFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 20;

                gfx.DrawString("Salaire", regularFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString($"{salaireDetail.Salaire} DA", regularFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 20;

                gfx.DrawString("Primes", regularFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString($"{salaireDetail.Primes} DA", regularFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 20;

                gfx.DrawString("Avances", regularFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString($"{salaireDetail.Avances} DA", regularFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 20;

                gfx.DrawString("Dettes", regularFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString($"{salaireDetail.Dettes} DA", regularFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 20;

                gfx.DrawString("Salaire Net", headerFont, XBrushes.Black, margin, yPoint);
                gfx.DrawString($"{salaireDetail.SalaireNet} DA", headerFont, XBrushes.Black, page.Width / 2, yPoint);
                yPoint += 30;

               
                yPoint = page.Height - margin;
                gfx.DrawString("Ce document est destiné uniquement à l'usage du destinataire.", regularFont, XBrushes.Gray,
                    new XRect(0, yPoint, page.Width, page.Height),
                    XStringFormats.BottomCenter);

                
                document.Save(stream, false);

               
                return await Task.FromResult(stream.ToArray());
            }
        }
    }
}
