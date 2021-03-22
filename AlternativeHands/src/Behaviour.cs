using System;
using Deli.Setup;
using FistVR;

namespace AlternativeHands
{
	public class Behaviour : DeliBehaviour
	{
		private readonly HandsConfig _config;
		
		public Behaviour()
		{
			_config = new HandsConfig(Config);
			_config.SettingChanged += SettingChanged;
		}

		private void SettingChanged(object sender, EventArgs e)
		{
			Logger.LogMessage("Configs cannot be changed after launch. Please ensure the config file has been adjusted, then restart the game for the changes to take effect.");
		}

		private void Awake()
		{
			On.FistVR.FVRViveHand.UpdateControllerDefinition += UpdateControllerDefinition;
		}

		private void UpdateControllerDefinition(On.FistVR.FVRViveHand.orig_UpdateControllerDefinition orig, FVRViveHand self)
		{
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

			orig(self);
		}
	}
}