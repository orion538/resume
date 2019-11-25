using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Resume.Data.Json;
using System.Collections.Generic;

namespace Resume.Pdf
{
    public interface IResumeGenerator
    {
        void Create(Data.Json.Resume resume);
    }

    public class ResumeGenerator : IResumeGenerator
    {
        private const string FileName = "Resume.pdf";

        public void Create(Data.Json.Resume resume)
        {
            var document = CreateDocument();
            document.Add(CreateBasics(resume.Basics));
            document.Add(CreateWork(resume.Work));
            document.Add(CreateEducation(resume.Education));
            document.Add(CreateLanguages(resume.Languages));
            document.Add(CreateInterests(resume.Interests));
            document.Add(CreateSkills(resume.Skills));
            document.Close();
        }

        private static Document CreateDocument()
        {
            var writer = new PdfWriter(FileName);
            var pdf = new PdfDocument(writer);
            return new Document(pdf);
        }

        private Table CreateBasics(Basics basics)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Basics")));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetPaddingTop(20).Add(new Paragraph(nameof(basics.Name))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetPaddingTop(20).Add(new Paragraph(basics.Name)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Location.Address))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Location.Address)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Location.PostalCode))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Location.PostalCode)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Location.Region))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Location.Region)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Location.City))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Location.City)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Location.CountryCode))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Location.CountryCode)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Email))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Email)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Phone))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Phone)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Picture))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Picture)));

            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(basics.Summary))));
            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(basics.Summary)));

            return table;
        }

        private Table CreateWork(IEnumerable<Work> previousWork)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(20);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Work experience")));

            foreach (var work in previousWork)
            {
                table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(20));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.Position))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(work.Position)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.Company))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(work.Company)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.StartDate))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(work.StartDate)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.EndDate))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(work.EndDate)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.Summary))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(work.Summary)));

                if (work.Highlights == null)
                {
                    continue;
                }

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(work.Highlights))));

                var highlightCell = new Cell().SetBorder(Border.NO_BORDER);

                foreach (var highlight in work.Highlights)
                {
                    highlightCell.Add(new Paragraph(highlight));
                }

                table.AddCell(highlightCell);
            }

            return table;
        }

        private Table CreateEducation(IEnumerable<Education> completeEducation)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(20);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Education")));

            foreach (var education in completeEducation)
            {
                table.AddCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetHeight(20));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(education.Institution))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(education.Institution)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(education.StudyType))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(education.StudyType)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(education.StartDate))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(education.StartDate)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(education.EndDate))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(education.EndDate)));

                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(nameof(education.Area))));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(education.Area)));
            }

            return table;
        }

        private static Table CreateLanguages(IEnumerable<Language> languages)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(20);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Languages")));

            foreach (var language in languages)
            {
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(language.Name)));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(language.Level)));
            }

            return table;
        }

        private static Table CreateInterests(IEnumerable<Interest> interests)
        {
            var table = new Table(new float[] { 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(20);

            table.AddHeaderCell(new Cell(1, 2).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Interests")));

            foreach (var interest in interests)
            {
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(interest.Name)));
                
                var keywordsCell = new Cell().SetBorder(Border.NO_BORDER);

                if (interest.Keywords != null)
                {
                    foreach (var keyword in interest.Keywords)
                    {
                        keywordsCell.Add(new Paragraph(keyword));
                    }
                }

                table.AddCell(keywordsCell);
            }

            return table;
        }

        private static Table CreateSkills(IEnumerable<Skill> skills)
        {
            var table = new Table(new float[] { 1, 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetMarginTop(20);

            table.AddHeaderCell(new Cell(1, 3).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(1)).Add(new Paragraph("Skills")));

            foreach (var skill in skills)
            {
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(skill.Name)));
                table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(skill.Level)));

                var keywordsCell = new Cell().SetBorder(Border.NO_BORDER);

                if (skill.Keywords != null)
                {
                    foreach (var keyword in skill.Keywords)
                    {
                        keywordsCell.Add(new Paragraph(keyword));
                    }
                }

                table.AddCell(keywordsCell);
            }

            return table;
        }
    }
}
