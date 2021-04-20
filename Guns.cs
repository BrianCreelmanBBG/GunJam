using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace PiratePack
{
    public class Anchor : GunBehaviour
    {
        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Anchor", "Anchor");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:anchor", "bripack:anchor");
            gun.gameObject.AddComponent<Anchor>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Careful not to take off");
            gun.SetLongDescription("This anchor was once the most valuable relic in Helicopter-Beard's collection. It would still be his, but he was disqualified for too much flight.");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "Anchor_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 3);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.SetAnimationFPS(gun.idleAnimation, 60); // usually 3
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            //gun.AddProjectileModuleFrom("ak-47", true, false);

            // Projectiles:

            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(56) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 5.1f;
            gun.DefaultModule.cooldownTime = 0.1f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.SetBaseMaxAmmo(15);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.A;
            gun.encounterTrackable.EncounterGuid = "change this for different guns, so the game doesn't think they're the same gun";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);

            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration = 100;

            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //You can modify a good number of stats but for now, let's just modify the damage and speed.
            projectile.baseData.damage = 150f;
            projectile.baseData.speed = 15.0f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.force = 50;
            projectile.BossDamageMultiplier = 2.5f;
            projectile.ignoreDamageCaps = true;
            projectile.pierceMinorBreakables = true;

            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "shovel";

            //This determines what sprite you want your projectile to use. Note this isn't necessary if you don't want to have a custom projectile sprite.
            //The x and y values determine the size of your custom projectile
            projectile.SetProjectileSpriteRight("Anchor_projectile_001", 25, 25, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");

            // Charge Shot Code
            // This code did not actually get implemented, I'm just scared to get rid of it

            ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(125) as Gun).DefaultModule.projectiles[0]),
                ChargeTime = 1f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };

            ProjectileModule.ChargeProjectile chargeProjectile = new ProjectileModule.ChargeProjectile();
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
                {
                   chargeProjectile,
                };
            gun.DefaultModule.chargeProjectiles[0].ChargeTime = 2f;
            gun.DefaultModule.chargeProjectiles[0].UsedProperties = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].UsedProperties;
            gun.DefaultModule.chargeProjectiles[0].VfxPool = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].VfxPool;
            gun.DefaultModule.chargeProjectiles[0].VfxPool.type = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].VfxPool.type;
            chargeProjectile.Projectile = UnityEngine.Object.Instantiate<Projectile>(((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile);
            chargeProjectile.Projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(chargeProjectile.Projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(chargeProjectile.Projectile);
            gun.barrelOffset.localPosition += new Vector3(1, 0f, 0f);
            chargeProjectile.Projectile.transform.parent = gun.barrelOffset;
            chargeProjectile.Projectile.baseData.range = 100f;
            //chargeProjectile.Projectile.gameObject.AddComponent<Anchor/*An3s said deleted this*/>();
            chargeProjectile.Projectile.baseData.speed *= 2f;
            chargeProjectile.Projectile.AdditionalScaleMultiplier = 2f;
            // I added this
            // chargeProjectile.SetProjectileSpriteRight("Anchor_projectile_001", 25, 25, null, null);

            ETGMod.Databases.Items.Add(gun, null, "ANY");


            // synergies :
            // Didn't implement these, thing is weird

            // Glacier: adds large explosion at end

            // Macho Brace, protein pill: Reload speed halved (make sure to change framerate)

            // Ghost bullets, zombie bullets: ghost ship, makes projectile glow, summons explosions on top of hit enemy
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            //This determines what sound you want to play when you fire a gun.
            //Sounds names are based on the Gungeon sound dump, which can be found at EnterTheGungeon/Etg_Data/StreamingAssets/Audio/GeneratedSoundBanks/Windows/sfx.txt
            gun.PreventNormalFireAudio = true;
            try
            {
                AkSoundEngine.PostEvent("Anchor_Fires", gameObject);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
            }

        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected void Update()
        {
            if (gun.CurrentOwner)
            {
                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                // sounds didn't work lol
                AkSoundEngine.PostEvent("Play_Anchor_Reloads", base.gameObject);
            }
        }

        
        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            PlayerController player = (PlayerController)this.gun.CurrentOwner;

            if (player.PlayerHasActiveSynergy("Ship Ton"))
            {
                // didn't work, too scared to remove
                if (!player.HasPassiveItem(531))
                {
                    ComplexProjectileModifier flak = projectile.gameObject.GetOrAddComponent<ComplexProjectileModifier>();
                    flak.MinFlakSpawns = 200;
                }
            }
            if (player.PlayerHasActiveSynergy("Hard Feelings"))
            {

            }
            if (player.PlayerHasActiveSynergy("Gains"))
            {

            }
            if (player.PlayerHasActiveSynergy("Ghost Ship"))
            {

            }

           
        }
        
        // Fixes necessary:
        // hitbox is too big
        // does not charge yet
        // needs synergies
        // needs custom sounds
        // make spinning sprite look more like sling
        // casey hiteffect hopefully
        // maybe melee around charge

        // didn't do these, too scared to edit project+
    }

}