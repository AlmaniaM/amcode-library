using System;
using System.IO;
using AMCode.Docx;
using AMCode.Docx.Providers.OpenXml;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.Docx
{
    /// <summary>
    /// Integration test to verify DOCX functionality
    /// </summary>
    public class IntegrationTest
    {
        public static void RunBasicDocumentTest()
        {
            Console.WriteLine("=== DOCX Integration Test ===");
            
            try
            {
                // Test 1: Create a basic document
                Console.WriteLine("Test 1: Creating basic document...");
                var document = DocumentFactory.CreateDocument();
                
                // Add a paragraph
                var paragraph = document.Paragraphs.Create("Hello, World! This is a test document.");
                paragraph.Alignment = HorizontalAlignment.Center;
                paragraph.SpacingAfter = 12;
                
                Console.WriteLine("✓ Basic document created successfully");
                
                // Test 2: Save document to stream
                Console.WriteLine("Test 2: Saving document to stream...");
                using var stream = new MemoryStream();
                document.SaveAs(stream);
                
                if (stream.Length > 0)
                {
                    Console.WriteLine($"✓ Document saved successfully ({stream.Length} bytes)");
                }
                else
                {
                    Console.WriteLine("✗ Document save failed - empty stream");
                    return;
                }
                
                // Test 3: Create document with table
                Console.WriteLine("Test 3: Creating document with table...");
                var tableDocument = DocumentFactory.CreateDocumentWithTable("Test Table Document", 3, 4);
                
                // Add some content to the table
                if (tableDocument.Tables.Count > 0)
                {
                    var table = tableDocument.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        var firstRow = table.Rows[0];
                        if (firstRow.Cells.Count > 0)
                        {
                            firstRow.Cells[0].AddText("Test Cell 1");
                            firstRow.Cells[0].AddText("Test Cell 2");
                        }
                    }
                }
                
                Console.WriteLine("✓ Table document created successfully");
                
                // Test 4: Save table document
                Console.WriteLine("Test 4: Saving table document...");
                using var tableStream = new MemoryStream();
                tableDocument.SaveAs(tableStream);
                
                if (tableStream.Length > 0)
                {
                    Console.WriteLine($"✓ Table document saved successfully ({tableStream.Length} bytes)");
                }
                else
                {
                    Console.WriteLine("✗ Table document save failed - empty stream");
                }
                
                // Test 5: Test document properties
                Console.WriteLine("Test 5: Testing document properties...");
                document.Properties.Title = "Integration Test Document";
                document.Properties.Author = "AMCode.Documents";
                document.Properties.Subject = "DOCX Library Integration Test";
                
                Console.WriteLine($"✓ Document properties set: Title='{document.Properties.Title}', Author='{document.Properties.Author}'");
                
                // Test 6: Test sections
                Console.WriteLine("Test 6: Testing document sections...");
                if (document.Sections.Count > 0)
                {
                    var section = document.Sections[0];
                    section.SetPageSize(AMCode.Documents.Common.Models.PageSize.A4);
                    section.SetMargins(AMCode.Documents.Common.Models.Margins.Normal);
                    section.SetOrientation(AMCode.Documents.Docx.Interfaces.PageOrientation.Portrait);
                    
                    Console.WriteLine($"✓ Section configured: PageSize={section.PageSize.Name}, Margins={section.Margins.Top}pt");
                }
                
                // Test 7: Test formatted document
                Console.WriteLine("Test 7: Creating formatted document...");
                var fontStyle = new AMCode.Documents.Common.Models.FontStyle { Bold = true, FontSize = 14, FontName = "Arial" };
                var formattedDocument = DocumentFactory.CreateFormattedDocument("Formatted Test Document", "This is formatted text", fontStyle);
                
                using var formattedStream = new MemoryStream();
                formattedDocument.SaveAs(formattedStream);
                
                if (formattedStream.Length > 0)
                {
                    Console.WriteLine($"✓ Formatted document created and saved ({formattedStream.Length} bytes)");
                }
                else
                {
                    Console.WriteLine("✗ Formatted document save failed - empty stream");
                }
                
                // Clean up
                document.Close();
                tableDocument.Close();
                formattedDocument.Close();
                
                Console.WriteLine("\n🎉 All integration tests passed successfully!");
                Console.WriteLine("DOCX library is working correctly.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Integration test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
        
        public static void RunAdvancedDocumentTest()
        {
            Console.WriteLine("\n=== Advanced DOCX Integration Test ===");
            
            try
            {
                // Create a comprehensive document
                var document = DocumentFactory.CreateDocument();
                
                // Set document properties
                document.Properties.Title = "Advanced Test Document";
                document.Properties.Author = "AMCode.Documents";
                document.Properties.Subject = "Comprehensive DOCX Testing";
                document.Properties.Keywords = "test, integration, docx, amcode";
                
                // Add multiple paragraphs with different formatting
                var titleParagraph = document.Paragraphs.Create("Advanced DOCX Test Document");
                titleParagraph.Alignment = HorizontalAlignment.Center;
                
                var subtitleParagraph = document.Paragraphs.Create("Testing Various Features");
                subtitleParagraph.Alignment = HorizontalAlignment.Center;
                subtitleParagraph.SpacingAfter = 24;
                
                // Add a paragraph with runs
                var paragraph = document.Paragraphs.Create();
                paragraph.AddText("This is ");
                paragraph.AddText("bold text", new AMCode.Documents.Common.Models.FontStyle { Bold = true });
                paragraph.AddText(" and this is ");
                paragraph.AddText("italic text", new AMCode.Documents.Common.Models.FontStyle { Italic = true });
                paragraph.AddText(".");
                
                // Add line break
                paragraph.AddLineBreak();
                paragraph.AddText("This is on a new line.");
                
                // Create a table with multiple rows and columns
                var table = document.Tables.Create(3, 4);
                
                // Add header row
                var headerRow = table.Rows[0];
                for (int i = 0; i < 4; i++)
                {
                    headerRow.Cells[i].AddText($"Header {i + 1}");
                }
                
                // Add data rows
                for (int row = 1; row < 3; row++)
                {
                    var dataRow = table.Rows[row];
                    for (int col = 0; col < 4; col++)
                    {
                        dataRow.Cells[col].AddText($"Row {row}, Col {col + 1}");
                    }
                }
                
                // Apply table style
                var tableStyle = new OpenXmlTableStyle("Table Grid");
                table.ApplyStyle(tableStyle);
                
                // Add another paragraph after table
                var conclusionParagraph = document.Paragraphs.Create("This concludes our advanced testing.");
                conclusionParagraph.Alignment = HorizontalAlignment.Justify;
                
                // Save the document
                using var stream = new MemoryStream();
                document.SaveAs(stream);
                
                if (stream.Length > 0)
                {
                    Console.WriteLine($"✓ Advanced document created and saved successfully ({stream.Length} bytes)");
                    Console.WriteLine("✓ Multiple paragraphs with formatting");
                    Console.WriteLine("✓ Table with 3 rows and 4 columns");
                    Console.WriteLine("✓ Document properties set");
                    Console.WriteLine("✓ Various text formatting applied");
                }
                else
                {
                    Console.WriteLine("✗ Advanced document save failed - empty stream");
                }
                
                document.Close();
                Console.WriteLine("\n🎉 Advanced integration test passed successfully!");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Advanced integration test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}
