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
                XFont titleFont = new XFont("Arial", 18, XFontStyleEx.Bold);
                XFont labelFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XFont valueFont = new XFont("Arial", 12, XFontStyleEx.Bold);
                XFont tableHeaderFont = new XFont("Arial", 12, XFontStyleEx.Bold);
                XFont tableCellFont = new XFont("Arial", 12, XFontStyleEx.Regular);

                int leftMargin = 60;
                int rightMargin = 500;
                int y = 40;

                // Title
                gfx.DrawString("Rapport des Salaires et Postes de l'Équipe", titleFont, XBrushes.Black,
                    new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
                y += 40;

                gfx.DrawLine(XPens.LightGray, leftMargin, y, rightMargin, y);
                y += 30;

                // Équipe Infos
                if (equipeSalaires != null)
                {
                    gfx.DrawString("Nom de l'Équipe:", labelFont, XBrushes.Black, new XPoint(leftMargin, y));
                    gfx.DrawString(equipeSalaires.NomEquipe, valueFont, XBrushes.Black, new XPoint(160, y));
                    y += 25;

                    gfx.DrawString("Total des Postes:", labelFont, XBrushes.Black, new XPoint(leftMargin, y));
                    gfx.DrawString(equipeSalaires.TotalePostes.ToString(), valueFont, XBrushes.Black, new XPoint(160, y));
                    y += 25;

                    gfx.DrawString("Date du Rapport:", labelFont, XBrushes.Black, new XPoint(leftMargin, y));
                    gfx.DrawString(date.ToString("dd/MM/yyyy"), valueFont, XBrushes.Black, new XPoint(160, y));
                    y += 30;
                }

                // Table Title
                y += 20;
                gfx.DrawString("Détails des Postes des Employés", tableHeaderFont, XBrushes.Black, new XPoint(leftMargin, y));
                y += 25;

                // Table Headers
                gfx.DrawRectangle(XBrushes.LightGray, leftMargin, y, 440, 25);
                gfx.DrawString("Nom de l'Employé", tableHeaderFont, XBrushes.Black, new XPoint(leftMargin + 10, y + 18));
                gfx.DrawString("Total des Postes", tableHeaderFont, XBrushes.Black, new XPoint(leftMargin + 320, y + 18));
                y += 25;

                // Table Rows
                foreach (var employePost in employePosts)
                {
                    gfx.DrawRectangle(XBrushes.White, leftMargin, y, 440, 25);
                    gfx.DrawRectangle(XPens.LightGray, leftMargin, y, 440, 25); // Border
                    gfx.DrawString(employePost.EmployeNomPrenom, tableCellFont, XBrushes.Black, new XPoint(leftMargin + 10, y + 18));
                    gfx.DrawString(employePost.TotalPostsEmploye.ToString(), tableCellFont, XBrushes.Black, new XPoint(leftMargin + 320, y + 18));
                    y += 25;
                }

                // Return the PDF
                using (MemoryStream stream = new MemoryStream())
                {
                    document.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }
    }


}