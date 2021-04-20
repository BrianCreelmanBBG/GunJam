using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PiratePack
{
    public class Module : ETGModule
    {
        public static string ZipFilePath;
        public static string FilePath;

        public static readonly string MOD_NAME = "Pirate Pack";
        public static readonly string VERSION = "0.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {
            Anchor.Add();
            ItemBuilder.Init();
           //ExamplePassive.Register();
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);

            //Synergies

            // Anchor
            // Hard Feelings
            List<string> mandatoryHardFeelingsIds = new List<string>
            {
            "bripack:anchor"
            };

            List<string> optionalHardFeelignsIds = new List<string>
            {
            "glacier"
            };//not mandatory, use it only if you want your synergy to trigger with one of multiple items
            bool ignoreLichEyesBulletsEffect = false;
            CustomSynergies.Add("Hard Feelings", mandatoryHardFeelingsIds, optionalHardFeelignsIds, ignoreLichEyesBulletsEffect);

            // Ship Ton
            List<string> mandatoryshipTonIds = new List<string>
            {
            "bripack:anchor"
            };

            List<string> optionalShiptonIDs = new List<string>
            {
            "heavy_bullets",
            "fat_bullets",
            "stout_bullets",
            "snowballets"
            };//not mandatory, use it only if you want your synergy to trigger with one of multiple items
            ignoreLichEyesBulletsEffect = false;
            CustomSynergies.Add("Ship Ton", mandatoryshipTonIds, optionalShiptonIDs, ignoreLichEyesBulletsEffect);

            // Gains
            List<string> mandatoryGainsIds = new List<string>
            {
            "bripack:anchor"
            };

            List<string> optionalGainsIds = new List<string>
            {
            "muscle_relaxant",
            "macho_brace"
            };
            ignoreLichEyesBulletsEffect = true;
            CustomSynergies.Add("Gains", mandatoryGainsIds, optionalGainsIds, ignoreLichEyesBulletsEffect);

            // Ghost Ship
            List<string> mandatoryGhostShipIds = new List<string>
            {
            "bripack:anchor"
            };

            List<string> optionalGhostShipIds = new List<string>
            {
            "zombie_bullets",
            "ghost_bullets"
            };
            ignoreLichEyesBulletsEffect = true;
            CustomSynergies.Add("Ghost Ship", mandatoryGhostShipIds, optionalGhostShipIds, ignoreLichEyesBulletsEffect);

            ZipFilePath = this.Metadata.Archive;
            FilePath = this.Metadata.Directory;

            AudioResourceLoader.InitAudio();
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
