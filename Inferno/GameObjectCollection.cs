using System.Collections;
using Inferno.GameSprites;
using Inferno.GameSprites.Items;

namespace Inferno;

public class GameObjectCollection : IEnumerable<IGameObject>
{
    public readonly List<IGameObject> Objects = new();

    public IEnumerable<IGameObject> GetObjectsAt(int x, int y) =>
        Objects.Where(o => o.GridPosX == x && o.GridPosY == y && o is not Player);

    public IEnumerable<Item> GetItemsAt(int x, int y) =>
        Objects.Where(o => o.GridPosX == x && o.GridPosY == y && o is Item).Cast<Item>();

    public void Add(IGameObject obj) => Objects.Add(obj);
    public void Remove(int x, int y) => Objects.RemoveAll(o => o.GridPosX == x && o.GridPosY == y);

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
}