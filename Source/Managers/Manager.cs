﻿using System.Collections.Generic;
using Verse;
using RimWorld;
using System;

namespace BetterPawnControl
{
	abstract class Manager<T>
	{
		internal static List<Policy> policies = new List<Policy>();
		internal static List<MapActivePolicy> activePolicies =
			new List<MapActivePolicy>();
		internal static List<T> links = new List<T>();

        static Manager()
        {
            Policy defaultPolicy = new Policy(
                policies.Count, "BPC.Auto".Translate());
            policies.Add(defaultPolicy);
            activePolicies.Add(new MapActivePolicy(0, defaultPolicy));
        }

        internal static void ForceInit()
        {
            policies = new List<Policy>();
            activePolicies = new List<MapActivePolicy>();
            links = new List<T>();
            Policy defaultPolicy = new Policy(
                policies.Count, "BPC.Auto".Translate());
            policies.Add(defaultPolicy);
            activePolicies.Add(new MapActivePolicy(0, defaultPolicy));
        }

        private static bool _dirtyPolicy = false;
		public static bool DirtyPolicy
		{
			get
			{
				return _dirtyPolicy;
			}

			set
			{
				_dirtyPolicy = value;
			}
		}

		internal static Policy GetActivePolicy()
		{
			return GetActivePolicy(Find.CurrentMap.uniqueID);
		}

		internal static void SetActivePolicy(Policy policy)
		{
			SetActivePolicy(Find.CurrentMap.uniqueID, policy);
		}

		internal static Policy GetActivePolicy(int mapId)
		{
			MapActivePolicy mapPolicy = activePolicies.Find(
				x => x.mapId == mapId);
			if (mapPolicy == null)
			{
				//new map!create default
				mapPolicy = new MapActivePolicy(mapId, policies[0]);
				activePolicies.Add(mapPolicy);
			}
			return mapPolicy.activePolicy;
		}

		internal static Policy GetPolicy(int selected)
		{
			return policies.Find(x => x.id == selected);
		}

		internal static void SetActivePolicy(int mapId, Policy policy)
		{
			MapActivePolicy map = activePolicies.Find(x => x.mapId == mapId);
			if (map != null)
			{
				map.activePolicy = policy;
			}
			else
			{
				activePolicies.Add(new MapActivePolicy(mapId, policy));
			}
		}

		internal static void MoveLinksToMap(int dstMap)
		{
			foreach (T link in links)
			{
				if (link.GetType() == typeof(WorkLink))
				{
					link.ChangeType<WorkLink>().mapId = dstMap;
				}

				if (link.GetType() == typeof(RestrictLink))
				{
					link.ChangeType<RestrictLink>().mapId = dstMap;
				}

				if (link.GetType() == typeof(AssignLink))
				{
					link.ChangeType<AssignLink>().mapId = dstMap;
				}

				if (link.GetType() == typeof(AnimalLink))
				{
					link.ChangeType<AnimalLink>().mapId = dstMap;
				}
			}
		}
	}
}

