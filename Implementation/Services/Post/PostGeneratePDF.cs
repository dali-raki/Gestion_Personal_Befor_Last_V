using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Collections.Generic;
using GestionPersonnel.Storages.Storages.PostesStorages;
using Infrastructures.Domains.Models.EquipePost;

namespace GestionPersonnel.Services
{
    public class PostGeneratePDF : IPostGeneratePDF
    {
        private readonly PosteStorage _posteStorage;

        public PostGeneratePDF(PosteStorage posteStorage)
        {
            _posteStorage = posteStorage;
        }

        public async Task<byte[]> GeneratePDF(int equipeId, DateTime date)
        {
            var (employePosts, equipeSalaires) = await _posteStorage.SelectEquipeSalairesAndPostes(equipeId, date);

            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont headerFont = new XFont("Verdana", 14, XFontStyleEx.Bold);
                XFont textFont = new XFont("Verdana", 12, XFontStyleEx.Regular);
                XFont tableHeaderFont = new XFont("Verdana", 12, XFontStyleEx.Bold);

                // Title
                gfx.DrawString("Rapport des Salaires et Postes de l'Équipe", titleFont, XBrushes.Black, new XRect(0, 30, page.Width, 40), XStringFormats.TopCenter);

                // Team Details
                if (equipeSalaires != null)
                {
                    gfx.DrawString($"Nom de l'Équipe: {equipeSalaires.NomEquipe}", headerFont, XBrushes.Black, new XPoint(50, 100));
                    gfx.DrawString($"Total des Postes: {equipeSalaires.TotalePostes}", textFont, XBrushes.Black, new XPoint(50, 130));
                    gfx.DrawString($"Salaire Total: {equipeSalaires.SalaireTotale} DA", textFont, XBrushes.Black, new XPoint(50, 160));
                }

                // Table Header
                gfx.DrawString("Détails des Postes des Employés", headerFont, XBrushes.Black, new XPoint(50, 200));

                // Draw Table Header
                gfx.DrawString("Nom de l'Employé", tableHeaderFont, XBrushes.Black, new XPoint(50, 230));
                gfx.DrawString("Total des Postes", tableHeaderFont, XBrushes.Black, new XPoint(300, 230));

                // Draw Table Lines
                gfx.DrawLine(XPens.Black, new XPoint(50, 240), new XPoint(550, 240)); // Header Line
                gfx.DrawLine(XPens.Black, new XPoint(50, 240), new XPoint(50, 240 + (employePosts.Count * 30))); // Left Vertical Line
                gfx.DrawLine(XPens.Black, new XPoint(300, 240), new XPoint(300, 240 + (employePosts.Count * 30))); // Middle Vertical Line
                gfx.DrawLine(XPens.Black, new XPoint(550, 240), new XPoint(550, 240 + (employePosts.Count * 30))); // Right Vertical Line

                // Table Data
                int yPosition = 260;
                foreach (var employePost in employePosts)
                {
                    gfx.DrawString(employePost.EmployeNomPrenom, textFont, XBrushes.Black, new XPoint(50, yPosition));
                    gfx.DrawString(employePost.TotalPostsEmploye.ToString(), textFont, XBrushes.Black, new XPoint(300, yPosition));
                    gfx.DrawLine(XPens.Black, new XPoint(50, yPosition + 10), new XPoint(550, yPosition + 10)); // Horizontal Line
                    yPosition += 30;
                }

                // Save PDF to MemoryStream
                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }
    }
}