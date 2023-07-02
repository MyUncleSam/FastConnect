using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FastConnect.Test
{
    [TestFixture]
    public class DbCreation
    {
        [Test]
        public async Task CreateDatabase_When_DatabaseIsMissing_CreateNewDatabase()
        {
            // Arrange
            var randomName = $"{Guid.NewGuid().ToString("N")}.sqlite";

            // Act
            var dbCreated = Repositories.Database.Create(randomName);

            // Assert
            dbCreated.Should().BeTrue();
            File.Exists(randomName).Should().BeTrue();

            // Cleanup
            File.Delete(randomName);
        }

        [Test]
        public async Task CreateDatabase_When_DatabaseIsExisting_DoNothing()
        {
            // Arrange
            var randomName = $"{Guid.NewGuid().ToString("N")}.sqlite";
            File.WriteAllText(randomName, "TEST");

            // Act
            var dbCreated = Repositories.Database.Create(randomName);

            // Assert
            dbCreated.Should().BeFalse();

            // Cleanup
            File.Delete(randomName);
        }
    }
}
