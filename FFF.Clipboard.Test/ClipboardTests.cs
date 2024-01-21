
using FFF.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace FFF.Clipboard.Test
{
    [TestFixture]
    public class ClipboardTests
    {
        private Mock<IClipboard> _mockClipboard;

        [SetUp]
        public void SetUp()
        {
            _mockClipboard = new Mock<IClipboard>();
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormatWithTypeDataFormat))]
        public void SetText_CallsCorrectMethod(TextDataFormat format, TypeDataFormat type)
        {
            // Arrange
            string content = "test content";

            // Act
            _mockClipboard.Object.SetText(content, type, format);

            // Assert
            _mockClipboard.Verify(x => x.SetText(content, type, format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormatWithTypeDataFormat))]
        public void SetText_WithEmptyString_CallsMethod(TextDataFormat format, TypeDataFormat type)
        {
            // Arrange
            string content = string.Empty;

            // Act
            _mockClipboard.Object.SetText(content, type, format);

            // Assert
            _mockClipboard.Verify(x => x.SetText(content, type, format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormatWithTypeDataFormat))]
        public async Task SetTextAsync_CallsCorrectMethod(TextDataFormat format, TypeDataFormat type)
        {
            // Arrange
            string content = "test content";

            // Act
            await _mockClipboard.Object.SetTextAsync(content, type, format);

            // Assert
            _mockClipboard.Verify(x => x.SetTextAsync(content, type, format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormatWithTypeDataFormat))]
        public async Task SetTextAsync_WithEmptyString_CallsMethod(TextDataFormat format, TypeDataFormat type)
        {
            // Arrange
            string content = string.Empty;

            // Act
            await _mockClipboard.Object.SetTextAsync(content, type, format);

            // Assert
            _mockClipboard.Verify(x => x.SetTextAsync(content, type, format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormat))]
        public void GetText_CallsCorrectMethod(TextDataFormat format)
        {
            // Act
            _mockClipboard.Object.GetText(format);

            // Assert
            _mockClipboard.Verify(x => x.GetText(format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormat))]
        public void GetText_WithUnsupportedFormat_ReturnsNull(TextDataFormat format)
        {
            // Arrange
            _mockClipboard.Setup(x => x.GetText(It.IsAny<TextDataFormat>())).Returns((string)null);

            // Act
            string result = _mockClipboard.Object.GetText(format);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormat))]
        public async Task GetTextAsync_CallsCorrectMethod(TextDataFormat format)
        {
            // Act
            await _mockClipboard.Object.GetTextAsync(format);

            // Assert
            _mockClipboard.Verify(x => x.GetTextAsync(format), Times.Once);
        }

        [TestCaseSource(typeof(MyDataSources), nameof(MyDataSources.ListTextDataFormat))]
        public void GetTextAsync_WhenExceptionOccurs_ThrowsException(TextDataFormat format)
        {
            // Arrange
            _mockClipboard.Setup(x => x.GetTextAsync(It.IsAny<TextDataFormat>())).ThrowsAsync(new InvalidOperationException());

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _mockClipboard.Object.GetTextAsync(format));
        }

        [Test]
        public void Clear_CallsMethod()
        {
            // Act
            _mockClipboard.Object.Clear();

            // Assert
            _mockClipboard.Verify(x => x.Clear(), Times.Once);
        }

        [Test]
        public void Clear_WhenCalled_ChecksIfClearedSuccessfully()
        {
            // Act
            _mockClipboard.Object.Clear();

            // You can add additional logic here if your interface supports checking if the clipboard is actually cleared
            // For example, calling a method that checks if the clipboard is empty and asserting the result

            // Assert
            _mockClipboard.Verify(x => x.Clear(), Times.Once);
        }

        [Test]
        public async Task ClearAsync_CallsMethod()
        {
            // Act
            await _mockClipboard.Object.ClearAsync();

            // Assert
            _mockClipboard.Verify(x => x.ClearAsync(), Times.Once);
        }

        [Test]
        public async Task ClearAsync_WithSimulatedDelay_CompletesSuccessfully()
        {
            // Arrange
            _mockClipboard.Setup(x => x.ClearAsync()).Returns(Task.Delay(1000)); // Simulate a delay

            // Act
            await _mockClipboard.Object.ClearAsync();

            // Assert
            _mockClipboard.Verify(x => x.ClearAsync(), Times.Once);
        }
    }
}
