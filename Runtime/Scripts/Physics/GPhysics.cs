// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : com.tratteo.gibframe.Packages.com.tratteo.gibframe.Runtime.Scripts.Physics : GPhysics.cs
//
// All Rights Reserved

using System;
using GibFrame.Utils;
using GibFrame.Utils.Mathematics;
using UnityEngine;

namespace GibFrame.Physic
{
    public static class GPhysics
    {
        public static Collider[] MatchingOverlapSphere(Vector3 position, float radius, Collider[] colliders, int layerMask, QueryTriggerInteraction queryTriggerInteraction, params Predicate<Collider>[] predicates)
        {
            Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask, queryTriggerInteraction);
            colliders = General.GetPredicatesMatchingObjects(colliders, predicates);
            return colliders;
        }

        public static Collider[] MatchingOverlapSphere(Vector3 position, float radius, int layerMask, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
            colliders = General.GetPredicatesMatchingObjects(colliders, predicates);
            return colliders;
        }

        public static Collider[] MatchingOverlapSphere(Vector3 position, float radius, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius);
            return General.GetPredicatesMatchingObjects(colliders, predicates);
        }

        public static Collider[] MatchingOverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, QueryTriggerInteraction queryTriggerInteraction, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);
            return General.GetPredicatesMatchingObjects(colliders, predicates);
        }

        public static Collider[] MatchingOverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask);
            return General.GetPredicatesMatchingObjects(colliders, predicates);
        }

        public static Collider[] MatchingOverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation);
            return General.GetPredicatesMatchingObjects(colliders, predicates);
        }

        public static Collider[] MatchingOverlapBox(Vector3 center, Vector3 halfExtents, params Predicate<Collider>[] predicates)
        {
            Collider[] colliders = Physics.OverlapBox(center, halfExtents);
            return General.GetPredicatesMatchingObjects(colliders, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layerMask, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, maxDistance, layerMask);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Vector3 origin, Vector3 direction, float maxDistance, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction, maxDistance);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Vector3 origin, Vector3 direction, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(origin, direction);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Ray ray, float maxDistance, int layerMask, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Ray ray, float maxDistance, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static RaycastHit[] MatchingRaycastAll(Ray ray, params Predicate<RaycastHit>[] predicates)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            return General.GetPredicatesMatchingObjects(hits, predicates);
        }

        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle, float gravityMagnitude)
        {
            float tan = Mathf.Tan(angle);
            float Dx = Vector3.Distance(origin, new Vector3(target.x, origin.y, target.z));
            float Dy = GMath.Abs(target.y - origin.y);
            float vi = (Dx / Mathf.Cos(angle)) * (Mathf.Sqrt(gravityMagnitude) / Mathf.Sqrt(2F * (Dx * tan + Dy)));
            Vector3 planeDir = (new Vector3(target.x, origin.y, target.z) - origin);
            return new Vector3(planeDir.x, planeDir.magnitude * tan, planeDir.z).normalized * vi * (1F + Time.fixedDeltaTime);
        }

        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle)
        {
            return CalculateThrowVelocity(origin, target, angle, Physics.gravity.magnitude);
        }
    }
}
