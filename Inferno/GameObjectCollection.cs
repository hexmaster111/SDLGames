﻿using System.Collections;
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
}