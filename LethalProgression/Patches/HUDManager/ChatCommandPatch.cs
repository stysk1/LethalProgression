using HarmonyLib;
using GameNetcodeStuff;
using System.Linq;

namespace LethalProgression.Patches;

[HarmonyPatch]
internal class ChatCommandPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HUDManager), "AddTextToChatOnServer")]
    private static bool HandleChatCommand(string chatMessage)
    {
        if (!chatMessage.StartsWith("/levelup", System.StringComparison.OrdinalIgnoreCase))
            return true;

        LethalPlugin.Log.LogInfo($"levelup command received: '{chatMessage}'");

        if (LP_NetworkManager.xpInstance == null)
        {
            LethalPlugin.Log.LogWarning("levelup command: xpInstance is null, game not fully initialized");
            HUDManager.Instance.DisplayTip("Level Up", "Game not fully initialized, try again.", true);
            return false;
        }

        if (!GameNetworkManager.Instance.isHostingGame)
        {
            HUDManager.Instance.DisplayTip("Level Up", "Only the host can use this command.", true);
            return false;
        }

        string[] args = chatMessage.Length > "/levelup".Length
            ? chatMessage.Substring("/levelup".Length).Trim().Split(' ')
            : System.Array.Empty<string>();

        int amount = 1;
        int nameStartIndex = 0;
        if (args.Length > 0 && int.TryParse(args[0], out int parsedAmount))
        {
            if (parsedAmount <= 0)
            {
                HUDManager.Instance.DisplayTip("Level Up", "Amount must be a positive number.", true);
                return false;
            }
            amount = parsedAmount;
            nameStartIndex = 1;
        }

        string targetName = string.Join(" ", args.Skip(nameStartIndex)).Trim();

        PlayerControllerB target;

        if (string.IsNullOrEmpty(targetName))
        {
            target = GameNetworkManager.Instance.localPlayerController;
        }
        else
        {
            target = StartOfRound.Instance.allPlayerScripts.FirstOrDefault(p =>
                p.gameObject.activeSelf &&
                p.playerUsername.IndexOf(targetName, System.StringComparison.OrdinalIgnoreCase) >= 0);

            if (target == null)
            {
                HUDManager.Instance.DisplayTip("Level Up", $"Player '{targetName}' not found.", true);
                return false;
            }
        }

        LC_XP xp = LP_NetworkManager.xpInstance;

        if (target == GameNetworkManager.Instance.localPlayerController)
        {
            xp.UpdateSkillPoints_S2CMessage(amount);
        }
        else
        {
            xp.updatePlayerSkillpointsServerMessage.SendClient(amount, target.actualClientId);
        }

        LethalPlugin.Log.LogInfo($"Host granted {amount} skill point(s) to {target.playerUsername}.");
        HUDManager.Instance.DisplayTip("Level Up", $"Granted {amount} skill point(s) to {target.playerUsername}.");
        return false;
    }
}
