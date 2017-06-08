using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a block type.
    /// </summary>
    [NoReorder]
    public sealed class BlockType : ITemplate
    {
        /// <summary>
        ///     The maximum ID.
        /// </summary>
        public const ushort MaxId = 470;

#pragma warning disable 1591
        public static readonly BlockType Honey = new BlockType(-4);
        public static readonly BlockType Lava = new BlockType(-3);
        public static readonly BlockType Water = new BlockType(-2);
        public static readonly BlockType Air = new BlockType(-1);
        public static readonly BlockType Dirt = new BlockType(0);
        public static readonly BlockType Stone = new BlockType(1);
        public static readonly BlockType Grass = new BlockType(2);
        public static readonly BlockType IronOre = new BlockType(6);
        public static readonly BlockType CopperOre = new BlockType(7);
        public static readonly BlockType GoldOre = new BlockType(8);
        public static readonly BlockType SilverOre = new BlockType(9);
        public static readonly BlockType WoodPlatform = new BlockType(19);
        public static readonly BlockType EbonwoodPlatform = new BlockType(19, -1, 18);
        public static readonly BlockType RichMahoganyPlatform = new BlockType(19, -1, 36);
        public static readonly BlockType PearlwoodPlatform = new BlockType(19, -1, 54);
        public static readonly BlockType BonePlatform = new BlockType(19, -1, 72);
        public static readonly BlockType ShadewoodPlatform = new BlockType(19, -1, 90);
        public static readonly BlockType BlueBrickPlatform = new BlockType(19, -1, 108);
        public static readonly BlockType PinkBrickPlatform = new BlockType(19, -1, 126);
        public static readonly BlockType GreenBrickPlatform = new BlockType(19, -1, 144);
        public static readonly BlockType MetalShelf = new BlockType(19, -1, 162);
        public static readonly BlockType BrassShelf = new BlockType(19, -1, 180);
        public static readonly BlockType WoodShelf = new BlockType(19, -1, 198);
        public static readonly BlockType DungeonShelf = new BlockType(19, -1, 216);
        public static readonly BlockType ObsidianPlatform = new BlockType(19, -1, 234);
        public static readonly BlockType GlassPlatform = new BlockType(19, -1, 252);
        public static readonly BlockType PumpkinPlatform = new BlockType(19, -1, 270);
        public static readonly BlockType SpookyPlatform = new BlockType(19, -1, 288);
        public static readonly BlockType PalmWoodPlatform = new BlockType(19, -1, 306);
        public static readonly BlockType MushroomPlatform = new BlockType(19, -1, 324);
        public static readonly BlockType BorealWoodPlatform = new BlockType(19, -1, 342);
        public static readonly BlockType SlimePlatform = new BlockType(19, -1, 360);
        public static readonly BlockType SteampunkPlatform = new BlockType(19, -1, 378);
        public static readonly BlockType SkywarePlatform = new BlockType(19, -1, 396);
        public static readonly BlockType LivingWoodPlatform = new BlockType(19, -1, 414);
        public static readonly BlockType HoneyPlatform = new BlockType(19, -1, 432);
        public static readonly BlockType CactusPlatform = new BlockType(19, -1, 450);
        public static readonly BlockType DemoniteOre = new BlockType(22);
        public static readonly BlockType Ebonstone = new BlockType(25);
        public static readonly BlockType Wood = new BlockType(30);
        public static readonly BlockType CorruptThorn = new BlockType(32);
        public static readonly BlockType Meteorite = new BlockType(37);
        public static readonly BlockType GrayBrick = new BlockType(38);
        public static readonly BlockType RedBrick = new BlockType(39);
        public static readonly BlockType Clay = new BlockType(40);
        public static readonly BlockType BlueBrick = new BlockType(41);
        public static readonly BlockType GreenBrick = new BlockType(43);
        public static readonly BlockType PinkBrick = new BlockType(44);
        public static readonly BlockType GoldBrick = new BlockType(45);
        public static readonly BlockType SilverBrick = new BlockType(46);
        public static readonly BlockType CopperBrick = new BlockType(47);
        public static readonly BlockType Spike = new BlockType(48);
        public static readonly BlockType Cobweb = new BlockType(51);
        public static readonly BlockType Sand = new BlockType(53);
        public static readonly BlockType Glass = new BlockType(54);
        public static readonly BlockType Obsidian = new BlockType(56);
        public static readonly BlockType Ash = new BlockType(57);
        public static readonly BlockType Hellstone = new BlockType(58);
        public static readonly BlockType Mud = new BlockType(59);
        public static readonly BlockType JungleGrass = new BlockType(60);
        public static readonly BlockType SapphireOre = new BlockType(63);
        public static readonly BlockType RubyOre = new BlockType(64);
        public static readonly BlockType EmeraldOre = new BlockType(65);
        public static readonly BlockType TopazOre = new BlockType(66);
        public static readonly BlockType AmethystOre = new BlockType(67);
        public static readonly BlockType DiamondOre = new BlockType(68);
        public static readonly BlockType JungleThorn = new BlockType(69);
        public static readonly BlockType MushroomGrass = new BlockType(70);
        public static readonly BlockType ObsidianBrick = new BlockType(75);
        public static readonly BlockType HellstoneBrick = new BlockType(76);
        public static readonly BlockType CobaltOre = new BlockType(107);
        public static readonly BlockType MythrilOre = new BlockType(108);
        public static readonly BlockType HallowedGrass = new BlockType(109);
        public static readonly BlockType AdamantiteOre = new BlockType(111);
        public static readonly BlockType Ebonsand = new BlockType(112);
        public static readonly BlockType Pearlsand = new BlockType(116);
        public static readonly BlockType Pearlstone = new BlockType(117);
        public static readonly BlockType PearlstoneBrick = new BlockType(118);
        public static readonly BlockType IridescentBrick = new BlockType(119);
        public static readonly BlockType MudstoneBrick = new BlockType(120);
        public static readonly BlockType CobaltBrick = new BlockType(121);
        public static readonly BlockType MythrilBrick = new BlockType(122);
        public static readonly BlockType Silt = new BlockType(123);
        public static readonly BlockType WoodenBeam = new BlockType(124);
        public static readonly BlockType IceRod = new BlockType(127);
        public static readonly BlockType ActiveStone = new BlockType(130);
        public static readonly BlockType InactiveStone = new BlockType(131);
        public static readonly BlockType DemoniteBrick = new BlockType(140);
        public static readonly BlockType CandyCane = new BlockType(145);
        public static readonly BlockType GreenCandyCane = new BlockType(146);
        public static readonly BlockType Snow = new BlockType(147);
        public static readonly BlockType SnowBrick = new BlockType(148);
        public static readonly BlockType AdamantiteBeam = new BlockType(150);
        public static readonly BlockType SandstoneBrick = new BlockType(151);
        public static readonly BlockType EbonstoneBrick = new BlockType(152);
        public static readonly BlockType RedStucco = new BlockType(153);
        public static readonly BlockType YellowStucco = new BlockType(154);
        public static readonly BlockType GreenStucco = new BlockType(155);
        public static readonly BlockType GrayStucco = new BlockType(156);
        public static readonly BlockType Ebonwood = new BlockType(157);
        public static readonly BlockType RichMahogany = new BlockType(158);
        public static readonly BlockType Pearlwood = new BlockType(159);
        public static readonly BlockType RainbowBrick = new BlockType(160);
        public static readonly BlockType IceBlock = new BlockType(161);
        public static readonly BlockType ThinIce = new BlockType(162);
        public static readonly BlockType PurpleIce = new BlockType(163);
        public static readonly BlockType PinkIce = new BlockType(164);
        public static readonly BlockType TinOre = new BlockType(166);
        public static readonly BlockType LeadOre = new BlockType(167);
        public static readonly BlockType TungstenOre = new BlockType(168);
        public static readonly BlockType PlatinumOre = new BlockType(169);
        public static readonly BlockType PineTree = new BlockType(170);
        public static readonly BlockType TinBrick = new BlockType(175);
        public static readonly BlockType TungstenBrick = new BlockType(176);
        public static readonly BlockType PlatinumBrick = new BlockType(177);
        public static readonly BlockType GreenMoss = new BlockType(179);
        public static readonly BlockType YellowMoss = new BlockType(180);
        public static readonly BlockType RedMoss = new BlockType(181);
        public static readonly BlockType BlueMoss = new BlockType(182);
        public static readonly BlockType PurpleMoss = new BlockType(183);
        public static readonly BlockType Cactus = new BlockType(188);
        public static readonly BlockType Cloud = new BlockType(189);
        public static readonly BlockType GlowingMushroom = new BlockType(190);
        public static readonly BlockType LivingWood = new BlockType(191);
        public static readonly BlockType Leaf = new BlockType(192);
        public static readonly BlockType Slime = new BlockType(193);
        public static readonly BlockType Bone = new BlockType(194);
        public static readonly BlockType Flesh = new BlockType(195);
        public static readonly BlockType RainCloud = new BlockType(196);
        public static readonly BlockType FrozenSlime = new BlockType(197);
        public static readonly BlockType Asphalt = new BlockType(198);
        public static readonly BlockType CrimsonGrass = new BlockType(199);
        public static readonly BlockType RedIce = new BlockType(200);
        public static readonly BlockType Sunplate = new BlockType(202);
        public static readonly BlockType Crimstone = new BlockType(203);
        public static readonly BlockType CrimtaneOre = new BlockType(204);
        public static readonly BlockType IceBrick = new BlockType(206);
        public static readonly BlockType Shadewood = new BlockType(208);
        public static readonly BlockType ChlorophyteOre = new BlockType(211);
        public static readonly BlockType Rope = new BlockType(213);
        public static readonly BlockType PalladiumOre = new BlockType(221);
        public static readonly BlockType OrichalcumOre = new BlockType(222);
        public static readonly BlockType TitaniumOre = new BlockType(223);
        public static readonly BlockType Slush = new BlockType(224);
        public static readonly BlockType Hive = new BlockType(225);
        public static readonly BlockType LihzahrdBrick = new BlockType(226);
        public static readonly BlockType HoneyBlock = new BlockType(229);
        public static readonly BlockType CrispyHoney = new BlockType(230);
        public static readonly BlockType WoodenSpike = new BlockType(232);
        public static readonly BlockType Crimsand = new BlockType(234);
        public static readonly BlockType PalladiumColumn = new BlockType(248);
        public static readonly BlockType Bubblegum = new BlockType(249);
        public static readonly BlockType Titanstone = new BlockType(250);
        public static readonly BlockType Pumpkin = new BlockType(251);
        public static readonly BlockType Hay = new BlockType(252);
        public static readonly BlockType SpookyWood = new BlockType(253);
        public static readonly BlockType AmethystGemsparkOff = new BlockType(255);
        public static readonly BlockType TopazGemsparkOff = new BlockType(256);
        public static readonly BlockType SapphireGemsparkOff = new BlockType(257);
        public static readonly BlockType EmeraldGemsparkOff = new BlockType(258);
        public static readonly BlockType RubyGemsparkOff = new BlockType(259);
        public static readonly BlockType DiamondGemsparkOff = new BlockType(260);
        public static readonly BlockType AmberGemsparkOff = new BlockType(261);
        public static readonly BlockType AmethystGemspark = new BlockType(262);
        public static readonly BlockType TopazGemspark = new BlockType(263);
        public static readonly BlockType SapphireGemspark = new BlockType(264);
        public static readonly BlockType EmeraldGemspark = new BlockType(265);
        public static readonly BlockType RubyGemspark = new BlockType(266);
        public static readonly BlockType DiamondGemspark = new BlockType(267);
        public static readonly BlockType AmberGemspark = new BlockType(268);
        public static readonly BlockType Cog = new BlockType(272);
        public static readonly BlockType StoneSlab = new BlockType(273);
        public static readonly BlockType SandstoneSlab = new BlockType(274);
        public static readonly BlockType CopperPlating = new BlockType(284);
        public static readonly BlockType DynastyWood = new BlockType(311);
        public static readonly BlockType RedDynastyShingles = new BlockType(312);
        public static readonly BlockType BlueDynastyShingles = new BlockType(313);
        public static readonly BlockType BorealWood = new BlockType(321);
        public static readonly BlockType PalmWood = new BlockType(322);
        public static readonly BlockType TinPlating = new BlockType(325);
        public static readonly BlockType Waterfall = new BlockType(326);
        public static readonly BlockType Lavafall = new BlockType(327);
        public static readonly BlockType Confetti = new BlockType(328);
        public static readonly BlockType MidnightConfetti = new BlockType(329);
        public static readonly BlockType LivingFire = new BlockType(336);
        public static readonly BlockType LivingCursedFire = new BlockType(340);
        public static readonly BlockType LivingDemonFire = new BlockType(341);
        public static readonly BlockType LivingFrostFire = new BlockType(342);
        public static readonly BlockType LivingIchorFire = new BlockType(343);
        public static readonly BlockType LivingUltrabrightFire = new BlockType(344);
        public static readonly BlockType Honeyfall = new BlockType(345);
        public static readonly BlockType ChlorophyteBrick = new BlockType(346);
        public static readonly BlockType CrimtaneBrick = new BlockType(347);
        public static readonly BlockType ShroomitePlating = new BlockType(348);
        public static readonly BlockType MartianConduitPlating = new BlockType(350);
        public static readonly BlockType Smoke = new BlockType(351);
        public static readonly BlockType CrimsonThorn = new BlockType(352);
        public static readonly BlockType VineRope = new BlockType(353);
        public static readonly BlockType SmoothMarble = new BlockType(357);
        public static readonly BlockType SilkRope = new BlockType(365);
        public static readonly BlockType WebRope = new BlockType(366);
        public static readonly BlockType Marble = new BlockType(367);
        public static readonly BlockType Granite = new BlockType(368);
        public static readonly BlockType SmoothGranite = new BlockType(369);
        public static readonly BlockType MeteoriteBrick = new BlockType(370);
        public static readonly BlockType PinkSlime = new BlockType(371);
        public static readonly BlockType Bubble = new BlockType(379);
        public static readonly BlockType DaybloomPlanter = new BlockType(380);
        public static readonly BlockType MoonglowPlanter = new BlockType(380, -1, 18);
        public static readonly BlockType BlinkrootPlanter = new BlockType(380, -1, 36);
        public static readonly BlockType ShiverthornPlanter = new BlockType(380, -1, 54);
        public static readonly BlockType WaterleafPlanter = new BlockType(380, -1, 72);
        public static readonly BlockType FireblossomPlanter = new BlockType(380, -1, 90);
        public static readonly BlockType CorruptDeathweedPlanter = new BlockType(380, -1, 108);
        public static readonly BlockType CrimsonDeathweedPlanter = new BlockType(380, -1, 126);
        public static readonly BlockType LavaMoss = new BlockType(381);
        public static readonly BlockType LivingMahogany = new BlockType(383);
        public static readonly BlockType LivingMahoganyLeaf = new BlockType(384);
        public static readonly BlockType Crystal = new BlockType(385);
        public static readonly BlockType Sandstone = new BlockType(396);
        public static readonly BlockType HardenedSand = new BlockType(397);
        public static readonly BlockType HardenedEbonsand = new BlockType(398);
        public static readonly BlockType HardenedCrimsand = new BlockType(399);
        public static readonly BlockType Ebonsandstone = new BlockType(400);
        public static readonly BlockType Crimsandstone = new BlockType(401);
        public static readonly BlockType HardenedPearlsand = new BlockType(402);
        public static readonly BlockType Pearlsandstone = new BlockType(403);
        public static readonly BlockType DesertFossil = new BlockType(404);
        public static readonly BlockType SturdyFossil = new BlockType(407);
        public static readonly BlockType Luminite = new BlockType(408);
        public static readonly BlockType LuminiteBrick = new BlockType(409);
        public static readonly BlockType SolarFragment = new BlockType(415);
        public static readonly BlockType VortexFragment = new BlockType(416);
        public static readonly BlockType NebulaFragment = new BlockType(417);
        public static readonly BlockType StardustFragment = new BlockType(418);
        public static readonly BlockType RedTeam = new BlockType(426);
        public static readonly BlockType RedTeamPlatform = new BlockType(427);
        public static readonly BlockType WireBulb = new BlockType(429);
        public static readonly BlockType GreenTeam = new BlockType(430);
        public static readonly BlockType BlueTeam = new BlockType(431);
        public static readonly BlockType YellowTeam = new BlockType(432);
        public static readonly BlockType PinkTeam = new BlockType(433);
        public static readonly BlockType WhiteTeam = new BlockType(434);
        public static readonly BlockType GreenTeamPlatform = new BlockType(435);
        public static readonly BlockType BlueTeamPlatform = new BlockType(436);
        public static readonly BlockType YellowTeamPlatform = new BlockType(437);
        public static readonly BlockType PinkTeamPlatform = new BlockType(438);
        public static readonly BlockType WhiteTeamPlatform = new BlockType(439);
        public static readonly BlockType Sandfall = new BlockType(458);
        public static readonly BlockType Snowfall = new BlockType(459);
        public static readonly BlockType SnowCloud = new BlockType(460);
#pragma warning restore 1591

        private readonly short _frameX;
        private readonly short _frameY;
        private readonly int _id;

        private BlockType(int id, short frameX = -1, short frameY = -1)
        {
            _id = id;
            _frameX = frameX;
            _frameY = frameY;
        }

        /// <summary>
        ///     Creates a new <see cref="BlockType" /> instance with the specified ID.
        /// </summary>
        /// <param name="id">The ID, which must be less than <see cref="MaxId" />.</param>
        /// <returns>The block type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id" /> is too large.</exception>
        [NotNull]
        public static BlockType FromId(ushort id)
        {
            if (id >= MaxId)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID is too large.");
            }

            return new BlockType(id);
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            if (-4 <= _id && _id <= -2)
            {
                tile.Liquid = 255;
                tile.LiquidType = -_id - 2;
            }
            else
            {
                tile.Liquid = 0;
                tile.LiquidType = 0;
            }

            tile.IsActive = _id >= 0;
            tile.FrameX = _frameX;
            tile.FrameY = _frameY;
            tile.BlockId = (ushort)Math.Max(0, _id);
            return tile;
        }

        /// <inheritdoc />
        /// <remarks>
        ///     This method will ignore frame values of -1; they essentially act as placeholders.
        /// </remarks>
        public bool Matches(Tile tile)
        {
            if (-4 <= _id && _id <= -2)
            {
                return !tile.IsActive && tile.Liquid > 0 && tile.LiquidType == -_id - 2;
            }
            if (_id == -1)
            {
                return !tile.IsActive && tile.Liquid == 0;
            }

            var doFramesMatch = (_frameX == -1 || tile.FrameX == _frameX) && (_frameY == -1 || tile.FrameY == _frameY);
            return tile.IsActive && tile.BlockId == _id && doFramesMatch;
        }

        /// <summary>
        ///     Creates a new <see cref="BlockType" /> instance from this block type with the specified frames.
        /// </summary>
        /// <param name="frameX">The X frame.</param>
        /// <param name="frameY">The Y frame.</param>
        /// <returns>The block type.</returns>
        [NotNull]
        [Pure]
        public BlockType WithFrames(short frameX, short frameY) => new BlockType(_id, frameX, frameY);
    }
}
