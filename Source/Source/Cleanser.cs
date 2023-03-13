using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace SaleOfGoods
{
    internal class Cleanser
    {
        public static void RemoveSnow(Map map, bool homearea)
        {
            bool flag = map == null || map.snowGrid == null || map.mapDrawer == null;
            if (!flag)
            {
                if (homearea)
                {
                    SnowGrid snowGrid = map.snowGrid;
                    using (IEnumerator<IntVec3> enumerator = map.areaManager.Home.ActiveCells.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            IntVec3 intVec = enumerator.Current;
                            snowGrid.SetDepth(intVec, 0f);
                        }
                        return;
                    }
                }
                map.snowGrid = new SnowGrid(map);
                map.mapDrawer.WholeMapChanged(MapMeshFlag.Snow);
                map.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
                map.pathing.RecalculateAllPerceivedPathCosts();
            }
        }
        public static int RemoveFilth(Map map, bool homearea)
        {
            bool flag = map == null || map.listerFilthInHomeArea == null || map.listerThings == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                int num = 0;
                List<Thing> list;
                if (homearea)
                {
                    list = map.listerFilthInHomeArea.FilthInHomeArea;
                }
                else
                {
                    list = map.listerThings.ThingsInGroup(ThingRequestGroup.Filth);
                }
                num += list.Count;
                for (int i = list.Count - 1; i > -1; i--)
                {
                    Thing bob = list[i];
                    Filth filth = (Filth)list[i];
                    filth.DeSpawn(DestroyMode.Vanish);
                    if (!filth.Destroyed)
                    {
                        filth.Destroy(DestroyMode.Vanish);
                    }
                    if (!filth.Discarded)
                    {
                        Log.Warning("A thing_filth_object destroyed before is not discarded!That's wierd.");
                        filth.Discard(false);
                    }
                }
                result = num;
            }
            return result;
        }
    }
}
