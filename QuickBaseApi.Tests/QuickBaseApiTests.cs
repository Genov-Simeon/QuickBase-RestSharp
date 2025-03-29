using QuickBaseApi.Client;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Tests
{
    [TestFixture]
    public class QuickBaseApiTests
    {
        private QuickBaseClient _client;
        private QuickBaseConfig _config;
        private const string TableId = "YOUR_TABLE_ID"; // Replace with actual table ID

        [OneTimeSetUp]
        public async Task Setup()
        {
            _config = new QuickBaseConfig
            {
                Realm = "YOUR_REALM", // Replace with actual realm
                UserToken = "YOUR_USER_TOKEN", // Replace with actual user token
                AppToken = "YOUR_APP_TOKEN" // Replace with actual app token
            };

            _client = new QuickBaseClient(_config);
            await _client.AuthenticateAsync();
        }

        [Test]
        public async Task AddRecord_ValidData_ShouldSucceed()
        {
            // Arrange
            var record = new
            {
                field1 = "Test Value 1",
                field2 = "Test Value 2"
                // Add more fields based on your table structure
            };

            // Act
            var result = await _client.AddRecordAsync(TableId, record);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("success"));
        }

        [Test]
        public async Task EditRecord_ValidData_ShouldSucceed()
        {
            // Arrange
            var recordId = "RECORD_ID"; // Replace with actual record ID
            var updatedRecord = new
            {
                field1 = "Updated Value 1",
                field2 = "Updated Value 2"
                // Add more fields based on your table structure
            };

            // Act
            var result = await _client.EditRecordAsync(TableId, recordId, updatedRecord);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("success"));
        }

        [Test]
        public async Task DeleteRecord_ValidRecordId_ShouldSucceed()
        {
            // Arrange
            var recordId = "RECORD_ID"; // Replace with actual record ID

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _client.DeleteRecordAsync(TableId, recordId));
        }

        [Test]
        public async Task AddRecord_InvalidData_ShouldThrowException()
        {
            // Arrange
            var invalidRecord = new
            {
                invalidField = "Invalid Value"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => 
                await _client.AddRecordAsync(TableId, invalidRecord));
        }
    }
} 