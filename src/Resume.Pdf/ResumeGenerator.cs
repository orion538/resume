using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Resume.Data.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Resume.Pdf
{
    public interface IResumeGenerator
    {
        void Create(Data.Json.Resume resume);
    }

    public class ResumeGenerator : IResumeGenerator
    {
        private const string FileName = "Resume.pdf";
        private const float Spacing = 10f;

        public void Create(Data.Json.Resume resume)
        {
            var (pdf, document) = CreateDocument();
            document.SetMargins(0, 0, 0, 0);
            document.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(9);

            var pageTable = new Table(2);
            pageTable.SetWidth(UnitValue.CreatePercentValue(100));

            var sideBarCell = new Cell().SetBackgroundColor(new DeviceRgb(242, 242, 242)).SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(30));
            var contentCell = new Cell().SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(70));

            sideBarCell.Add(CreateProfilePicture(pdf));
            sideBarCell.Add(CreateBasics(resume.Basics));
            sideBarCell.Add(CreateLanguages(resume.Languages));
            sideBarCell.Add(CreateInterests(resume.Interests));

            contentCell.Add(CreateSummary(resume.Basics.Summary));
            contentCell.Add(CreateWork(resume.Work));
            contentCell.Add(CreateEducation(resume.Education));
            contentCell.Add(CreateSkills(resume.Skills));

            pageTable.AddCell(sideBarCell);
            pageTable.AddCell(contentCell);
            pageTable.AddCell(new Cell(1, 2).Add(CreateMessage()));

            document.Add(pageTable);

            document.Close();
        }

        private static (PdfDocument, Document) CreateDocument()
        {
            var writer = new PdfWriter(FileName);
            var pdf = new PdfDocument(writer);
            return (pdf, new Document(pdf));
        }

        private Table CreateBasics(Basics basics)
        {
            var table = new Table(1);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell(1, 1).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Personalia")).SetFontSize(11));

            table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(Spacing));

            table.AddCell(CreateDefaultCell(nameof(basics.Name), true));
            table.AddCell(CreateDefaultCell(basics.Name));

            table.AddCell(CreateDefaultCell(nameof(basics.Location.Address), true));
            table.AddCell(CreateDefaultCell(basics.Location.Address));
            table.AddCell(CreateDefaultCell($"{basics.Location.PostalCode}, {basics.Location.City}"));
            table.AddCell(CreateDefaultCell(basics.Location.CountryCode));

            table.AddCell(CreateDefaultCell(nameof(basics.Email), true));
            table.AddCell(CreateDefaultCell(basics.Email));

            table.AddCell(CreateDefaultCell(nameof(basics.Phone), true));
            table.AddCell(CreateDefaultCell(basics.Phone));

            return table;
        }

        private static Table CreateWork(IEnumerable<Work> previousWork)
        {
            var table = new Table(1);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Werkervaring")).SetFontSize(11));

            foreach (var work in previousWork)
            {
                table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(Spacing));
                table.AddCell(CreateDefaultCell($"{DateTime.Parse(work.StartDate):yyyy} - {DateTime.Parse(work.EndDate):yyyy}"));
                table.AddCell(CreateDefaultCell(work.Position, true));
                table.AddCell(CreateDefaultCell(work.Company));
                table.AddCell(CreateDefaultCell(work.Summary));
            }

            return table;
        }

        private static Table CreateEducation(IEnumerable<Education> completeEducation)
        {
            var table = new Table(1);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(Spacing);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Education")).SetFontSize(11));

            foreach (var education in completeEducation)
            {
                table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(Spacing));

                table.AddCell(CreateDefaultCell($"{DateTime.Parse(education.StartDate):yyyy} - {DateTime.Parse(education.EndDate):yyyy}"));
                table.AddCell(CreateDefaultCell(education.Institution, true));
                table.AddCell(CreateDefaultCell(education.StudyType));
                table.AddCell(CreateDefaultCell(education.Area));
            }

            return table;
        }

        private static Table CreateLanguages(IEnumerable<Language> languages)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(Spacing);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Languages")).SetFontSize(11));
            table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(Spacing));

            foreach (var language in languages)
            {
                table.AddCell(CreateDefaultCell(language.Name));
                table.AddCell(CreateDefaultCell(language.Level));
            }

            return table;
        }

        private static Table CreateInterests(IEnumerable<Interest> interests)
        {
            var table = new Table(1);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(Spacing);

            table.AddHeaderCell(new Cell(1, 1).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Interests")).SetFontSize(11));
            table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(Spacing));

            var list = new List().SetListSymbol(new Image(ImageDataFactory.CreatePng(File.ReadAllBytes("images\\square-png-30.png"))).ScaleToFit(5, 5).SetMargins(1, 1, 1, 1));

            foreach (var interest in interests)
            {
                list.Add(new ListItem(interest.Name)).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
            }

            table.AddCell(new Cell(1, 1).Add(list).SetBorder(Border.NO_BORDER));

            return table;
        }

        private static Table CreateSkills(IEnumerable<Skill> skills)
        {
            var table = new Table(3);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(Spacing);

            table.AddHeaderCell(new Cell(1, 3).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Skills")).SetFontSize(11));

            foreach (var skill in skills)
            {
                table.AddCell(CreateDefaultCell(skill.Name));
            }

            return table;
        }

        private static Paragraph CreateSummary(string summary)
        {
            var paragraph = new Paragraph();
            paragraph.SetWidth(UnitValue.CreatePercentValue(100)).SetPaddings(Spacing, 2, Spacing, 2);

            paragraph.Add(new Text(summary));

            return paragraph;
        }

        private static Cell CreateProfilePicture(PdfDocument pdfDocument)
        {
            var pdfFormXObject = new PdfFormXObject(new Rectangle(100, 100));
            var pdfCanvas = new PdfCanvas(pdfFormXObject, pdfDocument);
            pdfCanvas.SaveState();

            // Setting color to the circle 
            var color = new DeviceRgb(255, 255, 255);
            pdfCanvas.SetColor(color, true);

            // creating a circle 
            pdfCanvas.Circle(50, 50, 50);
            pdfCanvas.Fill();

            var rect = new Rectangle(pdfFormXObject.GetWidth() - 100, pdfFormXObject.GetHeight() - 90, 100, 75);
            pdfCanvas.Rectangle(rect);
            pdfCanvas.RestoreState();

            var paragraph = new Paragraph("302\r\nPicture\r\n not available").SetTextAlignment(TextAlignment.CENTER).SetFontColor(ColorConstants.BLACK);
            using (var c = new Canvas(pdfCanvas, pdfDocument, rect, true))
            {
                c.Add(paragraph);
                c.Close();
            }

            var image = new Image(pdfFormXObject).SetHeight(100).SetWidth(100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            var cell = new Cell().SetPaddingTop(Spacing).SetPaddingBottom(Spacing).Add(image);

            return cell;
        }

        private static Paragraph CreateMessage()
        {
            var paragraph = new Paragraph();
            paragraph.SetWidth(UnitValue.CreatePercentValue(100));
            paragraph.SetMarginTop(Spacing);

            paragraph.Add(new Text("Resume generated using a console application.\r\nCode located at: https://www.github.com/orion538/resume"));
            paragraph.SetTextAlignment(TextAlignment.CENTER);

            return paragraph;
        }

        private static Cell CreateDefaultCell(string text, bool isBold = false)
        {
            var paragraphText = new Text(text).SetFont(PdfFontFactory.CreateFont(isBold ? StandardFonts.HELVETICA_BOLD : StandardFonts.HELVETICA));
            return new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(paragraphText));
        }
    }
}
