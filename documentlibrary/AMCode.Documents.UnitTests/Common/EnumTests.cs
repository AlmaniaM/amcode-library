using System;
using NUnit.Framework;
using AMCode.Documents.Common.Enums;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class EnumTests
    {
        [Test]
        public void ShouldHaveCorrectHorizontalAlignmentValues()
        {
            // Assert
            Assert.That((int)HorizontalAlignment.Left, Is.EqualTo(0));
            Assert.That((int)HorizontalAlignment.Center, Is.EqualTo(1));
            Assert.That((int)HorizontalAlignment.Right, Is.EqualTo(2));
            Assert.That((int)HorizontalAlignment.Justify, Is.EqualTo(3));
        }

        [Test]
        public void ShouldHaveCorrectBorderLineStyleValues()
        {
            // Assert
            Assert.That((int)BorderLineStyle.None, Is.EqualTo(0));
            Assert.That((int)BorderLineStyle.Single, Is.EqualTo(1));
            Assert.That((int)BorderLineStyle.Double, Is.EqualTo(2));
            Assert.That((int)BorderLineStyle.Dashed, Is.EqualTo(3));
            Assert.That((int)BorderLineStyle.Dotted, Is.EqualTo(4));
            Assert.That((int)BorderLineStyle.Thick, Is.EqualTo(5));
            Assert.That((int)BorderLineStyle.Medium, Is.EqualTo(6));
            Assert.That((int)BorderLineStyle.Thin, Is.EqualTo(7));
        }

        [Test]
        public void ShouldHaveCorrectBorderSidesValues()
        {
            // Assert
            Assert.That((int)BorderSides.None, Is.EqualTo(0));
            Assert.That((int)BorderSides.Top, Is.EqualTo(1));
            Assert.That((int)BorderSides.Bottom, Is.EqualTo(2));
            Assert.That((int)BorderSides.Left, Is.EqualTo(4));
            Assert.That((int)BorderSides.Right, Is.EqualTo(8));
            Assert.That((int)BorderSides.All, Is.EqualTo(15)); // 1 + 2 + 4 + 8
        }

        [Test]
        public void ShouldConvertHorizontalAlignmentToString()
        {
            // Assert
            Assert.That(HorizontalAlignment.Left.ToString(), Is.EqualTo("Left"));
            Assert.That(HorizontalAlignment.Center.ToString(), Is.EqualTo("Center"));
            Assert.That(HorizontalAlignment.Right.ToString(), Is.EqualTo("Right"));
            Assert.That(HorizontalAlignment.Justify.ToString(), Is.EqualTo("Justify"));
        }

        [Test]
        public void ShouldConvertBorderLineStyleToString()
        {
            // Assert
            Assert.That(BorderLineStyle.None.ToString(), Is.EqualTo("None"));
            Assert.That(BorderLineStyle.Single.ToString(), Is.EqualTo("Single"));
            Assert.That(BorderLineStyle.Double.ToString(), Is.EqualTo("Double"));
            Assert.That(BorderLineStyle.Dashed.ToString(), Is.EqualTo("Dashed"));
            Assert.That(BorderLineStyle.Dotted.ToString(), Is.EqualTo("Dotted"));
            Assert.That(BorderLineStyle.Thick.ToString(), Is.EqualTo("Thick"));
            Assert.That(BorderLineStyle.Medium.ToString(), Is.EqualTo("Medium"));
            Assert.That(BorderLineStyle.Thin.ToString(), Is.EqualTo("Thin"));
        }

        [Test]
        public void ShouldConvertBorderSidesToString()
        {
            // Assert
            Assert.That(BorderSides.None.ToString(), Is.EqualTo("None"));
            Assert.That(BorderSides.Top.ToString(), Is.EqualTo("Top"));
            Assert.That(BorderSides.Bottom.ToString(), Is.EqualTo("Bottom"));
            Assert.That(BorderSides.Left.ToString(), Is.EqualTo("Left"));
            Assert.That(BorderSides.Right.ToString(), Is.EqualTo("Right"));
            Assert.That(BorderSides.All.ToString(), Is.EqualTo("All"));
        }

        [Test]
        public void ShouldParseHorizontalAlignmentFromString()
        {
            // Act & Assert
            Assert.That(Enum.Parse<HorizontalAlignment>("Left"), Is.EqualTo(HorizontalAlignment.Left));
            Assert.That(Enum.Parse<HorizontalAlignment>("Center"), Is.EqualTo(HorizontalAlignment.Center));
            Assert.That(Enum.Parse<HorizontalAlignment>("Right"), Is.EqualTo(HorizontalAlignment.Right));
            Assert.That(Enum.Parse<HorizontalAlignment>("Justify"), Is.EqualTo(HorizontalAlignment.Justify));
        }

        [Test]
        public void ShouldParseBorderLineStyleFromString()
        {
            // Act & Assert
            Assert.That(Enum.Parse<BorderLineStyle>("None"), Is.EqualTo(BorderLineStyle.None));
            Assert.That(Enum.Parse<BorderLineStyle>("Single"), Is.EqualTo(BorderLineStyle.Single));
            Assert.That(Enum.Parse<BorderLineStyle>("Double"), Is.EqualTo(BorderLineStyle.Double));
            Assert.That(Enum.Parse<BorderLineStyle>("Dashed"), Is.EqualTo(BorderLineStyle.Dashed));
            Assert.That(Enum.Parse<BorderLineStyle>("Dotted"), Is.EqualTo(BorderLineStyle.Dotted));
            Assert.That(Enum.Parse<BorderLineStyle>("Thick"), Is.EqualTo(BorderLineStyle.Thick));
            Assert.That(Enum.Parse<BorderLineStyle>("Medium"), Is.EqualTo(BorderLineStyle.Medium));
            Assert.That(Enum.Parse<BorderLineStyle>("Thin"), Is.EqualTo(BorderLineStyle.Thin));
        }

        [Test]
        public void ShouldParseBorderSidesFromString()
        {
            // Act & Assert
            Assert.That(Enum.Parse<BorderSides>("None"), Is.EqualTo(BorderSides.None));
            Assert.That(Enum.Parse<BorderSides>("Top"), Is.EqualTo(BorderSides.Top));
            Assert.That(Enum.Parse<BorderSides>("Bottom"), Is.EqualTo(BorderSides.Bottom));
            Assert.That(Enum.Parse<BorderSides>("Left"), Is.EqualTo(BorderSides.Left));
            Assert.That(Enum.Parse<BorderSides>("Right"), Is.EqualTo(BorderSides.Right));
            Assert.That(Enum.Parse<BorderSides>("All"), Is.EqualTo(BorderSides.All));
        }
    }
}
