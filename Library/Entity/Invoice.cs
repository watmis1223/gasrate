using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Data;
using CalculationOilPrice.Library.Storage;
using System.IO;
using PdfSharp.Drawing;

namespace CalculationOilPrice.Library.Entity
{
    public class Invoice
    {
        Order currentOrder;
        
        Document document;
        //TextFrame addressFrame;
        //Table table;

        // Some pre-defined colors
#if true
        // RGB colors
        readonly static Color TableBorder = new Color(81, 125, 192);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);
#else
    // CMYK colors
    readonly static Color tableBorder = Color.FromCmyk(100, 50, 0, 30);
    readonly static Color tableBlue = Color.FromCmyk(0, 80, 50, 30);
    readonly static Color tableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif

        public Invoice()
        {

        }

        public void CreateInvoice(Order order)
        {
            if (order.OrderDetailDt == null)
            {
                return;
            }

            currentOrder = order;

            if (currentOrder == null)
            {
                return;
            }

            CreateDocument();
            document.UseCmykColor = true;

#if DEBUG
            // for debugging only...
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            //document = MigraDoc.DocumentObjectModel.IO.DdlReader.DocumentFromFile("MigraDoc.mdddl");
#endif

            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;

            // Create a renderer for PDF that uses Unicode font encoding
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);            

            // Set the MigraDoc document
            pdfRenderer.Document = document;
            
            // Create the PDF document
            pdfRenderer.RenderDocument();

            // Save the PDF document...
            //string filename = "Invoice.pdf";
            //string filename = string.Format("Invoice - {0}-{1} - {2}.pdf",
            //    currentOrder.OrderDate.ToString("yyyy.MM.dd"), DateTime.Now.ToString("HH.mm.ss"), currentOrder.OrderID.ToString().ToUpper());
            
            string filename = string.Format("{0}.pdf", currentOrder.GetReportName());

#if DEBUG
            // I don't want to close the document constantly...
            //filename = "Invoice-" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
            //filename = string.Format("Invoice - {0}-{1} - {2}.pdf", 
            //    currentOrder.OrderDate.ToString("yyyy.MM.dd"), DateTime.Now.ToString("HH.mm.ss"), currentOrder.OrderID.ToString().ToUpper());

            filename = string.Format("{0}.pdf", currentOrder.GetReportName());
#endif

