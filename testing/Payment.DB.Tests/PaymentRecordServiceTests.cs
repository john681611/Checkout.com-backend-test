using Payment.DB.Models;
using MongoDB.Driver;
using Moq;
using MongoDB.Driver.Core.Operations;

namespace Payment.DB.Services.Tests;

public class PaymentRecordServiceTests: IDisposable
{
    public void Dispose()
    {
        Environment.SetEnvironmentVariable("MONGO_CONNECTION", null);
        Environment.SetEnvironmentVariable("MONGO_DB", null);
    }

    [Fact]
    public void Instantiation_ConnectionStringArgument_ThrowsArgumentNullException()
    {
        var act = () => new PaymentRecordService();

        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'Environment Variable: MONGO_CONNECTION')", exception.Message);
    }

    [Fact]
    public void Instantiation_BadConnectionArgument_ThrowsArgumentNullException()
    {
        Environment.SetEnvironmentVariable("MONGO_CONNECTION", "https://Raining.com");
        var act = () => new PaymentRecordService();

        var exception = Assert.Throws<MongoDB.Driver.MongoConfigurationException>(act);
        Assert.Equal("The connection string 'https://Raining.com' is not valid.", exception.Message);
    }

    [Fact]
    public void Instantiation_DatabaseArgument_ThrowsArgumentNullException()
    {
        Environment.SetEnvironmentVariable("MONGO_CONNECTION", "mongodb://admin:admin@localhost:27017/");
        var act = () => new PaymentRecordService();

        var exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'Environment Variable: MONGO_DB')", exception.Message);
    }

    [Fact]
    public async Task GetAsync_NotFound_ReturnsNull()
    {
        SetupEnviromentVariables();
        var clientMock = new Mock<IMongoClient>();
        var collectionMock = new Mock<IMongoCollection<PaymentRecord>>();
        var dbMock = new Mock<IMongoDatabase>();
        var cursorMock = new Mock<IAsyncCursor<PaymentRecord>>();

        clientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(dbMock.Object);
        dbMock.Setup(x => x.GetCollection<PaymentRecord>(It.IsAny<string>(), null)).Returns(collectionMock.Object);

        collectionMock.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<PaymentRecord>>(), It.IsAny<FindOptions<PaymentRecord, PaymentRecord>>(), It.IsAny<CancellationToken>())).ReturnsAsync(cursorMock.Object);

        var service = new PaymentRecordService(clientMock.Object);

        var response = await service.GetAsync(Guid.NewGuid(), "ACME");

        Assert.Null(response);
    }

    [Fact]
    public async Task GetAsync_Found_ReturnsRecord()
    {
        SetupEnviromentVariables();
        var clientMock = new Mock<IMongoClient>();
        var collectionMock = new Mock<IMongoCollection<PaymentRecord>>();
        var dbMock = new Mock<IMongoDatabase>();
        var cursorMock = new Mock<IAsyncCursor<PaymentRecord>>();

        clientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), null)).Returns(dbMock.Object);
        dbMock.Setup(x => x.GetCollection<PaymentRecord>(It.IsAny<string>(), null)).Returns(collectionMock.Object);

        collectionMock.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<PaymentRecord>>(), It.IsAny<FindOptions<PaymentRecord, PaymentRecord>>(), It.IsAny<CancellationToken>())).ReturnsAsync(cursorMock.Object);
        
        cursorMock.Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        cursorMock.Setup(x => x.Current).Returns(new List<PaymentRecord>{new PaymentRecord{
            MerchantID = "1234"
        }}); 

        var service = new PaymentRecordService(clientMock.Object);

        var response = await service.GetAsync(Guid.NewGuid(), "ACME");

        Assert.Equal("1234", response?.MerchantID);
    }

    private void SetupEnviromentVariables()
    {
        Environment.SetEnvironmentVariable("MONGO_CONNECTION", "mongodb://admin:admin@localhost:27017/");
        Environment.SetEnvironmentVariable("MONGO_DB", "Bob");
    }
}