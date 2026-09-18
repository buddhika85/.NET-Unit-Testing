namespace GameLibrary;

public class QuestRewardCalculator
{
    private readonly double rewardMultiplier;

    public QuestRewardCalculator(double rewardMultiplier = 1.0)
    {
        this.rewardMultiplier = rewardMultiplier;
    }

    public int CalculateQuestReward(int questDifficulty)
    {
        return (int)(questDifficulty * 100 * rewardMultiplier);
    }
}