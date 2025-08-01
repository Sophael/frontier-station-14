using Content.Shared.Construction;
using Content.Shared.Construction.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;
using Content.Shared._NF.BindToStation; // Frontier

namespace Content.Client.Construction;

/// <inheritdoc/>
public sealed class FlatpackSystem : SharedFlatpackSystem
{
    [Dependency] private readonly AppearanceSystem _appearance = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlatpackComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(Entity<FlatpackComponent> ent, ref AppearanceChangeEvent args)
    {
        var (_, comp) = ent;
        if (!_appearance.TryGetData<string>(ent, FlatpackVisuals.Machine, out var machineBoardId) || args.Sprite == null)
            return;

        if (!PrototypeManager.TryIndex<EntityPrototype>(machineBoardId, out var machineBoardPrototype))
            return;

        if (!machineBoardPrototype.TryGetComponent<SpriteComponent>(out var sprite, EntityManager.ComponentFactory))
            return;

        Color? color = null;
        foreach (var layer in sprite.AllLayers)
        {
            if (layer.RsiState.Name is not { } spriteState)
                continue;

            if (!comp.BoardColors.TryGetValue(spriteState, out var c))
                continue;
            color = c;
            break;
        }

        if (color != null)
            _sprite.LayerSetColor((ent.Owner, args.Sprite), FlatpackVisualLayers.Overlay, color.Value);
    }

    // Frontier: station binding
    // NOOP, all done server-side.
    protected override void BindToStation(EntityUid toBind, StationBoundObjectComponent bindingParams)
    {
    }
    // End Frontier: station binding
}
