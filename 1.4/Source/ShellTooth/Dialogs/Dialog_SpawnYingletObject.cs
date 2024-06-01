using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using AlienRace;


namespace ShellTooth
{
	public class Dialog_SpawnYingletObject : Window
	{
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(602f, 562f);
			}
		}

		public Dialog_SpawnYingletObject()
		{
			this.forcePause = true;
			this.draggable = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = false;
			Text.Font = GameFont.Small;
			PrepTabs();
			SetLists();
		}
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}
		public override void DoWindowContents(Rect container)
		{
			Rect tabCont = new Rect(0, 0 + 64, container.width - Margin * 2, card.y * 3).CenteredOnXIn(container);
			Rect bg = new Rect(tabCont.x, tabCont.y + 1, tabCont.width, tabCont.height + 8);
			List<ThingDef> things = new List<ThingDef>();
			Color bgColor = new Color(0.1686f, 0.1725f, 0.1764f);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(container, "Yinglet Stuff");
			Text.Anchor = TextAnchor.UpperLeft;
			TabDrawer.DrawTabs(tabCont, tabsList);
			GUI.BeginGroup(container);
			switch (tab)
			{
				case Tabs.Char:
					SortTabs("Char");
					Widgets.DrawRectFast(bg, bgColor);
					YingCharTab(tabCont);
					break;
				case Tabs.Apparel:
					Widgets.DrawRectFast(bg, bgColor);
					YingItemTab(tabCont, yingletApparel);
					break;
				case Tabs.Items:
					SortTabs("Items");
					Widgets.DrawRectFast(bg, bgColor);
					YingItemTab(tabCont, yingletItems);
					break;
				case Tabs.Fodder:
					SortTabs("Fodder");
					Widgets.DrawRectFast(bg, bgColor);
					YingItemTab(tabCont, yingletFodder);
					break;
				default:
					return;
			}
			GUI.EndGroup();
		}
		private void SortTabs(String tablabel)
		{
		}
		private void PrepTabs()
		{
			tabsList.Clear();
			tabsList.Add(new TabRecord("Char", delegate ()
			{
				this.tab = Tabs.Char;
			}, () => this.tab == Tabs.Char));
			tabsList.Add(new TabRecord("Apparel", delegate ()
			{
				this.tab = Tabs.Apparel;
			}, () => this.tab == Tabs.Apparel));
			tabsList.Add(new TabRecord("Items", delegate ()
			{
				this.tab = Tabs.Items;
			}, () => this.tab == Tabs.Items));
			tabsList.Add(new TabRecord("Fodder", delegate ()
			{
				this.tab = Tabs.Fodder;
			}, () => this.tab == Tabs.Fodder));
		}
		private void YingItemTab(Rect container, List<ThingDef> thingList)
		{
			if (thingList.Count > 0)
			{
				container.y += 8;
				container.width -= 1;
				float scrollerWidth = card.x * 4;
				float scrollerHeight = card.y * (float)Math.Ceiling(thingList.Count / 4f);
				Rect viewRect = new Rect(container.x, container.y, scrollerWidth, scrollerHeight);
				Widgets.BeginScrollView(container, ref this.scrollPosition, viewRect, true);
				viewRect.x = container.x + ((container.width - scrollerWidth) / 2) - 6;
				float offset = 0;
				if (thingList.Count <= 12)
				{
					offset = 8;
				}
				if (this.tab == Tabs.Apparel)
				{
					for (int i = 0, x = 0, y = 0; i < thingList.Count; i++, x++)
					{
						if (x != 0 && x % 4 == 0)
						{
							y++;
							x = 0;
						}
						Rect tile = new Rect(viewRect.x + offset + (card.x * x), viewRect.y + card.y * y, card.x, card.y);
						YingItemTile(tile, thingList[i], IsHelmet(thingList[i]) ? 0.75f : 1.5f, IsHelmet(thingList[i]) ? -10 : -46);
					}
				}
				else if (this.tab == Tabs.Items)
				{
						for (int i = 0, x = 0, y = 0; i < thingList.Count; i++, x++)
						{
							if (x != 0 && x % 4 == 0)
							{
								y++;
								x = 0;
							}
							Rect tile = new Rect(viewRect.x + offset + (card.x * x), viewRect.y + card.y * y, card.x, card.y);
							YingItemTile(tile, thingList[i]);
						}
				}
				else if (this.tab == Tabs.Fodder)
				{
						for (int i = 0, x = 0, y = 0; i < thingList.Count; i++, x++)
						{
							if (x != 0 && x % 4 == 0)
							{
								y++;
								x = 0;
							}
							Rect tile = new Rect(viewRect.x + offset + (card.x * x), viewRect.y + card.y * y, card.x, card.y);
							YingItemTile(tile, thingList[i]);
						}
				}
				Widgets.EndScrollView();
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(container, "There's nothing here yet!");
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}
		private void YingItemTile(Rect tile, ThingDef def, float scale = 1, float offset = 0)
		{
			float w = 128;
			Rect icon = new Rect(0, tile.y + 4, w, w).CenteredOnXIn(tile);
			Rect frame = new Rect(0, icon.y + w, w, tile.height - w - 11).CenteredOnXIn(tile);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(frame, def.label ?? "err: no label");
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawAltRect(frame);
			if (Widgets.ButtonImageFitted(icon, greyBG))
			{
				DebugTools.curTool = new DebugTool(def.label ?? "err: no label", delegate ()
				{
					DebugThingPlaceHelper.DebugSpawn(def, UI.MouseCell());
				});
			}
			icon.y += offset;
			Widgets.DrawTextureFitted(icon, ContentFinder<Texture2D>.Get(def.graphicData.texPath, false) ?? ThingMaker.MakeThing(def).Graphic.MatAt(Rot4.South).mainTexture, scale);
		}
		private void YingCharTab(Rect container)
		{
			Rect tab = new Rect(container.x, container.y += 8, card.x * 3, card.y * 3).CenteredOnXIn(container);
			for (int i = 0, x = 0, y = 0, z = 2; !(y > 1 && x > 2); i++, x++, z--)
			{
				if (x > 2)
				{
					y++;
					x = 0;
					z = 2;
				}
				Rect tile = new Rect(tab.x + (card.x * x) + 3, tab.y + card.y * y, card.x, card.y);
				switch (y)
				{
					default:
					case 0:
						YingCharTile(tile, y, z);
						break;
					case 1:
						YingCharTile(tile, y, z);
						break;
					case 2:
						YingCharTile(tile, y, z);
						break;
				}
			}
		}
		private void YingCharTile(Rect tile, int ind, int stage)
		{
			float w = 128;
			Rect icon = new Rect(0, tile.y + 4, w, w).CenteredOnXIn(tile);
			Rect frame = new Rect(0, icon.y + w, w, tile.height - w - 11).CenteredOnXIn(tile);
			Widgets.DrawAltRect(frame);
			if (Widgets.ButtonImageFitted(icon, greyBG))
			{
				DebugTools.curTool = new DebugTool(templates[ind].def.label, delegate ()
				{
					Pawn pawn = new Pawn();
					switch (templates[ind].def.defName)
					{
						default:
							pawn = PawnGenerator.GeneratePawn(templates[ind].def.race.AnyPawnKind, Faction.OfPlayer);
							break;
						case "Alien_Younglet":
						case "Alien_Yinglet":
							pawn = YingletTemplate(templates[ind].def, stage);
							break;
					}
					GenSpawn.Spawn(pawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
				});
			}
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(frame, $"{templates[ind].def.race.lifeStageAges[stage].def}");
			Text.Anchor = TextAnchor.UpperLeft;
			icon.height -= 16;
			Widgets.ThingIcon(icon.ContractedBy((2f - stage) * 5), templates[ind]);
		}
		private static bool IsHelmet(ThingDef def)
		{
			List<BodyPartGroupDef> headParts = new List<BodyPartGroupDef>()
			{
				BodyPartGroupDefOf.Eyes,
				BodyPartGroupDefOf.FullHead,
				BodyPartGroupDefOf.UpperHead
			};			
			List<BodyPartGroupDef> bodyParts = new List<BodyPartGroupDef>()
			{
				BodyPartGroupDefOf.LeftHand,
				BodyPartGroupDefOf.RightHand,
				BodyPartGroupDefOf.Torso,
				BodyPartGroupDefOf.Legs
			};
			if (def.apparel.bodyPartGroups.Intersect(headParts).Any() && !def.apparel.bodyPartGroups.Intersect(bodyParts).Any())
			{
				return true;
			}
			return false;
		}
		public static Pawn YingletTemplate(ThingDef pawntype, int lifeStage = -1, bool random = false)
		{
			if (pawntype != YingDefOf.Alien_Yinglet || pawntype != YingDefOf.Alien_Younglet)
			{ 
			}
			List<LifeStageAge> defStages = pawntype.race.lifeStageAges;
			float age = defStages[defStages.Count - 1].minAge;
			switch (lifeStage)
			{
                default:
						break;
				case 1:
					age = defStages[1].minAge;
					break;
				case 0:
					age = defStages[0].minAge;
					break;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(
				tile: Current.Game.AnyPlayerHomeMap.Tile,
				kind: pawntype.race.AnyPawnKind,
				faction: Faction.OfPlayer,
				forceGenerateNewPawn: true,
				canGeneratePawnRelations: false,
				allowFood: false,
				allowAddictions: false,
				inhabitant: true,
				fixedBiologicalAge: age,
				fixedChronologicalAge: age,
				fixedGender: Gender.Female
			);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			pawn.apparel = new Pawn_ApparelTracker(pawn);

			if (!random)
			{
				if (pawntype == YingDefOf.Alien_Yinglet)
				{
					pawn.Name = new NameTriple("", "Yinglet", "");
					pawn.story.bodyType = YingDefOf.Ying;
				}
				else
				{
					pawn.Name = new NameTriple("", "Younglet", "");
				}
				pawn.story.HairColor = new Color(0.23f, 0.19f, 0.15f);
				AlienPartGenerator.AlienComp parts = pawn.GetComp<AlienPartGenerator.AlienComp>();
				parts.ColorChannels["eye"] = new AlienPartGenerator.ExposableValueTuple<Color, Color>(new Color(1.00f, 0.82f, 0.34f), new Color(1f, 1f, 1f));
				parts.ColorChannels["skin"] = new AlienPartGenerator.ExposableValueTuple<Color, Color>(new Color(0.51f, 0.39f, 0.28f), new Color(0.78f, 0.53f, 0.55f));
				parts.addonVariants = new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1 };
			}
			return pawn;
		}
		private static void AssignDamnedPawnstuff(Pawn pawn) // Because Rimworld has trouble with pawns being generated rapidly for some stupid reason
		{
			pawn.Name = new NameTriple("", "Yinglet", "");
			AlienPartGenerator.AlienComp parts = pawn.GetComp<AlienPartGenerator.AlienComp>();
			parts.ColorChannels["eye"] = new AlienPartGenerator.ExposableValueTuple<Color, Color>(new Color(1.00f, 0.82f, 0.34f), new Color(1f, 1f, 1f));
			parts.ColorChannels["hair"] = new AlienPartGenerator.ExposableValueTuple<Color, Color>(new Color(0.23f, 0.19f, 0.15f), new Color(1f, 1f, 1f));
			parts.ColorChannels["skin"] = new AlienPartGenerator.ExposableValueTuple<Color, Color>(new Color(0.51f, 0.39f, 0.28f), new Color(0.78f, 0.53f, 0.55f));
			parts.addonVariants = new List<int> { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1 };
		}
		private static void SetLists()
		{
			yingletThings = DefDatabase<ThingDef>.AllDefsListForReading.FindAll((ThingDef def) => def.thingCategories != null && def.thingCategories.Contains(YingDefOf.YingletThing));
			yingletApparel = yingletThings.FindAll((ThingDef def) => def.IsApparel);
			yingletItems = yingletThings.FindAll((ThingDef def) => !def.IsApparel);
			yingletFodder = DefDatabase<ThingDef>.AllDefsListForReading.FindAll((ThingDef def) => def is FodderDef);
			templates.Add(YingletTemplate(YingDefOf.Alien_Yinglet));
			templates.Add(YingletTemplate(YingDefOf.Alien_Younglet));
			templates.Add(PawnGenerator.GeneratePawn(PawnKindDef.Named("YingWorld_Tiplod"), null));
	}
		private Vector2 card = new Vector2(142, 162);
		private Vector2 scrollPosition;
		private Tabs tab;
		public static List<Pawn> templates = new List<Pawn>();
		public static List<TabRecord> tabsList = new List<TabRecord>();
		public static List<ThingDef> yingletThings = new List<ThingDef>();
		public static List<ThingDef> yingletApparel = new List<ThingDef>();
		public static List<ThingDef> yingletItems = new List<ThingDef>();
		public static List<ThingDef> yingletFodder = new List<ThingDef>();
		Texture2D greyBG = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 51, 51, 127).ToColor);
		private enum Tabs
		{
			Char,
			Apparel,
			Items,
			Fodder
		}
	}
	public class YingButton
	{ }
}