            filename = string.Format("{0}\\{1}", ApplicationOperator.GetGeneralSetting().ReportPathSetting.ReportPath, filename);
            pdfRenderer.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        Document CreateDocument()
        {
            // Create a new MigraDoc document
            this.document = new Document();
            //this.document.Info.Title = "A sample invoice";
            //this.document.Info.Subject = "Demonstrates how to create an invoice.";
            //this.document.Info.Author = "Stefan Lange";

            DefineStyles();

            CreateCalculationSection();
            CreateCommissionSection();

            //FillContent();

            return this.document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            
            style.Font.Name = "Verdana";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            //style.Font.Name = "Times New Roman";
            
            style.Font.Size = 7;
            
            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);            
        }

        Section reportSection;

        void CreateCalculationSection()
        {
            // Each MigraDoc document needs at least one section.
            //Section section = this.document.AddSection();
            reportSection = this.document.AddSection();

            //// Put a logo in the header
            //Image image = section.Headers.Primary.AddImage("../../PowerBooks.png");
            //image.Height = "2.5cm";
            //image.LockAspectRatio = true;
            //image.RelativeVertical = RelativeVertical.Line;
            //image.RelativeHorizontal = RelativeHorizontal.Margin;
            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Right;
            //image.WrapFormat.Style = WrapStyle.Through;

            // Create footer
            //Paragraph paragraph = section.Footers.Primary.AddParagraph();            
            //paragraph.AddText("PowerBooks Inc ท Sample Street 42 ท 56789 Cologne ท Germany");
            //paragraph.Format.Font.Size = 9;
            //paragraph.Format.Alignment = ParagraphAlignment.Center;

            //// Create the text frame for the address
            //this.addressFrame = section.AddTextFrame();
            //this.addressFrame.Height = "3.0cm";
            //this.addressFrame.Width = "7.0cm";
            //this.addressFrame.Left = ShapePosition.Left;
            //this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            //this.addressFrame.Top = "2.5cm";
            //this.addressFrame.RelativeVertical = RelativeVertical.Page;

            //// Put sender in address frame
            //Paragraph paragraph = this.addressFrame.AddParagraph();            

            Paragraph paragraph = null;            


            paragraph = reportSection.Headers.Primary.AddParagraph();
            paragraph.AddText(string.Format("Kalkulation {0}", currentOrder.OrderNumber));
            paragraph.Format.Font.Size = 15;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            paragraph = reportSection.AddParagraph();
            paragraph.AddText(currentOrder.OrderDate);
            paragraph.AddLineBreak();
            paragraph.AddText(string.Format("{0} {1}", currentOrder.CustomerName, currentOrder.CustomerSurname));
            paragraph.AddLineBreak();
            paragraph.AddText(currentOrder.CustomerAddress);
            paragraph.AddLineBreak();
            paragraph.AddText(string.Format("{0} {1}", currentOrder.CustomerZipcode, currentOrder.CustomerPlace));
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            

            ////Create the text frame for the order information.
            //TextFrame orderInfoFrame = section.AddTextFrame();
            //orderInfoFrame.Height = "3.0cm";
            //orderInfoFrame.Width = "10.0cm";
            //orderInfoFrame.Left = ShapePosition.Left;
            //orderInfoFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            //orderInfoFrame.Top = "5cm";
            //orderInfoFrame.RelativeVertical = RelativeVertical.Page;

            //// Put sender in address frame
            decimal quantityL = currentOrder.OilQuantity / ApplicationOperator.GetGeneralSetting().OilPriceSetting.DensityOfHeatOil;
            paragraph = reportSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.AddText(string.Format("{0}   {1:n0} Kg / {2:n0} L", currentOrder.OilQualityName, currentOrder.OilQuantity, quantityL));
            paragraph.AddLineBreak();
            paragraph.AddText(string.Format("Einkaufspreis   {0:n2} CHF", currentOrder.OilPrice));
            paragraph.AddLineBreak();
            decimal oilFactorPrice = currentOrder.OilPrice * currentOrder.WIRFactor;
            paragraph.AddText(string.Format("Preis per 100 Kg ({0:n2} x {1:n2})   {2:n2} CHF", currentOrder.OilPrice, currentOrder.WIRFactor, oilFactorPrice));
            paragraph.Format.Font.Size = 8;
            paragraph.Format.Alignment = ParagraphAlignment.Left;

            //// Put sender in address frame
            //paragraph = this.addressFrame.AddParagraph("PowerBooks Inc ท Sample Street 42 ท 56789 Cologne");
            //paragraph.Format.Font.Name = "Times New Roman";
            //paragraph.Format.Font.Size = 7;
            //paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = reportSection.AddParagraph();
            paragraph.Format.SpaceBefore = "0.5cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Kalkulation", TextFormat.Bold);
            //paragraph.AddTab();
            //paragraph.AddText("Cologne, ");
            //paragraph.AddDateField("dd.MM.yyyy");

            //paragraph = section.AddParagraph();
            //paragraph.Format.SpaceBefore = "2cm";

            DataRow[] orderItems = null;
            Table orderDetailTable = null;
            Row headerRow = null;

            for (int i = 1; i <= currentOrder.UnloadPlaceNumber; i++)
            {                
                //paragraph = section.AddParagraph();
                //paragraph.Format.SpaceBefore = "2cm";

                // Create the item table
                orderDetailTable = reportSection.AddTable();
                orderDetailTable.Style = "Table";
                orderDetailTable.Borders.Color = TableBorder;
                orderDetailTable.Borders.Width = 0.25;
                orderDetailTable.Borders.Left.Width = 0.5;
                orderDetailTable.Borders.Right.Width = 0.5;
                orderDetailTable.Rows.LeftIndent = 0;

                // Before you can add a row, you must define the columns
                Column column = orderDetailTable.AddColumn("1cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = orderDetailTable.AddColumn("10cm");
                column.Format.Alignment = ParagraphAlignment.Left;

                column = orderDetailTable.AddColumn("2.5cm");
                column.Format.Alignment = ParagraphAlignment.Right;

                column = orderDetailTable.AddColumn("2.5cm");
                column.Format.Alignment = ParagraphAlignment.Right;

                // Create the header of the table
                headerRow = orderDetailTable.AddRow();
                headerRow.HeadingFormat = true;
                headerRow.Format.Alignment = ParagraphAlignment.Center;
                headerRow.Format.Font.Bold = true;
                headerRow.Shading.Color = TableBlue;

                headerRow.Cells[0].AddParagraph("Pos.");
                headerRow.Cells[0].Format.Font.Bold = true;
                headerRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
                //headerRow.Cells[0].MergeDown = 1;

                headerRow.Cells[1].AddParagraph("Zuschlag");
                headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                headerRow.Cells[2].AddParagraph("");
                headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
                headerRow.Cells[3].AddParagraph("Betrag");
                headerRow.Cells[3].Format.Alignment = ParagraphAlignment.Center;


                //headerRow.Cells[1].AddParagraph(string.Format("Unload Place No.{0}", i));
                //headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                //headerRow.Cells[1].MergeRight = 2;

                //headerRow = orderDetailTable.AddRow();
                //headerRow.HeadingFormat = true;
                //headerRow.Format.Alignment = ParagraphAlignment.Center;
                //headerRow.Format.Font.Bold = true;
                //headerRow.Shading.Color = TableBlue;
                //headerRow.Cells[1].AddParagraph("Zuschlag");
                //headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                //headerRow.Cells[2].AddParagraph("");
                //headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
                //headerRow.Cells[3].AddParagraph("Betrag");
                //headerRow.Cells[3].Format.Alignment = ParagraphAlignment.Center;

                orderDetailTable.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
                //this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

                orderItems = currentOrder.OrderDetailDt.Select(string.Format("UnloadPlaceNumber={0}", i));

                if (orderItems == null || orderItems.Length == 0)
                {
                    Row emptyRow = orderDetailTable.AddRow();
                    emptyRow.Cells[0].AddParagraph("No items");
                    emptyRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                    emptyRow.Cells[0].MergeRight = 3;
                }
                //else
                //{
                //    FillOrderDetailContent(orderItems, orderDetailTable);
                //}

                FillOrderDetailContent(orderItems, orderDetailTable);
            }

            if (orderDetailTable == null)
            {
                return;
            }

            // Add an invisible row as a space line to the table
            Row orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Borders.Visible = false;

            // Add the total price row
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph("Total Zuschläge");
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            orderTotalRow.Cells[3].AddParagraph(string.Format("{0:n2}", orderDetailSubSumPrice));
            orderTotalRow.Cells[3].Format.Font.Bold = true;

            ///
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph(string.Format("Abladestellen ({0:n2} x {1:n0})",
                currentOrder.UnloadCost, currentOrder.UnloadPlace));
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            decimal unloadCost = currentOrder.UnloadCost * currentOrder.UnloadPlace;

            orderTotalRow.Cells[3].AddParagraph(unloadCost.ToString("n2"));
            orderTotalRow.Cells[3].Format.Font.Bold = true;


            ///
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph("VP für 100 kg in WIR");
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            decimal vpPer100KgInWir = currentOrder.OilPrice * currentOrder.WIRFactor;
            orderTotalRow.Cells[3].AddParagraph(string.Format("{0:n2}", vpPer100KgInWir));
            orderTotalRow.Cells[3].Format.Font.Bold = true;




            //orderTotalRow = orderDetailTable.AddRow();
            //orderTotalRow.Cells[0].Borders.Visible = false;
            //orderTotalRow.Cells[0].AddParagraph("Preis per 100 Kg");
            //orderTotalRow.Cells[0].Format.Font.Bold = true;
            //orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //orderTotalRow.Cells[0].MergeRight = 2;
            //orderTotalRow.Cells[3].AddParagraph(string.Format("{0:n2}", currentOrder.OilPrice * currentOrder.WIRFactor));
            //orderTotalRow.Cells[3].Format.Font.Bold = true;


            //orderTotalRow = orderDetailTable.AddRow();
            //orderTotalRow.Cells[0].Borders.Visible = false;
            //orderTotalRow.Cells[0].AddParagraph(string.Format("VP für 100 Kg ({0:n2} + {1:n2})", 
            //    orderDetailSubSumPrice, (currentOrder.OilPrice * currentOrder.WIRFactor)));
            //orderTotalRow.Cells[0].Format.Font.Bold = true;
            //orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //orderTotalRow.Cells[0].MergeRight = 2;
            //orderTotalRow.Cells[3].AddParagraph(string.Format("{0:n2}", currentOrder.TotalPricePer100KG));
            //orderTotalRow.Cells[3].Format.Font.Bold = true;


            ///
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph("VP für 100 kg in CHF");
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            orderTotalRow.Cells[3].AddParagraph(string.Format("{0:n2}", currentOrder.OilPrice));
            orderTotalRow.Cells[3].Format.Font.Bold = true; 


            ///Total I
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            decimal quantityDifferenceWir = 0;
            decimal oilQuantity = 0;
            oilQuantity = (currentOrder.OilQuantity / 100);
            quantityDifferenceWir = oilQuantity * (currentOrder.PartOfWIR / 100);
            orderTotalRow.Cells[0].AddParagraph(string.Format("Subtotal WIR ({0:n2} x {1:n0})",
                vpPer100KgInWir, quantityDifferenceWir));
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            orderTotalRow.Cells[3].AddParagraph(currentOrder.Total1Price.ToString("n2"));
            orderTotalRow.Cells[3].Format.Font.Bold = true;                       

            ///Total II
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            decimal quantityDifferenceCHF = 0;
            oilQuantity = (currentOrder.OilQuantity / 100);
            quantityDifferenceCHF = oilQuantity * (currentOrder.PartOfWIR / 100);
            quantityDifferenceCHF = oilQuantity - quantityDifferenceCHF;
            //orderTotalRow.Cells[0].AddParagraph(string.Format("Total II ({0:n2} + {1:n2})",
            //    currentOrder.Total1Price, unloadCost));
            //orderTotalRow.Cells[0].AddParagraph(string.Format("Total II ({0:n2} + {1:n2} + {2:n2})",
            //    currentOrder.Total2Price, unloadCost, orderDetailSubSumPrice));
            orderTotalRow.Cells[0].AddParagraph(string.Format("Subtotal CHF (({0:n2} x {1:n0}) + {2:n2} + {3:n2})",
                new object[] { currentOrder.OilPrice, quantityDifferenceCHF, unloadCost, orderDetailSubSumPrice }));
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            //orderTotalRow.Cells[3].AddParagraph(currentOrder.Total2Price.ToString("n2"));
            orderTotalRow.Cells[3].AddParagraph((currentOrder.Total2CashPrice).ToString("n2"));
            orderTotalRow.Cells[3].Format.Font.Bold = true;

            ///
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph("Average (Subtotal WIR and Subtotal CHF)");
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            orderTotalRow.Cells[3].AddParagraph(((currentOrder.Total1Price + currentOrder.Total2CashPrice)/2).ToString("n2"));
            orderTotalRow.Cells[3].Format.Font.Bold = true;

            ///
            orderTotalRow = orderDetailTable.AddRow();
            orderTotalRow.Cells[0].Borders.Visible = false;
            orderTotalRow.Cells[0].AddParagraph("Total (Subtotal WIR + Subtotal CHF)");
            orderTotalRow.Cells[0].Format.Font.Bold = true;
            orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            orderTotalRow.Cells[0].MergeRight = 2;
            orderTotalRow.Cells[3].AddParagraph((currentOrder.Total1Price + currentOrder.Total2CashPrice).ToString("n2"));
            orderTotalRow.Cells[3].Format.Font.Bold = true;


            //orderTotalRow = orderDetailTable.AddRow();
            //orderTotalRow.Cells[0].Borders.Visible = false;
            //orderTotalRow.Cells[0].AddParagraph(string.Format("WIR Anteil ({0}% from Total I)", currentOrder.PartOfWIR));
            //orderTotalRow.Cells[0].Format.Font.Bold = true;
            //orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //orderTotalRow.Cells[0].MergeRight = 2;
            //orderTotalRow.Cells[3].AddParagraph(currentOrder.Total1WIRPrice.ToString("n2"));
            //orderTotalRow.Cells[3].Format.Font.Bold = true;


            //orderTotalRow = orderDetailTable.AddRow();
            //orderTotalRow.Cells[0].Borders.Visible = false;
            ////orderTotalRow.Cells[0].AddParagraph(string.Format("Bar Anteil ({0}% from Total II)", currentOrder.PartOfWIR));
            //orderTotalRow.Cells[0].AddParagraph(string.Format("Bar Anteil ({0:n2} + {1:n2})", currentOrder.Total1WIRPrice, unloadCost));
            //orderTotalRow.Cells[0].Format.Font.Bold = true;
            //orderTotalRow.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            //orderTotalRow.Cells[0].MergeRight = 2;
            //orderTotalRow.Cells[3].AddParagraph(currentOrder.Total2CashPrice.ToString("n2"));
            //orderTotalRow.Cells[3].Format.Font.Bold = true;
        }

        void CreateCommissionSection()
        {
            // Each MigraDoc document needs at least one section.
            //Section section = this.document.AddSection();

            Paragraph paragraph = null;
            paragraph = reportSection.AddParagraph();
            paragraph.Format.SpaceBefore = "0.5cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Provision", TextFormat.Bold);

            // Create the commission table
            Table commissionTable = null;
            commissionTable = reportSection.AddTable();
            commissionTable.Style = "Table";
            commissionTable.Borders.Color = TableBorder;
            commissionTable.Borders.Width = 0.25;
            commissionTable.Borders.Left.Width = 0.5;
            commissionTable.Borders.Right.Width = 0.5;
            commissionTable.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = commissionTable.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = commissionTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = commissionTable.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row headerRow = null;
            headerRow = commissionTable.AddRow();
            headerRow.HeadingFormat = true;
            headerRow.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Format.Font.Bold = true;
            headerRow.Shading.Color = TableBlue;
                       
            headerRow.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[1].AddParagraph("Marge als Betrag");
            headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            headerRow.Cells[2].AddParagraph("Marge in Prozent ");
            headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Center;

            commissionTable.SetEdge(0, 0, 3, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            Row commissionRow = commissionTable.AddRow();            
            commissionRow.Cells[0].AddParagraph(string.Format("Kalk. Verkaufspreis ({0:n2} - {1:n2} x {2:n0})",
                currentOrder.TotalPricePer100KG, currentOrder.OilPrice, (currentOrder.OilQuantity / 100)));
            commissionRow.Cells[1].AddParagraph(string.Format("{0:n2}", currentOrder.Commission.CalculationSalesPriceAmount));
            commissionRow.Cells[2].AddParagraph(string.Format("{0:n2}%", currentOrder.Commission.CalculationSalesPricePercent));

            commissionRow = commissionTable.AddRow();
            commissionRow.Cells[0].AddParagraph("WIR Anteil %");
            commissionRow.Cells[0].MergeRight = 1;
            commissionRow.Cells[2].AddParagraph(string.Format("{0:n0}%", currentOrder.PartOfWIR));

            commissionRow = commissionTable.AddRow();
            commissionRow.Cells[0].AddParagraph(string.Format("WIR Korrektur ({0:n2} x {1:n2})",
                currentOrder.Commission.WIRCorrection, currentOrder.CommissionFactor));
            commissionRow.Cells[1].AddParagraph(string.Format("{0:n2}", currentOrder.Commission.WIRCorrectionAmount));
            commissionRow.Cells[2].Borders.Right.Visible = false;            

            commissionRow = commissionTable.AddRow();
            commissionRow.Cells[0].AddParagraph(string.Format("Provisionsberechtigte Marge ({0:n2} - {1:n2})",
                currentOrder.Commission.CalculationSalesPriceAmount, currentOrder.Commission.WIRCorrectionAmount));
            commissionRow.Cells[1].AddParagraph(string.Format("{0:n2}", currentOrder.Commission.CommissionLegitimateAmount));
            commissionRow.Cells[2].AddParagraph(string.Format("{0:n2}%", currentOrder.Commission.CommissionLegitimatePercent));

            commissionRow = commissionTable.AddRow();
            commissionRow.Borders.Visible = false;

            commissionRow = commissionTable.AddRow();
            commissionRow.Cells[0].AddParagraph("Provision gemäss Matrix in Prozent");
            commissionRow.Cells[0].MergeRight = 1;                                    
            commissionRow.Cells[2].AddParagraph(string.Format("{0:n0}%", currentOrder.Commission.TotalCommissionPercent));
            commissionRow.Cells[2].Format.Font.Bold = true;

            commissionRow = commissionTable.AddRow();
            commissionRow.Cells[0].AddParagraph("zu zahlende Provision");            
            commissionRow.Cells[1].AddParagraph(string.Format("{0:n2}", currentOrder.Commission.TotalCommissionAmount));
            commissionRow.Cells[1].Format.Font.Bold = true;
            commissionRow.Cells[2].Borders.Right.Visible = false;
            commissionRow.Cells[2].Borders.Bottom.Visible = false;

            commissionTable.SetEdge(2, 6, 1, 1, Edge.Bottom, BorderStyle.Single, 1);
            commissionTable.SetEdge(1, 7, 1, 1, Edge.Bottom, BorderStyle.Single, 1);
        }

        decimal orderDetailSubSumPrice = 0;

        /// <summary>
        /// Creates the dynamic parts of the invoice.
        /// </summary>
        void FillOrderDetailContent(DataRow[] orderDetailItems, Table orderDetailTable)
        {
            int itemCount = 0;
            string itemSubPrice = string.Empty;            
            Row orderDetailRow = null;
            decimal subSurchargePrice = 0;

            foreach (DataRow dr in orderDetailItems)
            {
                itemCount++;

                // Each item fills two rows
                orderDetailRow = orderDetailTable.AddRow();
                
                orderDetailRow.Cells[0].AddParagraph(itemCount.ToString());
                orderDetailRow.Cells[1].AddParagraph(dr["ItemDescription"].ToString());

                if (Convert.ToDecimal(dr["ItemCostPercent"]) != 0)
                {
                    itemSubPrice = string.Format("{0:n2}%", dr["ItemCostPercent"]);
                }
                else
                {
                    itemSubPrice = string.Format("{0:n2}", dr["ItemCostCash"]);
                }

                orderDetailRow.Cells[2].AddParagraph(itemSubPrice);

                orderDetailSubSumPrice += Convert.ToDecimal(dr["AdditionalCosts"]);
                subSurchargePrice += Convert.ToDecimal(dr["AdditionalCosts"]);

                orderDetailRow.Cells[3].AddParagraph(string.Format("{0:n2}", dr["AdditionalCosts"]));                
            }

            orderDetailRow = orderDetailTable.AddRow();
            orderDetailRow.Cells[0].Shading.Color = TableGray;
            orderDetailRow.Cells[1].MergeRight = 1;
            orderDetailRow.Cells[1].AddParagraph("Total");
            orderDetailRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            orderDetailRow.Cells[1].Format.Font.Bold = true;
            orderDetailRow.Cells[3].AddParagraph(string.Format("{0:n2}", subSurchargePrice));
            orderDetailRow.Cells[3].Format.Font.Bold = true;
            orderDetailTable.SetEdge(3, orderDetailTable.Rows.Count - 1, 1, 1, Edge.Bottom, BorderStyle.Single, 1);
            orderDetailTable.SetEdge(3, orderDetailTable.Rows.Count - 2, 1, 1, Edge.Bottom, BorderStyle.Single, 1);
        }
    }
}