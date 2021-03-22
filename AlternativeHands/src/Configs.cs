using System;
using BepInEx.Configuration;
using FistVR;

namespace AlternativeHands
{
	public class HandConfig
	{
		public ConfigEntry<bool> Enabled { get; }
		public ConfigEntry<DisplayMode> Type { get; }

		public event EventHandler SettingChanged
		{
			add
			{
				Enabled.SettingChanged += value;
				Type.SettingChanged += value;
			}
			remove
			{
				Enabled.SettingChanged -= value;
				Type.SettingChanged -= value;
			}
		}

		public HandConfig(ConfigFile config, string section)
		{
			Enabled = config.Bind(section, nameof(Enabled), true, "Whether or not to alter the hand.");
			Type = config.Bind(section, nameof(Type), DisplayMode.Index, "What to set the hand type to.");
		}
	}
	
	public class HandsConfig
	{
		public HandConfig Left { get; }
		public HandConfig Right { get; }
		
		public event EventHandler SettingChanged
		{
			add
			{
				Left.SettingChanged += value;
				Right.SettingChanged += value;
			}
			remove
			{
				Left.SettingChanged -= value;
				Right.SettingChanged += value;
			}
		}

		public HandsConfig(ConfigFile config)
		{
			Left = new HandConfig(config, nameof(Left));
			Right = new HandConfig(config, nameof(Right));
		}
	}
}