using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;


namespace ShellTooth
{
	class FodderDef : ThingDef
	{
		public FodderDef()
		{
			altitudeLayer = AltitudeLayer.Building;
			building = new BuildingProperties();
			category = ThingCategory.Building;
			drawerType = DrawerType.MapMeshAndRealTime;
			selectable = true;
			graphicData = new GraphicData
			{
				graphicClass = typeof(Graphic_Single),
				shaderType = ShaderTypeDefOf.CutoutComplex
			};
			comps = new List<CompProperties>
			{
					new CompProperties_Forbiddable()
			};

		}
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				graphic = graphicData.Graphic;
				terrainTypes.ForEach(delegate (String terrain) {
					FodderUtility.defList[terrain].Add(this);
				});
			});
			base.PostLoad();
		}
		public List<String> terrainTypes = FodderUtility.tileTypes;
		public IntVec3 rarityThresholds = new IntVec3(80, 18, 2);
		public List<FodderDrop> rareRewards;
		public List<FodderDrop> uncommonRewards;
		public List<FodderDrop> commonRewards;

	}
	public class FodderDrop
	{
		public ThingDef item;
		public ThingDef winterItem = null;
		public int count = 1;
		public bool scalesWithSkill = false;
	}
}
