namespace WibboEmulator.Games.Items.Wired.Actions;
using System.Data;
using WibboEmulator.Communication.Packets.Outgoing.Rooms.Engine;
using WibboEmulator.Database.Interfaces;
using WibboEmulator.Games.Chats.Commands.User.Several;
using WibboEmulator.Games.Items.Wired.Bases;
using WibboEmulator.Games.Items.Wired.Interfaces;
using WibboEmulator.Games.Rooms;

public class Tridimension : WiredActionBase, IWiredEffect, IWired
{
    public Tridimension(Item item, Room room) : base(item, room, (int)WiredActionType.TRI_DIMENSION) => this.StringParam = "0;0;0.0";

    public override bool OnCycle(RoomUser user, Item item)
    {
        foreach (var roomItem in this.Items.ToList())
        {
            this.HandleMovement(roomItem);
        }

        return false;
    }

    private void HandleMovement(Item item)
    {
        if (this.RoomInstance.RoomItemHandling.GetItem(item.Id) == null)
        {
            return;
        }

        var parts = this.StringParam.Split(';');

        var x = 0;
        var y = 0;
        var z = 0.0;

        if (parts.Length == 3)
        {
            _ = int.TryParse(parts[0], out x);
            _ = int.TryParse(parts[1], out y);
            _ = double.TryParse(parts[2], out z);
        }

        var newX = item.X + x;
        var newY = item.Y + y;
        var newZ = item.Z + z;

        if (newX > this.RoomInstance.GameMap.Model.MapSizeX)
        {
            newX = this.RoomInstance.GameMap.Model.MapSizeX - 1;
        }

        if (newY > this.RoomInstance.GameMap.Model.MapSizeY)
        {
            newY = this.RoomInstance.GameMap.Model.MapSizeY - 1;
        }

        if (newZ > 1000)
        {
            newZ = 1000;
        }

        if (newX < 0)
        {
            newX = 0;
        }

        if (newY < 0)
        {
            newY = 0;
        }

        if (newZ < -1000)
        {
            newZ = -1000;
        }

        if (newX != item.X || newY != item.Y || newZ != item.Z)
        {
            if (this.RoomInstance.GameMap.ValidTile(newX, newY, newZ))
            {
                this.RoomInstance.RoomItemHandling.PositionReset(item, newX, newY, newZ);
            }
        }

        return;
    }

    public void SaveToDatabase(IQueryAdapter dbClient) => WiredUtillity.SaveTriggerItem(dbClient, this.Id, string.Empty, this.StringParam, false, this.Items, this.Delay);

    public void LoadFromDatabase(DataRow row)
    {
        this.IntParams.Clear();

        if (int.TryParse(row["delay"].ToString(), out var delay))
        {
            this.Delay = delay;
        }

        this.StringParam = row["trigger_data"].ToString();

        var triggerItems = row["triggers_item"].ToString();
        if (triggerItems is null or "")
        {
            return;
        }

        foreach (var itemId in triggerItems.Split(';'))
        {
            if (!int.TryParse(itemId, out var id))
            {
                continue;
            }

            if (!this.StuffIds.Contains(id))
            {
                this.StuffIds.Add(id);
            }
        }
    }
}