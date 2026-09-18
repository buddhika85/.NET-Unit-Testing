using FluentAssertions;
using Moq;

namespace GameLibrary.UnitTests;

public class QuestMasterTests
{
    // [Fact]
    // public void AssignQuest_WhenCalled_ReturnsNewQuestWithPlayerNameAndNotfiesPlayer()
    // {
    //     // arrange
    //     var playerContact = new PlayerContact("Alice", "alice@gmail.com");
    //     var questDifficulty = 1;
    //     var questGeneratorStub = new Mock<IQuestGenerator>();

    //     var quest = new Quest("Reach Mountain", 10);
    //     questGeneratorStub.Setup(x => x.GenerateQuest(questDifficulty)).Returns(quest);

    //     var notificationServiceMock = new Mock<INotificationService>();
    //     var expectedNotificationMessage = $"You have been assigned a new quest: {quest.Name}! Complete it to earn {quest.Reward} points.";
    //     var sut = new QuestMaster(questGeneratorStub.Object, notificationServiceMock.Object);


    //     // act
    //     var result = sut.AssignQuest(playerContact, questDifficulty);

    //     // assert
    //     result.Should().NotBeNull();
    //     result.Name.Should().Be("Reach Mountain");
    //     result.Reward.Should().Be(10);
    //     result.PlayerName.Should().Be("Alice");
    //     result.Should().BeSameAs(quest);                    // same object reference
    //     questGeneratorStub.Verify(x => x.GenerateQuest(questDifficulty), Times.Once);
    //     notificationServiceMock.Verify(x => x.NotifyPlayer(playerContact, expectedNotificationMessage), Times.Once);
    // }

    [Fact]
    public void AssignQuest_WhenCalled_ReturnsNewQuestWithPlayerNameAndNotfiesPlayer()
    {
        // arrange
        var playerContact = new PlayerContact("Alice", "alice@example.com");
        var questDifficulty = 1;
        var questGenerator = new QuestGenerator(new QuestRewardCalculator());
        var expectedMessageSections = new[] {
            "You have been assigned a new quest: ",
            "! Complete it to earn ",
            " points." };

        var notificationServiceMock = new Mock<INotificationService>();
        var sut = new QuestMaster(questGenerator, notificationServiceMock.Object);


        // act
        var result = sut.AssignQuest(playerContact, questDifficulty);

        // assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Quest>();
        result.PlayerName.Should().Be("Alice");
        result.Reward.Should().BePositive();

        notificationServiceMock.Verify(x => x.NotifyPlayer(playerContact,
            It.Is<string>(x => expectedMessageSections.All(section => x.Contains(section)))),
            Times.Once);
    }
}
