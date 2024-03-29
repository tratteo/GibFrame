using GibFrame.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.PathBuilder
{
    [ExecuteAlways]
    public class Waypoint : MonoBehaviour, IEquatable<Waypoint>
    {
        [SerializeField] private List<PathingOption> options;

        public Vector3 Position { get => transform.position; }

        public void AddOption(Waypoint point)
        {
            PathingOption opt = new PathingOption(point, 1F);
            if (!options.Contains(opt))
            {
                options.Add(opt);
            }
        }

        public bool RemoveOption(Waypoint point)
        {
            PathingOption opt = new PathingOption(point);
            return options.Remove(opt);
        }

        public bool Equals(Waypoint other)
        {
            return this.name == other.name;
        }

        public bool Reached(Vector3 pos, float threshold)
        {
            return Vector3.Distance(pos, transform.position) <= threshold;
        }

        public Waypoint GetNextOption()
        {
            if (options == null)
            {
                return null;
            }
            if (options != null && options.Count <= 0)
            {
                return null;
            }
            return options.SelectWithProbability().Point;
        }

        private void OnDestroy()
        {
        }

        private void Awake()
        {
            if (options is null)
            {
                options = new List<PathingOption>();
            }
            options.NormalizeProbabilities();
        }

        private void OnDrawGizmos()
        {
            foreach (PathingOption w in options)
            {
                GDebug.GDebug.DrawArrow(transform.position, w.Point.transform.position - transform.position, Color.green);
                //Gizmos.DrawLine(transform.position, w.transform.position);
            }
        }
    }
}