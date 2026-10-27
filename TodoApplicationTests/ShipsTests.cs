using CleanTodo.Application.UseCase;
using CleanTodo.Application.Validators;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Entities;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Domain.Interfaces.Repositories;
using Moq;

namespace TodoApplicationTests;

public class ShipsTests
{
    [Test]
    public async Task DockShipFromMockedBroker()
    {
        // Arrange : BrokerShipDto simule une reception
        var repository = new Mock<IShipRepository>(MockBehavior.Strict);
        var incoming = new BrokerShipDto
        {
            Name = "Black Pearl",
            GoldCargo = 500000m,
            Captain = "Jack Sparrow",
            CrewSize = 25,
            CreatedBy = "TESTEUR",
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
            LastModified = DateTimeOffset.UtcNow.AddMinutes(-10),
            Status = "sailing"
        };
        Ship? testShip = null;
        repository.Setup(repo => repo.Add(It.IsAny<Ship>()))
            .Callback<Ship>(ship => testShip = ship)
            .ReturnsAsync((Ship ship) => ship);

        var dockUseCase = new DockShipUseCase(
            repository.Object, new BrokerShipValidation());

        // Act : recevoir le bateau
        var received = await dockUseCase.Execute(incoming);

        // Assert : bateau recu et mis docked
        Assert.That(testShip, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(received.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(received.Id, Is.EqualTo(testShip!.Id));
            Assert.That(received.Name, Is.EqualTo(incoming.Name));
            Assert.That(received.Captain, Is.EqualTo(incoming.Captain));
            Assert.That(received.GoldCargo, Is.EqualTo(incoming.GoldCargo));
            Assert.That(received.CrewSize, Is.EqualTo(incoming.CrewSize));
            Assert.That(received.CreatedBy, Is.EqualTo(incoming.CreatedBy));
            Assert.That(received.CreatedAt, Is.EqualTo(incoming.CreatedAt));
            Assert.That(received.Status, Is.EqualTo("docked"));
            Assert.That(testShip.Status, Is.EqualTo("docked"));
            Assert.That(received.LastModified, Is.GreaterThan(incoming.LastModified));
        });
        repository.Verify(repo => repo.Add(It.IsAny<Ship>()), Times.Once);
        repository.VerifyNoOtherCalls();
    }

    [Test]
    public async Task CreateAndSailShip_DeleteItAfterConfirmation()
    {
        // Arrange : TOut est mocked, aucune BD ou broker actually used
        var repository = new Mock<IShipRepository>(MockBehavior.Strict);
        var broker = new Mock<IBrokerGateway>(MockBehavior.Strict);
        var destination = "Tortuga";
        var creatorId = "TESTEUR";
        var createDto = new CreateShipDto("Black Pearl", 500000, "Jack Sparrow", 25);
        Ship? testShip = null;
        var deliveryConfirmed = false;

        broker.SetupGet(gateway => gateway.CreatorId).Returns(creatorId);
        repository.Setup(repo => repo.Add(It.IsAny<Ship>()))
            .ReturnsAsync((Ship ship) =>
            {
                testShip = ship;
                return ship;
            });

        var createUseCase = new CreateShipUseCase(
            repository.Object, new CreateShipValidation(), broker.Object);
        var sailUseCase = new SailShipUseCase(repository.Object, broker.Object);

        // Act : creer le bateau avec le validateur du .Applicaiton
        var created = await createUseCase.Execute(createDto);

        Assert.Multiple(() =>
        {
            Assert.That(created.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(created.Name, Is.EqualTo(createDto.Name));
            Assert.That(created.Captain, Is.EqualTo(createDto.Captain));
            Assert.That(created.GoldCargo, Is.EqualTo(createDto.GoldCargo));
            Assert.That(created.CrewSize, Is.EqualTo(createDto.CrewSize));
            Assert.That(created.CreatedBy, Is.EqualTo(creatorId));
            Assert.That(created.Status, Is.EqualTo("docked"));
        });

        var ship = testShip!;
        broker.Setup(gateway => gateway.GetPorts())
            .ReturnsAsync(new List<string> { destination });
        repository.Setup(repo => repo.BeginSail(created.Id))
            .ReturnsAsync(() =>
            {
                ship.Status = "sailing";
                ship.LastModified = DateTime.UtcNow;
                return ship;
            });
        broker.Setup(gateway => gateway.Sail(destination, It.IsAny<BrokerShipDto>()))
            .ReturnsAsync((string _, BrokerShipDto sent) =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sent.Name, Is.EqualTo(created.Name));
                    Assert.That(sent.Captain, Is.EqualTo(created.Captain));
                    Assert.That(sent.GoldCargo, Is.EqualTo(created.GoldCargo));
                    Assert.That(sent.CrewSize, Is.EqualTo(created.CrewSize));
                    Assert.That(sent.CreatedBy, Is.EqualTo(creatorId));
                    Assert.That(sent.CreatedAt, Is.EqualTo(created.CreatedAt));
                    Assert.That(sent.LastModified, Is.EqualTo(new DateTimeOffset(ship.LastModified)));
                    Assert.That(sent.Status, Is.EqualTo("sailing"));
                });
                deliveryConfirmed = true;
                return DeliveryOutcome.Confirmed;
            });
        repository.Setup(repo => repo.Delete(ship))
            .ReturnsAsync(() =>
            {
                Assert.That(deliveryConfirmed, Is.True,
                    "Le bateau doit etre delete apres la confirmation du broker comme quoi il l'a recu.");
                return ship;
            });

        // Act : Send le nouveau bateau vers le fake port
        var sentBoat = await sailUseCase.Execute(created.Id, new SailShipDto(destination));

        // Assert : valide qu'il est bien envoye et delete apres
        Assert.Multiple(() =>
        {
            Assert.That(sentBoat.Id, Is.EqualTo(created.Id));
            Assert.That(sentBoat.Status, Is.EqualTo("sailing"));
        });
        repository.Verify(repo => repo.Add(It.IsAny<Ship>()), Times.Once);
        repository.Verify(repo => repo.BeginSail(created.Id), Times.Once);
        broker.Verify(gateway => gateway.GetPorts(), Times.Once);
        broker.Verify(gateway => gateway.Sail(destination, It.IsAny<BrokerShipDto>()), Times.Once);
        repository.Verify(repo => repo.Delete(ship), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}
