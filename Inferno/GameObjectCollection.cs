using System.Collections;
using Inferno.GameSprites;
using Inferno.GameSprites.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Inferno;

public class GameObjectCollection : IEnumerable<IGameObject>
{
    public readonly List<IGameObject> Objects = new();

    public IEnumerable<IGameObject> GetObjectsAt(int x, int y) =>
        Objects.Where(o => o.X == x && o.Y == y && o is not Player);

    public IEnumerable<Item> GetItemsAt(int x, int y) =>
        Objects.Where(o => o.X == x && o.Y == y && o is Item).Cast<Item>();

    public void Add(IGameObject obj) => Objects.Add(obj);
    public void Remove(int x, int y) => Objects.RemoveAll(o => o.X == x && o.Y == y);

    public void Remove(IGameObject obj) => Objects.Remove(obj);

    public IEnumerator<IGameObject> GetEnumerator()
    {
        return Objects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //returns all items in the four directions around the player + the tile the player is standing on
    public IEnumerable<IGameObject> GetItemsAround(int playerGridPosX, int playerGridPosY)
    {
        var items = new List<IGameObject>();
        items.AddRange(GetItemsAt(playerGridPosX, playerGridPosY));
        items.AddRange(GetItemsAt(playerGridPosX + 1, playerGridPosY));
        items.AddRange(GetItemsAt(playerGridPosX - 1, playerGridPosY));
        items.AddRange(GetItemsAt(playerGridPosX, playerGridPosY + 1));
        items.AddRange(GetItemsAt(playerGridPosX, playerGridPosY - 1));
        return items;
    }

    public IEnumerable<IGameObject> GetObjectsAround(int playerGridPosX, int playerGridPosY)
    {
        var items = new List<IGameObject>();
        items.AddRange(GetObjectsAt(playerGridPosX, playerGridPosY));
        items.AddRange(GetObjectsAt(playerGridPosX + 1, playerGridPosY));
        items.AddRange(GetObjectsAt(playerGridPosX - 1, playerGridPosY));
        items.AddRange(GetObjectsAt(playerGridPosX, playerGridPosY + 1));
        items.AddRange(GetObjectsAt(playerGridPosX, playerGridPosY - 1));
        return items;
    }

    private record struct GameObjectSave(string Type, int X, int Y, string? Config = null);
    
    public void Load(string saveStr)
    {
        
    }

    public string Save()
    {
        var retData = new GameObjectSave[Objects.Count];
        for (var index = 0; index < Objects.Count; index++)
        {
            var o = Objects[index];
            retData[index] = new GameObjectSave(o.GetType().Name, o.X, o.Y);
        }

        return JArray.FromObject(retData).ToString(Formatting.Indented);
    }
        
}