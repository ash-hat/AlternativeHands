using System;
using Deli;
using FistVR;
using HarmonyLib;

namespace AlternativeHands
{
	public class Behaviour : DeliBehaviour
	{
#pragma warning disable 8618
		private static Behaviour _this;
#pragma warning restore 8618
		
		private readonly HandsConfig _config;
		
		public Behaviour()
		{
			_config = new HandsConfig(Config);
			_config.SettingChanged += SettingChanged;
			
			_this = this;
		}

		private void SettingChanged(object sender, EventArgs e)
		{
			Logger.LogMessage("Configs cannot be changed after launch. Please ensure the config file has been adjusted, then restart the game for the changes to take effect.");
		}

		private void Awake()
		{
			new Harmony(Info.Guid).PatchAll(typeof(Behaviour));
		}

		[HarmonyPatch(typeof(FVRViveHand), nameof(FVRViveHand.UpdateControllerDefinition))]
		[HarmonyPrefix]
		private static void UpdateControllerDefinition(FVRViveHand __instance)
		{
			// Aliases for ease of rebasing
			var self = __instance;
			var _config = _this._config;
			var Logger = _this.Logger;
			
			var isRight = self.IsThisTheRightHand;
			var config = isRight ? _config.Right : _config.Left;

			ref var dmode = ref self.DMode;
			var sideLocale = isRight ? "right" : "left";
			if (config.Enabled.Value)
			{
				dmode = config.Type.Value;
				
				Logger.LogInfo($"Set the {sideLocale} controller type to: {dmode}");
			}
			else
			{
				Logger.LogInfo($"Defaulted the {sideLocale} controller type to: {dmode}");
			}
		}
	}
}