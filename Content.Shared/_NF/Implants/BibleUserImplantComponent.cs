using Robust.Shared.GameStates;

namespace Content.Shared._NF.Implants.Components;

/// <summary>
/// Implant to get BibleUser status (to pray, summon familiars, bless with bibles)
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BibleUserImplantComponent : Component;
