﻿using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    public class MockRegionSelector : RegionSelector
    {
        protected override Region GetRegion()
        {
            return new NullRegion();
        }
    }
}