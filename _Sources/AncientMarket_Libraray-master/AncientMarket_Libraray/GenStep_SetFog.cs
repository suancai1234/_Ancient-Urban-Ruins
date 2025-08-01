﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Verse;

namespace AncientMarket_Libraray
{
    public class GenStep_SetFog : GenStep
    {
        public override int SeedPart => 546516544;

        public override void Generate(Map map, GenStepParams parms)
        {
            CellIndices cellIndices = map.cellIndices;
            GenStep_SetFog.FogMap(map);
            if (Current.ProgramState == ProgramState.Playing)
            {
                map.roofGrid.Drawer.SetDirty();
            }
        }
        public static void FogMap(Map map)
        {
            map.fogGrid.Refog(CellRect.WholeMap(map));
            if (Current.ProgramState == ProgramState.Playing)
            {
                map.roofGrid.Drawer.SetDirty();
            }
        }
    }
}