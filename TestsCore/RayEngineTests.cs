﻿using NUnit.Framework;
using RadianceStandard.Primitives;
using RadianceStandard.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestsCore
{
    [TestFixture]
    public class RayEngineTests
    {
        #region Crossing Point
        static private IEnumerable<(Ray, Ray, Vector)> CrossingPointSource
        {
            get
            {
                var r1 = new Ray(new Vector(2, 4), new Vector(11, 2));
                var r2 = new Ray(new Vector(10, 1), new Vector(12, 3));
                var r3 = new Ray(new Vector(1, 3), new Vector(10, 1));
                yield return (r1, r2, new Vector(11, 2));
                yield return (r1, r3, null);
            }
        }

        [TestCaseSource(nameof(CrossingPointSource))]
        public void TestCrossingPoint((Ray r1, Ray r2, Vector expected) bundle)
        {
            var (r1, r2, expected) = bundle;
            var engine = new RayEngine();
            if (expected != null)
            {
                var success = engine.TryFindCrossingPoint(r1, r2, out Vector point);
                Assert.IsTrue(success && expected == point);
            }
            else
            {
                Assert.IsFalse(engine.TryFindCrossingPoint(r1, r2, out Vector point));
            }
        }
        #endregion

        #region Crossing Params
        static private IEnumerable<(Ray ray, Ray blocking, float t1, float t2)> CrossingParamsSource
        {
            get
            {
                var baseRay1 = new Ray(new Vector(2, 4), new Vector(13, 2));
                var blockingRay1 = new Ray(new Vector(12, 1), new Vector(14, 3));
                var blockingRay2 = new Ray(new Vector(23, -1), new Vector(26, 2));
                yield return (baseRay1, blockingRay1, 1f, .5f);
                yield return (baseRay1, blockingRay2, 2f, 1f / 3);
                var baseRay2 = new Ray(new Vector(2, 4), new Vector(7.5f, 3));
                yield return (baseRay2, blockingRay1, 2f, .5f);
                yield return (baseRay2, blockingRay2, 4f, 1f / 3);
            }
        }

        [TestCaseSource(nameof(CrossingParamsSource))]
        public void TestCrossingParams((Ray ray, Ray blocking, float t1, float t2) bundle)
        {
            var (ray, blocking, expected1, expected2) = bundle;
            var engine = new RayEngine();
            var received = engine.FindCrossingParams(ray, blocking);
            if (received.HasValue)
            {
                var (received1, received2) = received.Value;
                Assert.IsTrue(received1 == expected1 && received2 == expected2);
            }
            else
            {
                Assert.Fail();
            }
        }
        #endregion
    }
}
