using Inferno.GameSprites.Items;

namespace Inferno.GameSprites;

public class ContainerChestWood() :
    GameObject<StatefulAnimatedTextureWrapper>(Textures.ContainerChestWood, "Chest")
{
    public override bool CanOpen { get; set; } = true;

    public bool IsOpen { get; private set; }

    private const int OpenState = 1;

    private const int ClosedState = 0;

    public List<Item> Items { get; init; }

    private void OnOpen(IGameObject opener)
    {
        foreach (var i in Items)
        {
            Program.AddWorldSprite(i, X, Y);
        }

        Items.Clear();
    }

    public override void Open(IGameObject opener)
    {
        _texture.State = OpenState;
        if (!IsOpen) OnOpen(opener);
        IsOpen = true;
        CanOpen = false;
    }
}