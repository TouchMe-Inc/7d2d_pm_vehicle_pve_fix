using PluginManager.Api;
using PluginManager.Api.Capabilities.Implementations.Events.GameEvents;
using PluginManager.Api.Capabilities.Implementations.Utils;
using PluginManager.Api.Contracts;
using PluginManager.Api.Hooks;

namespace VehiclePveFix;

public class VehiclePveFix : BasePlugin
{
    public override string ModuleName => "VehiclePveFix";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "TouchMe-Inc";
    public override string ModuleDescription => "Show welcome message";

    protected override void OnLoad()
    {
        RegisterEventHandler<EntityDamageEvent>(OnEntityDamageEvent, HookMode.Pre);
    }

    private HookResult OnEntityDamageEvent(EntityDamageEvent evt)
    {
        if (Capabilities.Get<IGamePrefsUtil>().GetInt(GamePrefs.PlayerKillingMode) != (int)PlayerKillingMode.NoKilling)
        {
            return HookResult.Continue;
        }

        if (!Capabilities.Get<IPlayerUtil>().IsPlayerInVehicle(evt.VictimEntityId))
        {
            return HookResult.Continue;
        }

        if (!Capabilities.Get<IPlayerUtil>().IsPlayer(evt.AttackerEntityId))
        {
            return HookResult.Continue;
        }

        evt.Strength = 1;

        return HookResult.Changed;
    }
}