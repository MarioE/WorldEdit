using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a wall type.
    /// </summary>
    public class Wall : ITemplate
    {
        public static readonly Wall AdamantiteBeam = new Wall(32);
        public static readonly Wall Air = new Wall(0);
        public static readonly Wall AmberGemspark = new Wall(153);
        public static readonly Wall AmberGemsparkOff = new Wall(157);
        public static readonly Wall AmethystGemspark = new Wall(154);
        public static readonly Wall AmethystGemsparkOff = new Wall(158);
        public static readonly Wall ArcaneRune = new Wall(144);
        public static readonly Wall BlueBrick = new Wall(17);
        public static readonly Wall BlueDynasty = new Wall(143);
        public static readonly Wall BluegreenWallpaper = new Wall(124);
        public static readonly Wall BlueSlab = new Wall(100);
        public static readonly Wall BlueStainedGlass = new Wall(90);
        public static readonly Wall BlueTiled = new Wall(101);
        public static readonly Wall BoneBlock = new Wall(75);
        public static readonly Wall BorealWood = new Wall(149);
        public static readonly Wall BorealWoodFence = new Wall(150);
        public static readonly Wall BubblegumBlock = new Wall(110);
        public static readonly Wall BubbleWallpaper = new Wall(133);
        public static readonly Wall Cactus = new Wall(72);
        public static readonly Wall CandyCane = new Wall(29);
        public static readonly Wall CandyCaneWallpaper = new Wall(118);
        public static readonly Wall ChlorophyteBrick = new Wall(173);
        public static readonly Wall ChristmasTreeWallpaper = new Wall(116);
        public static readonly Wall Cloud = new Wall(73);
        public static readonly Wall CobaltBrick = new Wall(25);
        public static readonly Wall Cog = new Wall(225);
        public static readonly Wall Confetti = new Wall(168);
        public static readonly Wall CopperBrick = new Wall(12);
        public static readonly Wall CopperPipeWallpaper = new Wall(134);
        public static readonly Wall CopperPlating = new Wall(146);
        public static readonly Wall CrimtaneBrick = new Wall(174);
        public static readonly Wall CrystalBlock = new Wall(186);
        public static readonly Wall DemoniteBrick = new Wall(33);
        public static readonly Wall DesertFossil = new Wall(223);
        public static readonly Wall DiamondGemspark = new Wall(155);
        public static readonly Wall DiamondGemsparkOff = new Wall(159);
        public static readonly Wall Dirt = new Wall(16);
        public static readonly Wall Disc = new Wall(82);
        public static readonly Wall DuckyWallpaper = new Wall(135);
        public static readonly Wall EbonstoneBrick = new Wall(35);
        public static readonly Wall Ebonwood = new Wall(41);
        public static readonly Wall EbonwoodFence = new Wall(138);
        public static readonly Wall EmeraldGemspark = new Wall(156);
        public static readonly Wall EmeraldGemsparkOff = new Wall(160);
        public static readonly Wall FancyGrayWallpaper = new Wall(126);
        public static readonly Wall FestiveWallpaper = new Wall(119);
        public static readonly Wall FleshBlock = new Wall(77);
        public static readonly Wall Flower = new Wall(68);
        public static readonly Wall Glass = new Wall(21);
        public static readonly Wall GoldBrick = new Wall(10);
        public static readonly Wall Granite = new Wall(184);
        public static readonly Wall Grass = new Wall(66);
        public static readonly Wall GrayBrick = new Wall(5);
        public static readonly Wall GrayStucco = new Wall(39);
        public static readonly Wall GreenBrick = new Wall(18);
        public static readonly Wall GreenCandyCane = new Wall(30);
        public static readonly Wall GreenSlab = new Wall(104);
        public static readonly Wall GreenStainedGlass = new Wall(91);
        public static readonly Wall GreenStucco = new Wall(38);
        public static readonly Wall GreenTiled = new Wall(105);
        public static readonly Wall GrinchFingerWallpaper = new Wall(125);
        public static readonly Wall Hay = new Wall(114);
        public static readonly Wall HellstoneBrick = new Wall(177);
        public static readonly Wall Hive = new Wall(108);
        public static readonly Wall Honeyfall = new Wall(172);
        public static readonly Wall IceBrick = new Wall(84);
        public static readonly Wall IceFloeWallpaper = new Wall(127);
        public static readonly Wall IridescentBrick = new Wall(23);
        public static readonly Wall IronFence = new Wall(145);
        public static readonly Wall Jungle = new Wall(67);
        public static readonly Wall KrampusHornWallpaper = new Wall(123);
        public static readonly Wall Lavafall = new Wall(137);
        public static readonly Wall LeadFence = new Wall(107);
        public static readonly Wall LihzahrdBrick = new Wall(112);
        public static readonly Wall LivingWood = new Wall(78);
        public static readonly Wall LuminiteBrick = new Wall(224);
        public static readonly Wall Marble = new Wall(183);
        public static readonly Wall MartianConduit = new Wall(176);
        public static readonly Wall MeteoriteBrick = new Wall(182);
        public static readonly Wall MidnightConfetti = new Wall(169);
        public static readonly Wall MudstoneBrick = new Wall(24);
        public static readonly Wall MulticoloredStainedGlass = new Wall(93);
        public static readonly Wall Mushroom = new Wall(74);
        public static readonly Wall MusicWallpaper = new Wall(128);
        public static readonly Wall MythrilBrick = new Wall(26);
        public static readonly Wall ObsidianBrick = new Wall(20);
        public static readonly Wall OrnamentWallpaper = new Wall(117);
        public static readonly Wall PalladiumColumn = new Wall(109);
        public static readonly Wall PalmWood = new Wall(151);
        public static readonly Wall PalmWoodFence = new Wall(152);
        public static readonly Wall PearlstoneBrick = new Wall(22);
        public static readonly Wall Pearlwood = new Wall(43);
        public static readonly Wall PearlwoodFence = new Wall(140);
        public static readonly Wall PinkBrick = new Wall(19);
        public static readonly Wall PinkSlab = new Wall(102);
        public static readonly Wall PinkTiled = new Wall(103);
        public static readonly Wall Planked = new Wall(27);
        public static readonly Wall PlatinumBrick = new Wall(47);
        public static readonly Wall Pumpkin = new Wall(113);
        public static readonly Wall PurpleRainWallpaper = new Wall(129);
        public static readonly Wall PurpleStainedGlass = new Wall(88);
        public static readonly Wall RainbowBrick = new Wall(44);
        public static readonly Wall RainbowWallpaper = new Wall(130);
        public static readonly Wall RedBrick = new Wall(6);
        public static readonly Wall RedStainedGlass = new Wall(92);
        public static readonly Wall RedStucco = new Wall(36);
        public static readonly Wall RichMahogany = new Wall(42);
        public static readonly Wall RichMahoganyFence = new Wall(139);
        public static readonly Wall RubyGemspark = new Wall(164);
        public static readonly Wall RubyGemsparkOff = new Wall(161);
        public static readonly Wall Sail = new Wall(148);
        public static readonly Wall Sandfall = new Wall(226);
        public static readonly Wall Sandstone = new Wall(187);
        public static readonly Wall SandstoneBrick = new Wall(34);
        public static readonly Wall SapphireGemspark = new Wall(165);
        public static readonly Wall SapphireGemsparkOff = new Wall(162);
        public static readonly Wall Shadewood = new Wall(85);
        public static readonly Wall ShadewoodFence = new Wall(141);
        public static readonly Wall ShroomitePlating = new Wall(175);
        public static readonly Wall SillyGreenBalloon = new Wall(230);
        public static readonly Wall SillyPinkBalloon = new Wall(228);
        public static readonly Wall SillyPurpleBalloon = new Wall(229);
        public static readonly Wall SilverBrick = new Wall(11);
        public static readonly Wall SlimeBlock = new Wall(76);
        public static readonly Wall SmoothGranite = new Wall(181);
        public static readonly Wall SmoothMarble = new Wall(179);
        public static readonly Wall SnowBrick = new Wall(31);
        public static readonly Wall Snowfall = new Wall(227);
        public static readonly Wall SnowflakeWallpaper = new Wall(122);
        public static readonly Wall SparkleStoneWallpaper = new Wall(131);
        public static readonly Wall SpookyWood = new Wall(115);
        public static readonly Wall SquigglesWallpaper = new Wall(121);
        public static readonly Wall StarlitHeavenWallpaper = new Wall(132);
        public static readonly Wall StarsWallpaper = new Wall(120);
        public static readonly Wall Stone = new Wall(1);
        public static readonly Wall StoneSlab = new Wall(147);
        public static readonly Wall TinBrick = new Wall(45);
        public static readonly Wall TinPlating = new Wall(167);
        public static readonly Wall TitanstoneBlock = new Wall(111);
        public static readonly Wall TopazGemspark = new Wall(166);
        public static readonly Wall TopazGemsparkOff = new Wall(163);
        public static readonly Wall TungstenBrick = new Wall(46);
        public static readonly Wall Waterfall = new Wall(136);
        public static readonly Wall WhiteDynasty = new Wall(142);
        public static readonly Wall Wood = new Wall(4);
        public static readonly Wall WoodenFence = new Wall(106);
        public static readonly Wall YellowStainedGlass = new Wall(89);
        public static readonly Wall YellowStucco = new Wall(37);

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall" /> class with the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public Wall(byte type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// Parses the specified string into a wall.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Wall> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (byte.TryParse(s, out var type))
            {
                return ParsingResult.From(new Wall(type));
            }

            var field = typeof(Wall).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            return field != null
                ? ParsingResult.From((Wall)field.GetValue(null))
                : ParsingResult.FromError<Wall>($"Invalid wall '{s}'.");
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.Wall = Type;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile) => tile.Wall == Type;
    }
}
