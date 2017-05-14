using System;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a block type.
    /// </summary>
    public class Block : ITemplate
    {
        public static readonly Block ActiveStone = new Block(130);
        public static readonly Block AdamantiteBeam = new Block(150);
        public static readonly Block AdamantiteOre = new Block(111);
        public static readonly Block Air = new Block(-1);
        public static readonly Block AmberGemspark = new Block(268);
        public static readonly Block AmberGemsparkOff = new Block(261);
        public static readonly Block AmethystGemspark = new Block(262);
        public static readonly Block AmethystGemsparkOff = new Block(255);
        public static readonly Block AmethystOre = new Block(67);
        public static readonly Block Ash = new Block(57);
        public static readonly Block Asphalt = new Block(198);
        public static readonly Block BlinkrootPlanter = new Block(380, -1, 36);
        public static readonly Block BlueBrick = new Block(41);
        public static readonly Block BlueBrickPlatform = new Block(19, -1, 108);
        public static readonly Block BlueDynastyShingles = new Block(313);
        public static readonly Block BlueMoss = new Block(182);
        public static readonly Block BlueTeam = new Block(431);
        public static readonly Block BlueTeamPlatform = new Block(436);
        public static readonly Block Bone = new Block(194);
        public static readonly Block BonePlatform = new Block(19, -1, 72);
        public static readonly Block BorealWood = new Block(321);
        public static readonly Block BorealWoodPlatform = new Block(19, -1, 342);
        public static readonly Block BrassShelf = new Block(19, -1, 180);
        public static readonly Block Bubble = new Block(379);
        public static readonly Block Bubblegum = new Block(249);
        public static readonly Block Cactus = new Block(188);
        public static readonly Block CactusPlatform = new Block(19, -1, 450);
        public static readonly Block CandyCane = new Block(145);
        public static readonly Block ChlorophyteBrick = new Block(346);
        public static readonly Block ChlorophyteOre = new Block(211);
        public static readonly Block Clay = new Block(40);
        public static readonly Block Cloud = new Block(189);
        public static readonly Block CobaltBrick = new Block(121);
        public static readonly Block CobaltOre = new Block(107);
        public static readonly Block Cobweb = new Block(51);
        public static readonly Block Cog = new Block(272);
        public static readonly Block Confetti = new Block(328);
        public static readonly Block CopperBrick = new Block(47);
        public static readonly Block CopperOre = new Block(7);
        public static readonly Block CopperPlating = new Block(284);
        public static readonly Block CorruptDeathweedPlanter = new Block(380, -1, 108);
        public static readonly Block CorruptThorn = new Block(32);
        public static readonly Block Crimsand = new Block(234);
        public static readonly Block Crimsandstone = new Block(401);
        public static readonly Block CrimsonDeathweedPlanter = new Block(380, -1, 126);
        public static readonly Block CrimsonGrass = new Block(199);
        public static readonly Block CrimsonThorn = new Block(352);
        public static readonly Block Crimstone = new Block(203);
        public static readonly Block CrimtaneBrick = new Block(347);
        public static readonly Block CrimtaneOre = new Block(204);
        public static readonly Block CrispyHoney = new Block(230);
        public static readonly Block Crystal = new Block(385);
        public static readonly Block DaybloomPlanter = new Block(380);
        public static readonly Block DemoniteBrick = new Block(140);
        public static readonly Block DemoniteOre = new Block(22);
        public static readonly Block DesertFossil = new Block(404);
        public static readonly Block DiamondGemspark = new Block(267);
        public static readonly Block DiamondGemsparkOff = new Block(260);
        public static readonly Block DiamondOre = new Block(68);
        public static readonly Block Dirt = new Block(0);
        public static readonly Block DungeonShelf = new Block(19, -1, 216);
        public static readonly Block DynastyWood = new Block(311);
        public static readonly Block Ebonsand = new Block(112);
        public static readonly Block Ebonsandstone = new Block(400);
        public static readonly Block Ebonstone = new Block(25);
        public static readonly Block EbonstoneBrick = new Block(152);
        public static readonly Block Ebonwood = new Block(157);
        public static readonly Block EbonwoodPlatform = new Block(19, -1, 18);
        public static readonly Block EmeraldGemspark = new Block(265);
        public static readonly Block EmeraldGemsparkOff = new Block(258);
        public static readonly Block EmeraldOre = new Block(65);
        public static readonly Block FireblossomPlanter = new Block(380, -1, 90);
        public static readonly Block Flesh = new Block(195);
        public static readonly Block FrozenSlime = new Block(197);
        public static readonly Block Glass = new Block(54);
        public static readonly Block GlassPlatform = new Block(19, -1, 252);
        public static readonly Block GlowingMushroom = new Block(190);
        public static readonly Block GoldBrick = new Block(45);
        public static readonly Block GoldOre = new Block(8);
        public static readonly Block Granite = new Block(368);
        public static readonly Block Grass = new Block(2);
        public static readonly Block GrayBrick = new Block(38);
        public static readonly Block GrayStucco = new Block(156);
        public static readonly Block GreenBrick = new Block(43);
        public static readonly Block GreenBrickPlatform = new Block(19, -1, 144);
        public static readonly Block GreenCandyCane = new Block(146);
        public static readonly Block GreenMoss = new Block(179);
        public static readonly Block GreenStucco = new Block(155);
        public static readonly Block GreenTeam = new Block(430);
        public static readonly Block GreenTeamPlatform = new Block(435);
        public static readonly Block HallowedGrass = new Block(109);
        public static readonly Block HardenedCrimsand = new Block(399);
        public static readonly Block HardenedEbonsand = new Block(398);
        public static readonly Block HardenedPearlsand = new Block(402);
        public static readonly Block HardenedSand = new Block(397);
        public static readonly Block Hay = new Block(252);
        public static readonly Block Hellstone = new Block(58);
        public static readonly Block HellstoneBrick = new Block(76);
        public static readonly Block Hive = new Block(225);
        public static readonly Block Honey = new Block(-4);
        public static readonly Block HoneyBlock = new Block(229);
        public static readonly Block Honeyfall = new Block(345);
        public static readonly Block HoneyPlatform = new Block(19, -1, 432);
        public static readonly Block IceBlock = new Block(161);
        public static readonly Block IceBrick = new Block(206);
        public static readonly Block IceRod = new Block(127);
        public static readonly Block InactiveStone = new Block(131);
        public static readonly Block IridescentBrick = new Block(119);
        public static readonly Block IronOre = new Block(6);
        public static readonly Block JungleGrass = new Block(60);
        public static readonly Block JungleThorn = new Block(69);
        public static readonly Block Lava = new Block(-3);
        public static readonly Block Lavafall = new Block(327);
        public static readonly Block LavaMoss = new Block(381);
        public static readonly Block LeadOre = new Block(167);
        public static readonly Block Leaf = new Block(192);
        public static readonly Block LihzahrdBrick = new Block(226);
        public static readonly Block LivingCursedFire = new Block(340);
        public static readonly Block LivingDemonFire = new Block(341);
        public static readonly Block LivingFire = new Block(336);
        public static readonly Block LivingFrostFire = new Block(342);
        public static readonly Block LivingIchorFire = new Block(343);
        public static readonly Block LivingMahogany = new Block(383);
        public static readonly Block LivingMahoganyLeaf = new Block(384);
        public static readonly Block LivingUltrabrightFire = new Block(344);
        public static readonly Block LivingWood = new Block(191);
        public static readonly Block LivingWoodPlatform = new Block(19, -1, 414);
        public static readonly Block Luminite = new Block(408);
        public static readonly Block LuminiteBrick = new Block(409);
        public static readonly Block Marble = new Block(367);
        public static readonly Block MartianConduitPlating = new Block(350);
        public static readonly Block MetalShelf = new Block(19, -1, 162);
        public static readonly Block Meteorite = new Block(37);
        public static readonly Block MeteoriteBrick = new Block(370);
        public static readonly Block MidnightConfetti = new Block(329);
        public static readonly Block MoonglowPlanter = new Block(380, -1, 18);
        public static readonly Block Mud = new Block(59);
        public static readonly Block MudstoneBrick = new Block(120);
        public static readonly Block MushroomGrass = new Block(70);
        public static readonly Block MushroomPlatform = new Block(19, -1, 324);
        public static readonly Block MythrilBrick = new Block(122);
        public static readonly Block MythrilOre = new Block(108);
        public static readonly Block NebulaFragment = new Block(417);
        public static readonly Block Obsidian = new Block(56);
        public static readonly Block ObsidianBrick = new Block(75);
        public static readonly Block ObsidianPlatform = new Block(19, -1, 234);
        public static readonly Block OrichalcumOre = new Block(222);
        public static readonly Block PalladiumColumn = new Block(248);
        public static readonly Block PalladiumOre = new Block(221);
        public static readonly Block PalmWood = new Block(322);
        public static readonly Block PalmWoodPlatform = new Block(19, -1, 306);
        public static readonly Block Pearlsand = new Block(116);
        public static readonly Block Pearlsandstone = new Block(403);
        public static readonly Block Pearlstone = new Block(117);
        public static readonly Block PearlstoneBrick = new Block(118);
        public static readonly Block Pearlwood = new Block(159);
        public static readonly Block PearlwoodPlatform = new Block(19, -1, 54);
        public static readonly Block PineTree = new Block(170);
        public static readonly Block PinkBrick = new Block(44);
        public static readonly Block PinkBrickPlatform = new Block(19, -1, 126);
        public static readonly Block PinkIce = new Block(164);
        public static readonly Block PinkSlime = new Block(371);
        public static readonly Block PinkTeam = new Block(433);
        public static readonly Block PinkTeamPlatform = new Block(438);
        public static readonly Block PlatinumBrick = new Block(177);
        public static readonly Block PlatinumOre = new Block(169);
        public static readonly Block Pumpkin = new Block(251);
        public static readonly Block PumpkinPlatform = new Block(19, -1, 270);
        public static readonly Block PurpleIce = new Block(163);
        public static readonly Block PurpleMoss = new Block(183);
        public static readonly Block RainbowBrick = new Block(160);
        public static readonly Block RainCloud = new Block(196);
        public static readonly Block RedBrick = new Block(39);
        public static readonly Block RedDynastyShingles = new Block(312);
        public static readonly Block RedIce = new Block(200);
        public static readonly Block RedMoss = new Block(181);
        public static readonly Block RedStucco = new Block(153);
        public static readonly Block RedTeam = new Block(426);
        public static readonly Block RedTeamPlatform = new Block(427);
        public static readonly Block RichMahogany = new Block(158);
        public static readonly Block RichMahoganyPlatform = new Block(19, -1, 36);
        public static readonly Block Rope = new Block(213);
        public static readonly Block RubyGemspark = new Block(266);
        public static readonly Block RubyGemsparkOff = new Block(259);
        public static readonly Block RubyOre = new Block(64);
        public static readonly Block Sand = new Block(53);
        public static readonly Block Sandfall = new Block(458);
        public static readonly Block Sandstone = new Block(396);
        public static readonly Block SandstoneBrick = new Block(151);
        public static readonly Block SandstoneSlab = new Block(274);
        public static readonly Block SapphireGemspark = new Block(264);
        public static readonly Block SapphireGemsparkOff = new Block(257);
        public static readonly Block SapphireOre = new Block(63);
        public static readonly Block Shadewood = new Block(208);
        public static readonly Block ShadewoodPlatform = new Block(19, -1, 90);
        public static readonly Block ShiverthornPlanter = new Block(380, -1, 54);
        public static readonly Block ShroomitePlating = new Block(348);
        public static readonly Block SilkRope = new Block(365);
        public static readonly Block Silt = new Block(123);
        public static readonly Block SilverBrick = new Block(46);
        public static readonly Block SilverOre = new Block(9);
        public static readonly Block SkywarePlatform = new Block(19, -1, 396);
        public static readonly Block Slime = new Block(193);
        public static readonly Block SlimePlatform = new Block(19, -1, 360);
        public static readonly Block Slush = new Block(224);
        public static readonly Block Smoke = new Block(351);
        public static readonly Block SmoothGranite = new Block(369);
        public static readonly Block SmoothMarble = new Block(357);
        public static readonly Block Snow = new Block(147);
        public static readonly Block SnowBrick = new Block(148);
        public static readonly Block SnowCloud = new Block(460);
        public static readonly Block Snowfall = new Block(459);
        public static readonly Block SolarFragment = new Block(415);
        public static readonly Block Spike = new Block(48);
        public static readonly Block SpookyPlatform = new Block(19, -1, 288);
        public static readonly Block SpookyWood = new Block(253);
        public static readonly Block StardustFragment = new Block(418);
        public static readonly Block SteampunkPlatform = new Block(19, -1, 378);
        public static readonly Block Stone = new Block(1);
        public static readonly Block StoneSlab = new Block(273);
        public static readonly Block SturdyFossil = new Block(407);
        public static readonly Block Sunplate = new Block(202);
        public static readonly Block ThinIce = new Block(162);
        public static readonly Block TinBrick = new Block(175);
        public static readonly Block TinOre = new Block(166);
        public static readonly Block TinPlating = new Block(325);
        public static readonly Block TitaniumOre = new Block(223);
        public static readonly Block Titanstone = new Block(250);
        public static readonly Block TopazGemspark = new Block(263);
        public static readonly Block TopazGemsparkOff = new Block(256);
        public static readonly Block TopazOre = new Block(66);
        public static readonly Block TungstenBrick = new Block(176);
        public static readonly Block TungstenOre = new Block(168);
        public static readonly Block VineRope = new Block(353);
        public static readonly Block VortexFragment = new Block(416);
        public static readonly Block Water = new Block(-2);
        public static readonly Block Waterfall = new Block(326);
        public static readonly Block WaterleafPlanter = new Block(380, -1, 72);
        public static readonly Block WebRope = new Block(366);
        public static readonly Block WhiteTeam = new Block(434);
        public static readonly Block WhiteTeamPlatform = new Block(439);
        public static readonly Block WireBulb = new Block(429);
        public static readonly Block Wood = new Block(30);
        public static readonly Block WoodenBeam = new Block(124);
        public static readonly Block WoodenSpike = new Block(232);
        public static readonly Block WoodPlatform = new Block(19);
        public static readonly Block WoodShelf = new Block(19, -1, 198);
        public static readonly Block YellowMoss = new Block(180);
        public static readonly Block YellowStucco = new Block(154);
        public static readonly Block YellowTeam = new Block(432);
        public static readonly Block YellowTeamPlatform = new Block(437);

        /// <summary>
        /// Initializes a new instance of the <see cref="Block" /> class with the specified type and frames.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="frameX">The X frame, or -1 for none.</param>
        /// <param name="frameY">The Y frame, or -1 for none.</param>
        public Block(int type, short frameX = -1, short frameY = -1)
        {
            Type = type;
            FrameX = frameX;
            FrameY = frameY;
        }

        /// <summary>
        /// Gets the X frame.
        /// </summary>
        public short FrameX { get; }

        /// <summary>
        /// Gets the Y frame.
        /// </summary>
        public short FrameY { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Parses the specified string into a block.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Block> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (ushort.TryParse(s, out var type))
            {
                return ParsingResult.From(new Block(type));
            }

            var split = s.Split(':');
            if (split.Length == 3 && ushort.TryParse(split[0], out type) &&
                short.TryParse(split[1], out var frameX) && short.TryParse(split[2], out var frameY))
            {
                return ParsingResult.From(new Block(type, frameX, frameY));
            }

            var field = typeof(Block).GetField(s.ToPascalCase());
            return field != null
                ? ParsingResult.From((Block)field.GetValue(null))
                : ParsingResult.FromError<Block>($"Invalid block '{s}'.");
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            if (-4 <= Type && Type <= -2)
            {
                tile.Liquid = 255;
                tile.LiquidType = -Type - 2;
            }
            else
            {
                tile.Liquid = 0;
                tile.LiquidType = 0;
            }

            tile.Active = Type >= 0;
            tile.FrameX = FrameX;
            tile.FrameY = FrameY;
            tile.Type = (ushort)Math.Max(0, Type);
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            if (-4 <= Type && Type <= -2)
            {
                return tile.Liquid > 0 && tile.LiquidType == -Type - 2;
            }
            if (Type == -1)
            {
                return !tile.Active;
            }

            var doFramesMatch = (FrameX == -1 || tile.FrameX == FrameX) && (FrameY == -1 || tile.FrameY == FrameY);
            return tile.Active && tile.Type == Type && doFramesMatch;
        }
    }
}
