﻿using AncientMarket_Libraray;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AncientMarketAI_Libraray
{
    public class JobGiver_EnterAllowedLevel : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (AM_ModSetting.setting.enableAICrossLevel && pawn.IsColonist && GameComponent_AncientMarket.GetComp.GetSchedule(pawn) is LevelSchedule schedule && schedule.timeSchedule[GenLocalDate.HourOfDay(pawn.Map)])
            {
                if (pawn.Map.Parent is MapParent_Custom custom && !schedule.allowedLevels.Contains(custom.entrance) && this.FindBestExitForExit(pawn,schedule,pawn.Map) is MapPortal exit) 
                {
                    return JobMaker.MakeJob(JobDefOf.EnterPortal, exit);
                }
                if (pawn.mindState.lastJobTag == JobTag.Idle && 
                    this.FindBestExit(pawn, schedule, pawn.Map) is MapPortal portal) 
                {
                    return JobMaker.MakeJob(JobDefOf.EnterPortal, portal);
                }
            }
            return null;
        }
        public MapPortal FindBestExit(Pawn pawn, LevelSchedule s, Map map)
        {
            List<MapPortal> available = new List<MapPortal>();
            MapComponent_Submap.GetComp(map).Submaps.FindAll(m => m.entrance != null
            && pawn.CanReach(m.entrance, PathEndMode.Touch, Danger.Deadly
            ) && m.entrance.IsAvailable(pawn)).ForEach(m => available.Add(m.entrance));
            if (pawn.Map.Parent is MapParent_Custom custom && custom.Exit != null && custom.Exit.IsAvailable(pawn) && pawn.CanReach(custom.Exit, PathEndMode.Touch, Danger.Deadly))
            {
                available.Add(custom.Exit);

            }
            if (available.Any())
            {
                return available.RandomElement();
            }
            return null;
        }
        public MapPortal FindBestExitForExit(Pawn pawn,LevelSchedule s,Map map) 
        {
            List<MapPortal> available = new List<MapPortal>();
            MapComponent_Submap.GetComp(map).Submaps.FindAll(m => m.entrance != null && pawn.CanReach(m.entrance, PathEndMode.Touch, Danger.Deadly) && m.entrance.IsAllowed(pawn)).ForEach(m => available.Add(m.entrance));
            if (pawn.Map.Parent is MapParent_Custom custom && custom.Exit != null && pawn.CanReach(custom.Exit, PathEndMode.Touch, Danger.Deadly))
            {
                available.Add(custom.Exit);
            }
            if (available.Any()) 
            {
                return available.RandomElement();
            }
            return null;
        }
    }
}
