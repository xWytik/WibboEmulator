namespace WibboEmulator.Games.Items.Wired.Triggers;
using System.Data;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;
using WibboEmulator.Games.Rooms.Events;

public class Collision : WiredTriggerBase, IWired
{
    public Collision(Item item, Room room) : base(item, room, (int)WiredTriggerType.COLLISION) => this.RoomInstance.WiredHandler.TrgCollision += this.OnFurniCollision;

    private void OnFurniCollision(object obj, ItemTriggeredEventArgs args)
    {
        if (args.User == null)
        {
            return;
        }

        this.RoomInstance.WiredHandler.ExecutePile(this.ItemInstance.Coordinate, args.User, args.Item);
    }

    public override void Dispose()
    {
        base.Dispose();

        this.RoomInstance.WiredHandler.TrgCollision -= this.OnFurniCollision;
    }

    public void SaveToDatabase(IQueryAdapter dbClient)
    {
    }

    public void LoadFromDatabase(DataRow row)
    {
    }
}
