namespace GameLibrary;

internal class TreasureChest
{
    public TreasureChest(bool isLocked)
    {
        IsLocked = isLocked;
    }

    public bool IsLocked { get; set; }

    public bool CanOpen(bool hasKey)
    {
        // if (!IsLocked)
        //     return true;

        // if (hasKey)
        // {
        //     return true;
        // }

        // return false;
        return !IsLocked || hasKey;
    }
}
