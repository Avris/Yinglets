using System;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

namespace Yingworld
{
    class ColorGenerator_ChannelSync : ColorGenerator
	{
		public string channel;
		public bool second;

		public override Color NewRandomizedColor()
		{
			return new Color(1f, 0f, 1f, 1f);
		}
	}
}
