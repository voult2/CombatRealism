using UnityEngine;
using Verse;

namespace Combat_Realism.Detours
{
    [StaticConstructorOnStartup]
    internal class Initializer : ITab
    {
        protected static GameObject iconControllerObject;

        static Initializer()
        {
            Log.Message("Initialized Detour Core.");
            iconControllerObject = new GameObject("Detour Core Initializer");
            iconControllerObject.AddComponent<InitializerBehaviour>();
            Object.DontDestroyOnLoad(iconControllerObject);
        }

        protected override void FillTab()
        {

        }
    }
}