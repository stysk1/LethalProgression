using HarmonyLib;
using LethalProgression.Skills.Upgrades;
using UnityEngine;

namespace LethalProgression.Patches;

[HarmonyPatch]
internal class VoiceNoisePatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.PlayAudibleNoise))]
    private static void PlayAudibleNoisePrefix(ref float noiseRange, int noiseID, Vector3 noisePosition)
    {
        if (noiseID != Silent.VoiceNoiseId)
            return;

        // Only shrink range for the local player's own voice noise broadcast.
        var local = GameNetworkManager.Instance?.localPlayerController;
        if (local == null)
            return;

        if (Vector3.Distance(local.transform.position, noisePosition) > 1f)
            return;

        noiseRange = Silent.GetReducedVoiceRange(noiseRange);
    }
}
