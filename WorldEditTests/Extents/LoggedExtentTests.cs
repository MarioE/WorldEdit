﻿using System;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Extents;
using WorldEdit.History;
using TTile = Terraria.Tile;

namespace WorldEditTests.Extents
{
    [TestFixture]
    public class LoggedExtentTests
    {
        [Test]
        public void Ctor_NullChangeSet_ThrowsArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LoggedExtent(null, new ChangeSet()));
        }

        [Test]
        public void Ctor_NullExtent_ThrowsArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LoggedExtent(new MockExtent(), null));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var loggedExtent = new LoggedExtent(new MockExtent {Tiles = tiles}, new ChangeSet());

            Assert.AreEqual(1, loggedExtent[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var loggedExtent = new LoggedExtent(extent, new ChangeSet());

            Assert.AreEqual(extent.LowerBound, loggedExtent.LowerBound);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(new MockExtent {Tiles = new ITile[20, 10]}, changeSet);

            loggedExtent[x, y] = new Tile {Wall = 1};

            var changes = changeSet.ToList();
            Assert.AreEqual(1, changes.Count);
            var change = (TileChange)changes[0];
            Assert.AreEqual(new Vector(x, y), change.Position);
            Assert.AreEqual(0, change.OldTile.Wall);
            Assert.AreEqual(1, change.NewTile.Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var loggedExtent = new LoggedExtent(extent, new ChangeSet());

            Assert.AreEqual(extent.UpperBound, loggedExtent.UpperBound);
        }
    }
}
