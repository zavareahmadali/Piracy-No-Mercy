using UnityEngine;

public class InstantLootHandler : LootHandler
{
    public override void LootAreaEntered()
    {
        GetLooted();
    }

    public override void GetLooted()
    {
        GetComponent<Animator>().SetTrigger("Looted");
        base.GetLooted();
    }
}
