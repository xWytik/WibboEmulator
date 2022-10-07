namespace WibboEmulator.Games.Chat.Commands.User.Build;
using WibboEmulator.Games.GameClients;
using WibboEmulator.Games.Rooms;

internal class Coords : IChatCommand
{
    public void Execute(GameClient session, Room room, RoomUser userRoom, string[] parameters) => session.SendNotification("X: " + userRoom.X + " - Y: " + userRoom.Y + " - Z: " + userRoom.Z + " - Rot: " + userRoom.RotBody);
}
