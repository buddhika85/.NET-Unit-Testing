using FluentAssertions;

namespace GameLibrary.UnitTests;

public class TreasureChestTests
{
    // MethodName_StateUnderTest_ExpectedBehaviour
    // sut = system under test

    [Theory]
    [InlineData(true, true, true, "Locked chest with key can be opened")]
    [InlineData(true, false, false, "Locked chest without key cannot be opened")]
    [InlineData(false, true, true, "Unlocked chest with key can be opened")]
    [InlineData(false, false, true, "Unlocked chest without key can be opened")]
    public void CanOpen_WhenCalled_ExpectedOutcome(
       bool isLocked,
       bool hasKey,
       bool canOpenExpected,
       string because)
    {
        // arrange
        var sut = new TreasureChest(isLocked);
        // act
        var result = sut.CanOpen(hasKey);
        // assert
        result.Should().Be(canOpenExpected, because);
    }



    [Fact(Skip = "CanOpen_WhenCalled_ExpectedOutcome already covers this flow")]
    public void CanOpen_ChestIsLockedAndHasKey_ReturnsTrue()
    {
        // arrange
        var sut = new TreasureChest(isLocked: true);

        // act
        var result = sut.CanOpen(hasKey: true);

        // assert
        result.Should().BeTrue("a locked chest should be openable when user has the key");
    }


    [Fact(Skip = "CanOpen_WhenCalled_ExpectedOutcome already covers this flow")]
    public void CanOpen_ChestIsLockedAndHasNoKey_ReturnsFalse()
    {
        // arrange 
        var sut = new TreasureChest(isLocked: true);

        // act
        var result = sut.CanOpen(hasKey: false);

        // assert
        result.Should().BeFalse("a locked chest with no key is not openable");
    }

    [Fact(Skip = "CanOpen_WhenCalled_ExpectedOutcome already covers this flow")]
    public void CanOpen_ChestIsOpenAndHasKey_ReturnsTrue()
    {
        // arrange 
        var sut = new TreasureChest(isLocked: false);
        // act
        var result = sut.CanOpen(hasKey: true);
        // assert
        result.Should().BeTrue("an unlockled chest with key is openable");
    }

    [Fact(Skip = "CanOpen_WhenCalled_ExpectedOutcome already covers this flow")]
    public void CanOpen_ChestIsOpenAndHasNoKey_ReturnsTrue()
    {
        // arrange 
        var sut = new TreasureChest(isLocked: false);
        // act
        var result = sut.CanOpen(hasKey: false);
        // assert
        result.Should().BeTrue("an unlockled chest with no key is still openable");
    }


}
