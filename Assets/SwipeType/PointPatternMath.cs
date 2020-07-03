using System;
using System.Collections.Generic;

namespace SwipeType.Example
{
    public static class PointPatternMath
    {
        public static Point[] GetInterpolatedPointArray(Point[] points, int segments)
        {
            // Create an empty return collection to store interpolated points
            var interpolatedPoints = new List<Point>(segments);

            // Precalculate desired segment length and define helper variables
            var desiredSegmentLength = GetPointArrayLength(points)/segments;
            var currentSegmentLength = 0d; // Initialize to zero

            // Add first point in point pattern to return array and save it for use in the interpolation process
            var lastTestPoint = points[0]; // Initialize to first point in point pattern
            interpolatedPoints.Add(lastTestPoint);

            // Enumerate points starting with second point (if any)
            for (var currentIndex = 1; currentIndex < points.Length; currentIndex++)
            {
                // Store current index point in helper variable
                var currentPoint = points[currentIndex];

                // Calculate distance between last added point and current point in point pattern
                // and use calculated length to calculate test segment length for next point to add
                var incrementToCurrentlength = GetDistance(lastTestPoint, currentPoint);
                var testSegmentLength = currentSegmentLength + incrementToCurrentlength;

                // Does the test segment length meet our desired length requirement
                if (testSegmentLength < desiredSegmentLength)
                {
                    // Desired segment length has not been satisfied so we don't need to add an interpolated point
                    // save this test point and move on to next test point
                    currentSegmentLength = testSegmentLength;
                    lastTestPoint = currentPoint;
                    continue;
                }

                // Test segment length has met or exceeded our desired segment length
                // so lets calculate how far we overshot our desired segment length and calculate
                // an interpolation position to use to derive our new interpolation point
                var interpolationPosition = (desiredSegmentLength - currentSegmentLength)*(1/incrementToCurrentlength);

                // Use interpolation position to derive our new interpolation point
                var interpolatedPoint = GetInterpolatedPoint(lastTestPoint, currentPoint, interpolationPosition);
                interpolatedPoints.Add(interpolatedPoint);

                // Sometimes rounding errors cause us to attempt to add more points than the user has requested.
                // If we've reached our segment count limit, exit loop
                if (interpolatedPoints.Count == segments)
                    break;

                // Store new interpolated point as last test point for use in next segment calculations
                // reset current segment length and jump back to the last index because we aren't done with original line segment
                lastTestPoint = interpolatedPoint;
                currentSegmentLength = 0;
                currentIndex--;
            }

            // Return interpolated point array
            return interpolatedPoints.ToArray();
        }

        public static Point GetInterpolatedPoint(Point lineStartPoint, Point lineEndPoint, double interpolatePosition)
        {
            // Create return point
            // Calculate x and y of increment point
            var pReturn = new Point
            {
                X = (1 - interpolatePosition)*lineStartPoint.X + interpolatePosition*lineEndPoint.X,
                Y = (1 - interpolatePosition)*lineStartPoint.Y + interpolatePosition*lineEndPoint.Y
            };
            
            // Return new point
            return pReturn;
        }
        
        public static double[] GetPointArrayAngles(Point[] pointArray)
        {
            // Create an empty collection of angles
            var angularMargins = new List<double>();

            // Enumerate input point array starting with second point and calculate angular margin
            for (var currentIndex = 1; currentIndex < pointArray.Length; currentIndex++)
                angularMargins.Add(GetAngle(pointArray[currentIndex - 1], pointArray[currentIndex]));

            // Return angular margins array
            return angularMargins.ToArray();
        }

        public static double GetAngle(Point lineStartPoint, Point lineEndPoint)
        {
            return Math.Atan2((lineEndPoint.Y - lineStartPoint.Y), (lineEndPoint.X - lineStartPoint.X));
        }

        public static double GetDotProduct(double angle1, double angle2)
        {
            var retValue = Math.Abs(angle1 - angle2);

            if (retValue > Math.PI)
                retValue = Math.PI - (retValue - Math.PI);

            return retValue;
        }

        public static double GetProbabilityFromDotProduct(double dotProduct)
        {
            // Constant represents precalculated scale of conversion of angle to probability.
            const double dScale = 31.830988618379067D;
            return Math.Abs(dotProduct*dScale - 100);
        }

        public static double GetDegreeFromRadian(double angle)
        {
            return angle*(180.0/Math.PI);
        }
        
        public static double GetDistance(Point lineStartPoint, Point lineEndPoint)
        {
            return GetDistance(lineStartPoint.X, lineStartPoint.Y, lineEndPoint.X, lineEndPoint.Y);
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var xd = x2 - x1;
            var yd = y2 - y1;
            return Math.Sqrt(xd*xd + yd*yd);
        }

        public static double GetPointArrayLength(Point[] points)
        {
            // Create return variable to hold final calculated length
            double returnLength = 0;

            // Enumerate points in point pattern and get a sum of each line segments distances
            for (var currentIndex = 1; currentIndex < points.Length; currentIndex++)
                returnLength += GetDistance(points[currentIndex - 1], points[currentIndex]);

            // Return calculated length
            return returnLength;
        }
    }
}