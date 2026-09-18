namespace GameLibrary;

public class QuestGenerator
{
    private readonly QuestRewardCalculator questRewardCalculator;

    public QuestGenerator(QuestRewardCalculator questRewardCalculator) =>
        this.questRewardCalculator = questRewardCalculator;


    public Quest GenerateQuest(int questDifficulty)
    {
        int reward = questRewardCalculator.CalculateQuestReward(questDifficulty);
        string name = $"Quest Level {questDifficulty}";
        return new Quest(name, reward);
    }
}