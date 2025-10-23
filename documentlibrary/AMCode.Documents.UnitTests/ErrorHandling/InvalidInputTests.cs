using System;
using System.IO;
using NUnit.Framework;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Enums;
using AMCode.Docx;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.UnitTests.ErrorHandling
{
    /// <summary>
    /// Tests for invalid input handling across the document library
    /// </summary>
    [TestFixture]
    public class InvalidInputTests
    {
        #region Color Invalid Input Tests

        [Test]
        public void Color_FromArgb_WithNegativeValues_ShouldHandleGracefully()
        {
            // Arrange & Act
            var color = Color.FromArgb(-1);
            
            // Assert - Should not throw, but may have unexpected values
            Assert.That(color, Is.Not.Null);
            // The bit shifting will produce unexpected results with negative values
            Assert.That(color.A, Is.GreaterThanOrEqualTo(0));
            Assert.That(color.R, Is.GreaterThanOrEqualTo(0));
            Assert.That(color.G, Is.GreaterThanOrEqualTo(0));
            Assert.That(color.B, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Color_FromArgb_WithVeryLargeValues_ShouldHandleGracefully()
        {
            // Arrange & Act
            var color = Color.FromArgb(int.MaxValue);
            
            // Assert - Should not throw, but values should be clamped to byte range
            Assert.That(color, Is.Not.Null);
            Assert.That(color.A, Is.LessThanOrEqualTo(255));
            Assert.That(color.R, Is.LessThanOrEqualTo(255));
            Assert.That(color.G, Is.LessThanOrEqualTo(255));
            Assert.That(color.B, Is.LessThanOrEqualTo(255));
        }

        #endregion

        #region FontStyle Invalid Input Tests

        [Test]
        public void FontStyle_WithNullFontName_ShouldUseDefault()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.FontName = null;
            
            // Assert
            Assert.That(fontStyle.FontName, Is.EqualTo("Calibri")); // Should use default
        }

        [Test]
        public void FontStyle_WithEmptyFontName_ShouldUseDefault()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.FontName = "";
            
            // Assert
            Assert.That(fontStyle.FontName, Is.EqualTo("Calibri")); // Should use default
        }

        [Test]
        public void FontStyle_WithNegativeFontSize_ShouldHandleGracefully()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.FontSize = -10.0;
            
            // Assert - Should not throw, but may cause issues in document generation
            Assert.That(fontStyle.FontSize, Is.EqualTo(-10.0));
        }

        [Test]
        public void FontStyle_WithZeroFontSize_ShouldHandleGracefully()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.FontSize = 0.0;
            
            // Assert
            Assert.That(fontStyle.FontSize, Is.EqualTo(0.0));
        }

        [Test]
        public void FontStyle_WithVeryLargeFontSize_ShouldHandleGracefully()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.FontSize = double.MaxValue;
            
            // Assert
            Assert.That(fontStyle.FontSize, Is.EqualTo(double.MaxValue));
        }

        [Test]
        public void FontStyle_WithNullColor_ShouldUseDefault()
        {
            // Arrange
            var fontStyle = new AMCode.Documents.Common.Models.FontStyle();
            
            // Act
            fontStyle.Color = null;
            
            // Assert
            Assert.That(fontStyle.Color, Is.EqualTo(Color.Black)); // Should use default
        }

        #endregion

        #region PageSize Invalid Input Tests

        [Test]
        public void PageSize_WithNegativeWidth_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Width = -100.0;
            
            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(-100.0));
            Assert.That(pageSize.WidthInPoints, Is.EqualTo(-100.0));
        }

        [Test]
        public void PageSize_WithNegativeHeight_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Height = -100.0;
            
            // Assert
            Assert.That(pageSize.Height, Is.EqualTo(-100.0));
            Assert.That(pageSize.HeightInPoints, Is.EqualTo(-100.0));
        }

        [Test]
        public void PageSize_WithZeroDimensions_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Width = 0.0;
            pageSize.Height = 0.0;
            
            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(0.0));
            Assert.That(pageSize.Height, Is.EqualTo(0.0));
        }

        [Test]
        public void PageSize_WithVeryLargeDimensions_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Width = double.MaxValue;
            pageSize.Height = double.MaxValue;
            
            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(double.MaxValue));
            Assert.That(pageSize.Height, Is.EqualTo(double.MaxValue));
        }

        [Test]
        public void PageSize_WithNullName_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Name = null;
            
            // Assert
            Assert.That(pageSize.Name, Is.Null);
        }

        [Test]
        public void PageSize_WithEmptyName_ShouldHandleGracefully()
        {
            // Arrange
            var pageSize = new AMCode.Documents.Common.Models.PageSize();
            
            // Act
            pageSize.Name = "";
            
            // Assert
            Assert.That(pageSize.Name, Is.EqualTo(""));
        }

        #endregion

        #region Margins Invalid Input Tests

        [Test]
        public void Margins_WithNegativeValues_ShouldHandleGracefully()
        {
            // Arrange
            var margins = new AMCode.Documents.Common.Models.Margins();
            
            // Act
            margins.Top = -10.0;
            margins.Bottom = -20.0;
            margins.Left = -30.0;
            margins.Right = -40.0;
            
            // Assert
            Assert.That(margins.Top, Is.EqualTo(-10.0));
            Assert.That(margins.Bottom, Is.EqualTo(-20.0));
            Assert.That(margins.Left, Is.EqualTo(-30.0));
            Assert.That(margins.Right, Is.EqualTo(-40.0));
        }

        [Test]
        public void Margins_WithVeryLargeValues_ShouldHandleGracefully()
        {
            // Arrange
            var margins = new AMCode.Documents.Common.Models.Margins();
            
            // Act
            margins.Top = double.MaxValue;
            margins.Bottom = double.MaxValue;
            margins.Left = double.MaxValue;
            margins.Right = double.MaxValue;
            
            // Assert
            Assert.That(margins.Top, Is.EqualTo(double.MaxValue));
            Assert.That(margins.Bottom, Is.EqualTo(double.MaxValue));
            Assert.That(margins.Left, Is.EqualTo(double.MaxValue));
            Assert.That(margins.Right, Is.EqualTo(double.MaxValue));
        }

        #endregion

        #region BorderStyle Invalid Input Tests

        [Test]
        public void BorderStyle_WithNullColor_ShouldHandleGracefully()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            
            // Act
            borderStyle.Color = null;
            
            // Assert
            Assert.That(borderStyle.Color, Is.Null);
        }

        [Test]
        public void BorderStyle_WithInvalidLineStyle_ShouldHandleGracefully()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            
            // Act
            borderStyle.LineStyle = (BorderLineStyle)999; // Invalid enum value
            
            // Assert
            Assert.That(borderStyle.LineStyle, Is.EqualTo((BorderLineStyle)999));
        }

        [Test]
        public void BorderStyle_WithInvalidSides_ShouldHandleGracefully()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            
            // Act
            borderStyle.Sides = (BorderSides)999; // Invalid enum value
            
            // Assert
            Assert.That(borderStyle.Sides, Is.EqualTo((BorderSides)999));
        }

        #endregion

        #region Document Factory Invalid Input Tests

        [Test]
        public void DocumentFactory_CreateDocumentWithTable_WithNegativeRows_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                DocumentFactory.CreateDocumentWithTable("Test", -1, 5);
            });
        }

        [Test]
        public void DocumentFactory_CreateDocumentWithTable_WithNegativeColumns_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                DocumentFactory.CreateDocumentWithTable("Test", 5, -1);
            });
        }

        [Test]
        public void DocumentFactory_CreateDocumentWithTable_WithZeroRows_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                DocumentFactory.CreateDocumentWithTable("Test", 0, 5);
            });
        }

        [Test]
        public void DocumentFactory_CreateDocumentWithTable_WithZeroColumns_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                DocumentFactory.CreateDocumentWithTable("Test", 5, 0);
            });
        }

        [Test]
        public void DocumentFactory_CreateFormattedDocument_WithNullFontStyle_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                DocumentFactory.CreateFormattedDocument("Test", "Content", null);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithNullStream_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                DocumentFactory.OpenDocument((Stream)null);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithNullFilePath_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                DocumentFactory.OpenDocument((string)null);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithEmptyFilePath_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                DocumentFactory.OpenDocument("");
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithWhitespaceFilePath_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                DocumentFactory.OpenDocument("   ");
            });
        }

        #endregion

        #region PDF Factory Invalid Input Tests

        [Test]
        public void PdfFactory_RegisterProvider_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                PdfFactory.RegisterProvider(null, new MockPdfProvider());
            });
        }

        [Test]
        public void PdfFactory_RegisterProvider_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                PdfFactory.RegisterProvider("", new MockPdfProvider());
            });
        }

        [Test]
        public void PdfFactory_RegisterProvider_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                PdfFactory.RegisterProvider("   ", new MockPdfProvider());
            });
        }

        [Test]
        public void PdfFactory_RegisterProvider_WithNullProvider_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                PdfFactory.RegisterProvider("Test", null);
            });
        }

        [Test]
        public void PdfFactory_SetDefaultProvider_WithNullProvider_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                PdfFactory.SetDefaultProvider(null);
            });
        }

        [Test]
        public void PdfFactory_CreateDocument_WithNonExistentProvider_ShouldReturnFailure()
        {
            // Arrange & Act
            var result = PdfFactory.CreateDocument("NonExistentProvider");
            
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("not found"));
        }

        [Test]
        public void PdfFactory_CreateBuilder_WithNonExistentProvider_ShouldThrowInvalidOperationException()
        {
            // Arrange & Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                PdfFactory.CreateBuilder("NonExistentProvider");
            });
        }

        [Test]
        public void PdfFactory_OpenDocument_WithNullStream_ShouldReturnFailure()
        {
            // Arrange & Act
            var result = PdfFactory.OpenDocument((Stream)null);
            
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithNullFilePath_ShouldReturnFailure()
        {
            // Arrange & Act
            var result = PdfFactory.OpenDocument((string)null);
            
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithEmptyFilePath_ShouldReturnFailure()
        {
            // Arrange & Act
            var result = PdfFactory.OpenDocument("");
            
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        #endregion

        #region Helper Classes

        private class MockPdfProvider : IPdfProvider
        {
            public string Name => "MockPdfProvider";
            public string Version => "1.0.0";

            public Result<IPdfDocument> CreateDocument()
            {
                throw new NotImplementedException();
            }

            public Result<IPdfDocument> CreateDocument(IPdfProperties properties)
            {
                throw new NotImplementedException();
            }

            public Result<IPdfDocument> OpenDocument(Stream stream)
            {
                throw new NotImplementedException();
            }

            public Result<IPdfDocument> OpenDocument(string filePath)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
