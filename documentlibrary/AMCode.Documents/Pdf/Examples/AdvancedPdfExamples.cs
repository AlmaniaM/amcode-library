using System;
using System.IO;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf.Examples
{
    /// <summary>
    /// Advanced PDF usage examples demonstrating complex scenarios
    /// </summary>
    public static class AdvancedPdfExamples
    {
        /// <summary>
        /// Example: Creating a professional invoice document
        /// </summary>
        public static void CreateInvoiceDocument()
        {
            Console.WriteLine("=== Creating Professional Invoice ===");

            var properties = new PdfProperties
            {
                Title = "Invoice #INV-2024-001",
                Author = "AMCode Corporation",
                Subject = "Professional Invoice",
                Keywords = "invoice, billing, professional",
                Creator = "AMCode PDF Library",
                Producer = "AMCode PDF Library v1.0"
            };

            var result = PdfFactory.CreateDocument(properties);
            if (result.IsSuccess)
            {
                var document = result.Value;
                var page = document.Pages.Create();

                // Header
                page.AddText("AMCode Corporation", 50, 750, new FontStyle { FontSize = 24, Bold = true });
                page.AddText("123 Business Street", 50, 730);
                page.AddText("Business City, BC 12345", 50, 715);
                page.AddText("Phone: (555) 123-4567", 50, 700);
                page.AddText("Email: billing@amcode.com", 50, 685);

                // Invoice title
                page.AddText("INVOICE", 400, 750, new FontStyle { FontSize = 28, Bold = true });
                page.AddText("Invoice #: INV-2024-001", 400, 720);
                page.AddText("Date: " + DateTime.Now.ToString("MMM dd, yyyy"), 400, 705);
                page.AddText("Due Date: " + DateTime.Now.AddDays(30).ToString("MMM dd, yyyy"), 400, 690);

                // Bill to section
                page.AddText("Bill To:", 50, 650, new FontStyle { FontSize = 14, Bold = true });
                page.AddText("Client Company Name", 50, 630);
                page.AddText("456 Client Avenue", 50, 615);
                page.AddText("Client City, CC 67890", 50, 600);

                // Invoice table
                var table = page.AddTable(50, 550, 4, 4);
                
                // Headers
                table.SetCellValue(0, 0, "Description");
                table.SetCellValue(0, 1, "Quantity");
                table.SetCellValue(0, 2, "Unit Price");
                table.SetCellValue(0, 3, "Total");

                // Data rows
                table.SetCellValue(1, 0, "Software Development Services");
                table.SetCellValue(1, 1, "40");
                table.SetCellValue(1, 2, "$150.00");
                table.SetCellValue(1, 3, "$6,000.00");

                table.SetCellValue(2, 0, "PDF Library Integration");
                table.SetCellValue(2, 1, "20");
                table.SetCellValue(2, 2, "$200.00");
                table.SetCellValue(2, 3, "$4,000.00");

                table.SetCellValue(3, 0, "Testing and Quality Assurance");
                table.SetCellValue(3, 1, "15");
                table.SetCellValue(3, 2, "$100.00");
                table.SetCellValue(3, 3, "$1,500.00");

                // Totals
                page.AddLine(350, 400, 500, 400, AMCode.Documents.Common.Drawing.Color.Black, 1.0);
                page.AddText("Subtotal:", 350, 380);
                page.AddText("$11,500.00", 450, 380);
                page.AddText("Tax (8%):", 350, 360);
                page.AddText("$920.00", 450, 360);
                page.AddLine(350, 340, 500, 340, AMCode.Documents.Common.Drawing.Color.Black, 2.0);
                page.AddText("Total:", 350, 320, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("$12,420.00", 450, 320, new FontStyle { FontSize = 16, Bold = true });

                // Footer
                page.AddText("Thank you for your business!", 50, 250, new FontStyle { FontSize = 12, Italic = true });
                page.AddText("Payment terms: Net 30 days", 50, 230);
                page.AddText("Late payments may incur additional fees", 50, 215);

                document.SaveAs("ProfessionalInvoice.pdf");
                Console.WriteLine("Professional invoice saved as ProfessionalInvoice.pdf");
                document.Dispose();
            }
        }

        /// <summary>
        /// Example: Creating a technical report with charts and diagrams
        /// </summary>
        public static void CreateTechnicalReport()
        {
            Console.WriteLine("=== Creating Technical Report ===");

            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;

                // Title page
                var titlePage = document.Pages.Create();
                titlePage.AddText("TECHNICAL REPORT", 200, 400, new FontStyle { FontSize = 32, Bold = true });
                titlePage.AddText("PDF Library Performance Analysis", 150, 350, new FontStyle { FontSize = 20 });
                titlePage.AddText("Prepared by: AMCode Development Team", 200, 300);
                titlePage.AddText("Date: " + DateTime.Now.ToString("MMMM dd, yyyy"), 200, 280);
                titlePage.AddRectangle(100, 200, 400, 300, AMCode.Documents.Common.Drawing.Color.LightGray, AMCode.Documents.Common.Drawing.Color.DarkGray);

                // Executive summary page
                var summaryPage = document.Pages.Create();
                summaryPage.AddText("Executive Summary", 50, 750, new FontStyle { FontSize = 18, Bold = true });
                summaryPage.AddText("This report analyzes the performance characteristics of the AMCode PDF Library", 50, 700);
                summaryPage.AddText("across different usage scenarios and provider implementations.", 50, 680);
                
                summaryPage.AddText("Key Findings:", 50, 630, new FontStyle { FontSize = 14, Bold = true });
                summaryPage.AddText("• QuestPDF provider shows superior performance for complex layouts", 70, 600);
                summaryPage.AddText("• iTextSharp provider excels at document manipulation and editing", 70, 580);
                summaryPage.AddText("• Memory usage remains stable across high-volume operations", 70, 560);
                summaryPage.AddText("• Fluent API reduces development time by approximately 40%", 70, 540);

                // Performance metrics page
                var metricsPage = document.Pages.Create();
                metricsPage.AddText("Performance Metrics", 50, 750, new FontStyle { FontSize = 18, Bold = true });
                
                // Create a mock chart using rectangles and text
                metricsPage.AddText("Document Creation Time (ms)", 200, 700, new FontStyle { FontSize = 12, Bold = true });
                
                // Bar chart representation
                metricsPage.AddRectangle(100, 600, 50, 80, AMCode.Documents.Common.Drawing.Color.Blue); // QuestPDF
                metricsPage.AddText("QuestPDF", 100, 580);
                metricsPage.AddText("2.5ms", 105, 560);

                metricsPage.AddRectangle(200, 600, 50, 60, AMCode.Documents.Common.Drawing.Color.Green); // iTextSharp
                metricsPage.AddText("iTextSharp", 200, 580);
                metricsPage.AddText("3.2ms", 205, 560);

                metricsPage.AddRectangle(300, 600, 50, 40, AMCode.Documents.Common.Drawing.Color.Red); // Memory Efficient
                metricsPage.AddText("Memory Efficient", 300, 580);
                metricsPage.AddText("1.8ms", 305, 560);

                // Memory usage chart
                metricsPage.AddText("Memory Usage (KB)", 200, 500, new FontStyle { FontSize = 12, Bold = true });
                metricsPage.AddRectangle(100, 400, 50, 100, AMCode.Documents.Common.Drawing.Color.LightBlue);
                metricsPage.AddText("Standard", 100, 380);
                metricsPage.AddText("120KB", 105, 360);

                metricsPage.AddRectangle(200, 400, 50, 60, AMCode.Documents.Common.Drawing.Color.LightGreen);
                metricsPage.AddText("Optimized", 200, 380);
                metricsPage.AddText("80KB", 205, 360);

                // Recommendations page
                var recommendationsPage = document.Pages.Create();
                recommendationsPage.AddText("Recommendations", 50, 750, new FontStyle { FontSize = 18, Bold = true });
                
                recommendationsPage.AddText("1. Provider Selection", 50, 700, new FontStyle { FontSize = 14, Bold = true });
                recommendationsPage.AddText("   Use QuestPDF for new document generation and complex layouts.", 70, 680);
                recommendationsPage.AddText("   Use iTextSharp for document manipulation and legacy compatibility.", 70, 660);

                recommendationsPage.AddText("2. Performance Optimization", 50, 620, new FontStyle { FontSize = 14, Bold = true });
                recommendationsPage.AddText("   Implement document pooling for high-volume scenarios.", 70, 600);
                recommendationsPage.AddText("   Use memory-efficient factory for batch operations.", 70, 580);

                recommendationsPage.AddText("3. Development Best Practices", 50, 540, new FontStyle { FontSize = 14, Bold = true });
                recommendationsPage.AddText("   Leverage fluent API for improved code readability.", 70, 520);
                recommendationsPage.AddText("   Always validate documents before saving.", 70, 500);
                recommendationsPage.AddText("   Implement proper error handling and logging.", 70, 480);

                document.SaveAs("TechnicalReport.pdf");
                Console.WriteLine("Technical report saved as TechnicalReport.pdf");
                document.Dispose();
            }
        }

        /// <summary>
        /// Example: Creating a form with interactive elements
        /// </summary>
        public static void CreateFormDocument()
        {
            Console.WriteLine("=== Creating Form Document ===");

            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;
                var page = document.Pages.Create();

                // Form title
                page.AddText("EMPLOYEE INFORMATION FORM", 200, 750, new FontStyle { FontSize = 20, Bold = true });
                page.AddText("Please fill out all required fields", 220, 720, new FontStyle { FontSize = 12, Italic = true });

                // Personal Information Section
                page.AddText("Personal Information", 50, 680, new FontStyle { FontSize = 16, Bold = true });
                page.AddLine(50, 670, 550, 670, AMCode.Documents.Common.Drawing.Color.Black, 1.0);

                // Form fields (represented as rectangles for now)
                page.AddText("First Name:", 50, 640);
                page.AddRectangle(150, 635, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("Last Name:", 50, 610);
                page.AddRectangle(150, 605, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("Email:", 50, 580);
                page.AddRectangle(150, 575, 300, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("Phone:", 50, 550);
                page.AddRectangle(150, 545, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                // Address Section
                page.AddText("Address", 50, 500, new FontStyle { FontSize = 16, Bold = true });
                page.AddLine(50, 490, 550, 490, AMCode.Documents.Common.Drawing.Color.Black, 1.0);

                page.AddText("Street Address:", 50, 460);
                page.AddRectangle(150, 455, 350, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("City:", 50, 430);
                page.AddRectangle(150, 425, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("State:", 50, 400);
                page.AddRectangle(150, 395, 100, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("ZIP Code:", 50, 370);
                page.AddRectangle(150, 365, 100, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                // Employment Section
                page.AddText("Employment Information", 50, 320, new FontStyle { FontSize = 16, Bold = true });
                page.AddLine(50, 310, 550, 310, AMCode.Documents.Common.Drawing.Color.Black, 1.0);

                page.AddText("Department:", 50, 280);
                page.AddRectangle(150, 275, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("Position:", 50, 250);
                page.AddRectangle(150, 245, 200, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                page.AddText("Start Date:", 50, 220);
                page.AddRectangle(150, 215, 150, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                // Checkboxes (represented as squares)
                page.AddText("Employment Status:", 50, 180);
                page.AddRectangle(200, 175, 15, 15, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);
                page.AddText("Full-time", 220, 180);
                page.AddRectangle(300, 175, 15, 15, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);
                page.AddText("Part-time", 320, 180);
                page.AddRectangle(400, 175, 15, 15, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);
                page.AddText("Contract", 420, 180);

                // Signature section
                page.AddText("Signature:", 50, 130);
                page.AddRectangle(150, 100, 300, 30, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);
                page.AddText("Date:", 50, 80);
                page.AddRectangle(150, 75, 150, 20, AMCode.Documents.Common.Drawing.Color.White, AMCode.Documents.Common.Drawing.Color.Black);

                // Footer
                page.AddText("This form is confidential and for internal use only.", 50, 40, new FontStyle { FontSize = 10, Italic = true });

                document.SaveAs("EmployeeForm.pdf");
                Console.WriteLine("Employee form saved as EmployeeForm.pdf");
                document.Dispose();
            }
        }

        /// <summary>
        /// Example: Creating a multi-language document
        /// </summary>
        public static void CreateMultiLanguageDocument()
        {
            Console.WriteLine("=== Creating Multi-Language Document ===");

            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;
                var page = document.Pages.Create();

                // Title in multiple languages
                page.AddText("Welcome / Bienvenue / Willkommen", 200, 750, new FontStyle { FontSize = 20, Bold = true });

                // English section
                page.AddText("English", 50, 700, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Welcome to the AMCode PDF Library!", 50, 680);
                page.AddText("This library provides comprehensive PDF generation capabilities", 50, 660);
                page.AddText("with support for multiple providers and clean architecture.", 50, 640);

                // French section
                page.AddText("Français", 50, 600, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Bienvenue dans la bibliothèque PDF AMCode!", 50, 580);
                page.AddText("Cette bibliothèque fournit des capacités complètes de génération PDF", 50, 560);
                page.AddText("avec support pour plusieurs fournisseurs et architecture propre.", 50, 540);

                // German section
                page.AddText("Deutsch", 50, 500, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Willkommen bei der AMCode PDF-Bibliothek!", 50, 480);
                page.AddText("Diese Bibliothek bietet umfassende PDF-Generierungsfunktionen", 50, 460);
                page.AddText("mit Unterstützung für mehrere Anbieter und saubere Architektur.", 50, 440);

                // Spanish section
                page.AddText("Español", 50, 400, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("¡Bienvenido a la biblioteca PDF AMCode!", 50, 380);
                page.AddText("Esta biblioteca proporciona capacidades completas de generación de PDF", 50, 360);
                page.AddText("con soporte para múltiples proveedores y arquitectura limpia.", 50, 340);

                // Features table
                var table = page.AddTable(50, 300, 3, 4);
                table.SetCellValue(0, 0, "Feature / Caractéristique / Funktion / Característica");
                table.SetCellValue(0, 1, "QuestPDF");
                table.SetCellValue(0, 2, "iTextSharp");

                table.SetCellValue(1, 0, "Document Creation");
                table.SetCellValue(1, 1, "✓");
                table.SetCellValue(1, 2, "✓");

                table.SetCellValue(2, 0, "Document Editing");
                table.SetCellValue(2, 1, "✗");
                table.SetCellValue(2, 2, "✓");

                table.SetCellValue(3, 0, "Performance");
                table.SetCellValue(3, 1, "High");
                table.SetCellValue(3, 2, "Medium");

                document.SaveAs("MultiLanguageDocument.pdf");
                Console.WriteLine("Multi-language document saved as MultiLanguageDocument.pdf");
                document.Dispose();
            }
        }

        /// <summary>
        /// Example: Creating a document with custom styling and branding
        /// </summary>
        public static void CreateBrandedDocument()
        {
            Console.WriteLine("=== Creating Branded Document ===");

            var properties = new PdfProperties
            {
                Title = "AMCode Brand Guidelines",
                Author = "AMCode Design Team",
                Subject = "Brand Identity and Usage Guidelines",
                Keywords = "brand, guidelines, identity, amcode"
            };

            var result = PdfFactory.CreateDocument(properties);
            if (result.IsSuccess)
            {
                var document = result.Value;
                var page = document.Pages.Create();

                // Brand header with logo area
                page.AddRectangle(0, 0, 600, 100, AMCode.Documents.Common.Drawing.Color.DarkBlue, AMCode.Documents.Common.Drawing.Color.DarkBlue);
                page.AddText("AMCODE", 50, 50, new FontStyle { FontSize = 32, Bold = true, Color = AMCode.Documents.Common.Drawing.Color.White });

                // Brand colors section
                page.AddText("Brand Colors", 50, 200, new FontStyle { FontSize = 18, Bold = true });
                
                // Color palette
                page.AddRectangle(50, 250, 80, 40, AMCode.Documents.Common.Drawing.Color.DarkBlue);
                page.AddText("Primary Blue", 140, 270);
                page.AddText("#1E3A8A", 140, 255);

                page.AddRectangle(50, 300, 80, 40, AMCode.Documents.Common.Drawing.Color.LightBlue);
                page.AddText("Secondary Blue", 140, 320);
                page.AddText("#3B82F6", 140, 305);

                page.AddRectangle(50, 350, 80, 40, AMCode.Documents.Common.Drawing.Color.Green);
                page.AddText("Success Green", 140, 370);
                page.AddText("#10B981", 140, 355);

                page.AddRectangle(50, 400, 80, 40, AMCode.Documents.Common.Drawing.Color.Red);
                page.AddText("Error Red", 140, 420);
                page.AddText("#EF4444", 140, 405);

                // Typography section
                page.AddText("Typography", 300, 200, new FontStyle { FontSize = 18, Bold = true });
                page.AddText("Headings", 300, 250, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Use bold, sans-serif fonts for headings", 300, 230);
                page.AddText("Body Text", 300, 300, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Use regular, readable fonts for body content", 300, 280);
                page.AddText("Code", 300, 350, new FontStyle { FontSize = 16, Bold = true });
                page.AddText("Use monospace fonts for code snippets", 300, 330);

                // Logo usage guidelines
                page.AddText("Logo Usage Guidelines", 50, 500, new FontStyle { FontSize = 18, Bold = true });
                page.AddText("• Maintain minimum clear space around logo", 50, 470);
                page.AddText("• Use approved color variations only", 50, 450);
                page.AddText("• Do not distort or modify the logo", 50, 430);
                page.AddText("• Ensure sufficient contrast for readability", 50, 410);

                // Footer
                page.AddRectangle(0, 700, 600, 50, AMCode.Documents.Common.Drawing.Color.LightGray, AMCode.Documents.Common.Drawing.Color.LightGray);
                page.AddText("© 2024 AMCode Corporation. All rights reserved.", 200, 720, new FontStyle { FontSize = 10 });

                document.SaveAs("BrandedDocument.pdf");
                Console.WriteLine("Branded document saved as BrandedDocument.pdf");
                document.Dispose();
            }
        }

        /// <summary>
        /// Run all advanced examples
        /// </summary>
        public static void RunAllAdvancedExamples()
        {
            Console.WriteLine("Running AMCode PDF Library Advanced Examples...\n");

            try
            {
                CreateInvoiceDocument();
                Console.WriteLine();

                CreateTechnicalReport();
                Console.WriteLine();

                CreateFormDocument();
                Console.WriteLine();

                CreateMultiLanguageDocument();
                Console.WriteLine();

                CreateBrandedDocument();
                Console.WriteLine();

                Console.WriteLine("All advanced examples completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running advanced examples: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
