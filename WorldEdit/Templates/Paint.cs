using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a paint type.
    /// </summary>
    public class Paint : ITemplate
    {
        public static readonly Paint Black = new Paint(25, false);
        public static readonly Paint BlackWall = new Paint(25, true);
        public static readonly Paint Blank = new Paint(0, false);
        public static readonly Paint BlankWall = new Paint(0, true);
        public static readonly Paint Blue = new Paint(9, false);
        public static readonly Paint BlueWall = new Paint(9, true);
        public static readonly Paint Brown = new Paint(28, false);
        public static readonly Paint BrownWall = new Paint(28, true);
        public static readonly Paint Cyan = new Paint(7, false);
        public static readonly Paint CyanWall = new Paint(7, true);
        public static readonly Paint DeepBlue = new Paint(21, false);
        public static readonly Paint DeepBlueWall = new Paint(21, true);
        public static readonly Paint DeepCyan = new Paint(19, false);
        public static readonly Paint DeepCyanWall = new Paint(19, true);
        public static readonly Paint DeepGreen = new Paint(17, false);
        public static readonly Paint DeepGreenWall = new Paint(17, true);
        public static readonly Paint DeepLime = new Paint(16, false);
        public static readonly Paint DeepLimeWall = new Paint(16, true);
        public static readonly Paint DeepOrange = new Paint(14, false);
        public static readonly Paint DeepOrangeWall = new Paint(14, true);
        public static readonly Paint DeepPink = new Paint(24, false);
        public static readonly Paint DeepPinkWall = new Paint(24, true);
        public static readonly Paint DeepPurple = new Paint(22, false);
        public static readonly Paint DeepPurpleWall = new Paint(22, true);
        public static readonly Paint DeepRed = new Paint(13, false);
        public static readonly Paint DeepRedWall = new Paint(13, true);
        public static readonly Paint DeepSkyBlue = new Paint(20, false);
        public static readonly Paint DeepSkyBlueWall = new Paint(20, true);
        public static readonly Paint DeepTeal = new Paint(18, false);
        public static readonly Paint DeepTealWall = new Paint(18, true);
        public static readonly Paint DeepViolet = new Paint(23, false);
        public static readonly Paint DeepVioletWall = new Paint(23, true);
        public static readonly Paint DeepYellow = new Paint(15, false);
        public static readonly Paint DeepYellowWall = new Paint(15, true);
        public static readonly Paint Gray = new Paint(27, false);
        public static readonly Paint GrayWall = new Paint(27, true);
        public static readonly Paint Green = new Paint(5, false);
        public static readonly Paint GreenWall = new Paint(5, true);
        public static readonly Paint Lime = new Paint(4, false);
        public static readonly Paint LimeWall = new Paint(4, true);
        public static readonly Paint Negative = new Paint(30, false);
        public static readonly Paint NegativeWall = new Paint(30, true);
        public static readonly Paint Orange = new Paint(2, false);
        public static readonly Paint OrangeWall = new Paint(2, true);
        public static readonly Paint Pink = new Paint(12, false);
        public static readonly Paint PinkWall = new Paint(12, true);
        public static readonly Paint Purple = new Paint(10, false);
        public static readonly Paint PurpleWall = new Paint(10, true);
        public static readonly Paint Red = new Paint(1, false);
        public static readonly Paint RedWall = new Paint(1, true);
        public static readonly Paint Shadow = new Paint(29, false);
        public static readonly Paint ShadowWall = new Paint(29, true);
        public static readonly Paint SkyBlue = new Paint(8, false);
        public static readonly Paint SkyBlueWall = new Paint(8, true);
        public static readonly Paint Teal = new Paint(6, false);
        public static readonly Paint TealWall = new Paint(6, true);
        public static readonly Paint Violet = new Paint(11, false);
        public static readonly Paint VioletWall = new Paint(11, true);
        public static readonly Paint White = new Paint(26, false);
        public static readonly Paint WhiteWall = new Paint(26, true);
        public static readonly Paint Yellow = new Paint(3, false);
        public static readonly Paint YellowWall = new Paint(3, true);

        /// <summary>
        /// Initializes a new instance of the <see cref="Paint" /> class with the specified type and target.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="targetWalls"><c>true</c> to target walls; otherwise, <c>false</c>.</param>
        public Paint(byte type, bool targetWalls)
        {
            Type = type;
            TargetWalls = targetWalls;
        }

        /// <summary>
        /// Gets a value indicating whether to target walls.
        /// </summary>
        public bool TargetWalls { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// Tries to parse the specified string into a paint.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="paint">The resulting paint.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static bool TryParse(string s, out Paint paint)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Paint).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (field != null)
            {
                paint = (Paint)field.GetValue(null);
                return true;
            }

            paint = null;
            return false;
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            if (TargetWalls)
            {
                tile.WallColor = Type;
            }
            else
            {
                tile.Color = Type;
            }
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            return (TargetWalls ? tile.WallColor : tile.Color) == Type;
        }
    }
}
