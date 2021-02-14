// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.PathBuilder : Path.cs
//
// All Rights Reserved

using System.Collections.Generic;
using GibFrame.Utils;
using UnityEngine;

namespace GibFrame.PathBuilder
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private List<PathingOption> startPoints;
        [SerializeField] private List<Waypoint> path;

        private GameObject waypointPrefab;

        public Waypoint GetRandomStartPoint()
        {
            return Utils.General.SelectWithProbability(startPoints).Point;
        }

        public void CreateWaypoint(Vector3 position)
        {
            GameObject waypoint = Instantiate(waypointPrefab, position, Quaternion.identity);
            waypoint.name = "Waypoint_" + path.Count + 1;
            if (!AddWaypoint(waypoint.GetComponent<Waypoint>()))
            {
                Destroy(waypoint);
            }
        }

        public bool AddWaypoint(Waypoint point)
        {
            if (path == null)
            {
                path = new List<Waypoint>();
            }
            if (!path.Contains(point))
            {
                path.Add(point);
                return true;
            }
            return false;
        }

        public void CreateStartPoint(Vector3 position, float prob)
        {
            GameObject waypoint = Instantiate(waypointPrefab, position, Quaternion.identity);
            waypoint.name = "StartPoint_" + startPoints.Count + 1;
            if (!AddStartPoint(waypoint.GetComponent<Waypoint>(), prob))
            {
                Destroy(waypoint);
            }
        }

        public bool AddStartPoint(Waypoint startPoint, float prob)
        {
            if (startPoints == null)
            {
                startPoints = new List<PathingOption>();
            }
            foreach (PathingOption opt in startPoints)
            {
                if (opt.Point.Equals(startPoint))
                {
                    return false;
                }
            }
            startPoints.Add(new PathingOption(startPoint, prob));
            return true;
        }

        public bool RemoveStartPoint(Waypoint startPoint)
        {
            foreach (Waypoint p in path)
            {
                p.RemoveOption(startPoint);
            }
            foreach (PathingOption p in startPoints)
            {
                p.Point.RemoveOption(startPoint);
            }
            foreach (PathingOption opt in startPoints)
            {
                if (opt.Point.Equals(startPoint))
                {
                    return startPoints.Remove(opt);
                }
            }

            return false;
        }

        public bool RemoveWaypoint(Waypoint point)
        {
            foreach (Waypoint p in path)
            {
                p.RemoveOption(point);
            }
            foreach (PathingOption p in startPoints)
            {
                p.Point.RemoveOption(point);
            }
            return path.Remove(point);
        }

        public void Clear()
        {
            if (startPoints != null)
            {
                foreach (PathingOption w in startPoints)
                {
                    if (w != null)
                    {
                        DestroyImmediate(w.Point.gameObject);
                    }
                }
                startPoints.Clear();
            }
            if (path != null)
            {
                foreach (Waypoint w in path)
                {
                    if (w != null)
                    {
                        DestroyImmediate(w.gameObject);
                    }
                }
                path.Clear();
            }
        }

        public Waypoint GetClosestWaypoint(Vector3 position)
        {
            return Utils.General.GetPredicateMinObject(path.ToArray(), w => Vector3.Distance(position, w.Position));
        }

        private void Awake()
        {
            Utils.General.NormalizeProbabilities(startPoints, s => s.ProvideSelectProbability());
            UnityUtils.ReadGameObject(out waypointPrefab, "TUtils/PathBuilder/Waypoint");
        }
    }
}