using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a wall type.
    /// </summary>
    [NoReorder]
    public sealed class WallType : ITemplate
    {
        /// <summary>
        ///     The maximum ID.
        /// </summary>
        public const byte MaxId = 231;

#pragma warning disable 1591
        public static readonly WallType Air = new WallType(0);
        public static readonly WallType Stone = new WallType(1);
        public static readonly WallType Wood = new WallType(4);
        public static readonly WallType GrayBrick = new WallType(5);
        public static readonly WallType RedBrick = new WallType(6);
        public static readonly WallType GoldBrick = new WallType(10);
        public static readonly WallType SilverBrick = new WallType(11);
        public static readonly WallType CopperBrick = new WallType(12);
        public static readonly WallType Dirt = new WallType(16);
        public static readonly WallType BlueBrick = new WallType(17);
        public static readonly WallType GreenBrick = new WallType(18);
        public static readonly WallType PinkBrick = new WallType(19);
        public static readonly WallType ObsidianBrick = new WallType(20);
        public static readonly WallType Glass = new WallType(21);
        public static readonly WallType PearlstoneBrick = new WallType(22);
        public static readonly WallType IridescentBrick = new WallType(23);
        public static readonly WallType MudstoneBrick = new WallType(24);
        public static readonly WallType CobaltBrick = new WallType(25);
        public static readonly WallType MythrilBrick = new WallType(26);
        public static readonly WallType Planked = new WallType(27);
        public static readonly WallType CandyCane = new WallType(29);
        public static readonly WallType GreenCandyCane = new WallType(30);
        public static readonly WallType SnowBrick = new WallType(31);
        public static readonly WallType AdamantiteBeam = new WallType(32);
        public static readonly WallType DemoniteBrick = new WallType(33);
        public static readonly WallType SandstoneBrick = new WallType(34);
        public static readonly WallType EbonstoneBrick = new WallType(35);
        public static readonly WallType RedStucco = new WallType(36);
        public static readonly WallType YellowStucco = new WallType(37);
        public static readonly WallType GreenStucco = new WallType(38);
        public static readonly WallType GrayStucco = new WallType(39);
        public static readonly WallType Ebonwood = new WallType(41);
        public static readonly WallType RichMahogany = new WallType(42);
        public static readonly WallType Pearlwood = new WallType(43);
        public static readonly WallType RainbowBrick = new WallType(44);
        public static readonly WallType TinBrick = new WallType(45);
        public static readonly WallType TungstenBrick = new WallType(46);
        public static readonly WallType PlatinumBrick = new WallType(47);
        public static readonly WallType Grass = new WallType(66);
        public static readonly WallType Jungle = new WallType(67);
        public static readonly WallType Flower = new WallType(68);
        public static readonly WallType Cactus = new WallType(72);
        public static readonly WallType Cloud = new WallType(73);
        public static readonly WallType Mushroom = new WallType(74);
        public static readonly WallType BoneBlock = new WallType(75);
        public static readonly WallType SlimeBlock = new WallType(76);
        public static readonly WallType FleshBlock = new WallType(77);
        public static readonly WallType LivingWood = new WallType(78);
        public static readonly WallType Disc = new WallType(82);
        public static readonly WallType IceBrick = new WallType(84);
        public static readonly WallType Shadewood = new WallType(85);
        public static readonly WallType PurpleStainedGlass = new WallType(88);
        public static readonly WallType YellowStainedGlass = new WallType(89);
        public static readonly WallType BlueStainedGlass = new WallType(90);
        public static readonly WallType GreenStainedGlass = new WallType(91);
        public static readonly WallType RedStainedGlass = new WallType(92);
        public static readonly WallType MulticoloredStainedGlass = new WallType(93);
        public static readonly WallType BlueSlab = new WallType(100);
        public static readonly WallType BlueTiled = new WallType(101);
        public static readonly WallType PinkSlab = new WallType(102);
        public static readonly WallType PinkTiled = new WallType(103);
        public static readonly WallType GreenSlab = new WallType(104);
        public static readonly WallType GreenTiled = new WallType(105);
        public static readonly WallType WoodenFence = new WallType(106);
        public static readonly WallType LeadFence = new WallType(107);
        public static readonly WallType Hive = new WallType(108);
        public static readonly WallType PalladiumColumn = new WallType(109);
        public static readonly WallType BubblegumBlock = new WallType(110);
        public static readonly WallType TitanstoneBlock = new WallType(111);
        public static readonly WallType LihzahrdBrick = new WallType(112);
        public static readonly WallType Pumpkin = new WallType(113);
        public static readonly WallType Hay = new WallType(114);
        public static readonly WallType SpookyWood = new WallType(115);
        public static readonly WallType ChristmasTreeWallpaper = new WallType(116);
        public static readonly WallType OrnamentWallpaper = new WallType(117);
        public static readonly WallType CandyCaneWallpaper = new WallType(118);
        public static readonly WallType FestiveWallpaper = new WallType(119);
        public static readonly WallType StarsWallpaper = new WallType(120);
        public static readonly WallType SquigglesWallpaper = new WallType(121);
        public static readonly WallType SnowflakeWallpaper = new WallType(122);
        public static readonly WallType KrampusHornWallpaper = new WallType(123);
        public static readonly WallType BluegreenWallpaper = new WallType(124);
        public static readonly WallType GrinchFingerWallpaper = new WallType(125);
        public static readonly WallType FancyGrayWallpaper = new WallType(126);
        public static readonly WallType IceFloeWallpaper = new WallType(127);
        public static readonly WallType MusicWallpaper = new WallType(128);
        public static readonly WallType PurpleRainWallpaper = new WallType(129);
        public static readonly WallType RainbowWallpaper = new WallType(130);
        public static readonly WallType SparkleStoneWallpaper = new WallType(131);
        public static readonly WallType StarlitHeavenWallpaper = new WallType(132);
        public static readonly WallType BubbleWallpaper = new WallType(133);
        public static readonly WallType CopperPipeWallpaper = new WallType(134);
        public static readonly WallType DuckyWallpaper = new WallType(135);
        public static readonly WallType Waterfall = new WallType(136);
        public static readonly WallType Lavafall = new WallType(137);
        public static readonly WallType EbonwoodFence = new WallType(138);
        public static readonly WallType RichMahoganyFence = new WallType(139);
        public static readonly WallType PearlwoodFence = new WallType(140);
        public static readonly WallType ShadewoodFence = new WallType(141);
        public static readonly WallType WhiteDynasty = new WallType(142);
        public static readonly WallType BlueDynasty = new WallType(143);
        public static readonly WallType ArcaneRune = new WallType(144);
        public static readonly WallType IronFence = new WallType(145);
        public static readonly WallType CopperPlating = new WallType(146);
        public static readonly WallType StoneSlab = new WallType(147);
        public static readonly WallType Sail = new WallType(148);
        public static readonly WallType BorealWood = new WallType(149);
        public static readonly WallType BorealWoodFence = new WallType(150);
        public static readonly WallType PalmWood = new WallType(151);
        public static readonly WallType PalmWoodFence = new WallType(152);
        public static readonly WallType AmberGemspark = new WallType(153);
        public static readonly WallType AmethystGemspark = new WallType(154);
        public static readonly WallType DiamondGemspark = new WallType(155);
        public static readonly WallType EmeraldGemspark = new WallType(156);
        public static readonly WallType AmberGemsparkOff = new WallType(157);
        public static readonly WallType AmethystGemsparkOff = new WallType(158);
        public static readonly WallType DiamondGemsparkOff = new WallType(159);
        public static readonly WallType EmeraldGemsparkOff = new WallType(160);
        public static readonly WallType RubyGemsparkOff = new WallType(161);
        public static readonly WallType SapphireGemsparkOff = new WallType(162);
        public static readonly WallType TopazGemsparkOff = new WallType(163);
        public static readonly WallType RubyGemspark = new WallType(164);
        public static readonly WallType SapphireGemspark = new WallType(165);
        public static readonly WallType TopazGemspark = new WallType(166);
        public static readonly WallType TinPlating = new WallType(167);
        public static readonly WallType Confetti = new WallType(168);
        public static readonly WallType MidnightConfetti = new WallType(169);
        public static readonly WallType Honeyfall = new WallType(172);
        public static readonly WallType ChlorophyteBrick = new WallType(173);
        public static readonly WallType CrimtaneBrick = new WallType(174);
        public static readonly WallType ShroomitePlating = new WallType(175);
        public static readonly WallType MartianConduit = new WallType(176);
        public static readonly WallType HellstoneBrick = new WallType(177);
        public static readonly WallType SmoothMarble = new WallType(179);
        public static readonly WallType SmoothGranite = new WallType(181);
        public static readonly WallType MeteoriteBrick = new WallType(182);
        public static readonly WallType Marble = new WallType(183);
        public static readonly WallType Granite = new WallType(184);
        public static readonly WallType CrystalBlock = new WallType(186);
        public static readonly WallType Sandstone = new WallType(187);
        public static readonly WallType DesertFossil = new WallType(223);
        public static readonly WallType LuminiteBrick = new WallType(224);
        public static readonly WallType Cog = new WallType(225);
        public static readonly WallType Sandfall = new WallType(226);
        public static readonly WallType Snowfall = new WallType(227);
        public static readonly WallType SillyPinkBalloon = new WallType(228);
        public static readonly WallType SillyPurpleBalloon = new WallType(229);
        public static readonly WallType SillyGreenBalloon = new WallType(230);
#pragma warning restore 1591

        private readonly byte _id;

        private WallType(byte id)
        {
            _id = id;
        }

        /// <summary>
        ///     Creates a new <see cref="WallType" /> instance with the specified ID.
        /// </summary>
        /// <param name="id">The ID, which must be less than <see cref="MaxId" />.</param>
        /// <returns>The wall type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id" /> is too large.</exception>
        [NotNull]
        public static WallType FromId(byte id)
        {
            if (id >= MaxId)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID is too large.");
            }

            return new WallType(id);
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.WallId = _id;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile) => tile.WallId == _id;
    }
}
