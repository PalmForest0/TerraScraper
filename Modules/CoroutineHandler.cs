using System;
using System.Collections;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace TerraScraper.Modules;

internal class CoroutineHandler : ModSystem
{
    private static readonly List<(IEnumerator coroutine, Action callback)> coroutines = new();

    public static void StartNew(IEnumerator coroutine, Action callback = null) => coroutines.Add((coroutine, callback));

    public override void PostUpdateEverything()
    {
        for (int i = coroutines.Count - 1; i >= 0; i--)
        {
            if (coroutines[i].coroutine is null)
                coroutines.RemoveAt(i);

            // MoveNext returns false if the coroutine is finished, so invoke callback action
            if (!coroutines[i].coroutine.MoveNext())
            {
                coroutines[i].callback?.Invoke();
                coroutines.RemoveAt(i);
            }     
        }
    }

    public override void Unload() => coroutines.Clear();
}