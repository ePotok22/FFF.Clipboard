
using FFF.Interfaces;
using Moq;
using NUnit.Framework;

namespace FFF.Clipboard.Test
{
    // TestFixture attribute denotes a class that contains unit tests
    [TestFixture]
    public class FileDirectoryTests
    {
        // This field will hold our mock object of the IFileDirectory interface
        private Mock<IFileDirectory> _mockFileDirectory;

        // SetUp attribute marks the method to run before each test method
        [SetUp]
        public void Setup()
        {
            // Initialize the mock object before each test
            _mockFileDirectory = new Mock<IFileDirectory>();
        }

        // Use TearDown to define cleanup logic after each test
        [TearDown]
        public void TearDown()
        {
            // Perform any cleanup or state resetting here
            _mockFileDirectory.VerifyAll();
        }

        // Test attribute denotes a unit test method
        [Test]
        public void Test_FileExists_ReturnsTrue()
        {
            // Arrange: Prepare the test scenario
            string filePath = "existing_file.txt";
            // Set up the mock to return true when IsExists is called with a specific file path
            _mockFileDirectory.Setup(x => x.IsExists(filePath)).Returns(true);

            // Act: Perform the actual work of the test
            bool result = _mockFileDirectory.Object.IsExists(filePath);

            // Assert: Verify the result of the test
            Assert.That(result, Is.True);

        }

        // Another test case to test a different scenario
        [Test]
        public void Test_FileDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string filePath = "non_existing_file.txt";
            // Set up the mock to return false for a different file path
            _mockFileDirectory.Setup(x => x.IsExists(filePath)).Returns(false);

            // Act
            bool result = _mockFileDirectory.Object.IsExists(filePath);

            // Assert
            Assert.That(result, Is.False);
        }
    }

}
