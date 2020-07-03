using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeType.Example
{
    /// <summary>
    /// Provides logic to compare a single gesture against a set of learned PointPatterns.
    /// </summary>
    public class PointPatternAnalyzer
    {
        /// <summary>
        /// Instantiates a new PointPatternAnalyzer with an empty PointPatternSet and a default precision of 100.
        /// </summary>
        public PointPatternAnalyzer()
            : this(new PointPattern[0], 100)
        {

        }

        /// <summary>
        /// Instantiates a new PointPatternAnalyzer with the specified PointPatternSet and a default precision of 100.
        /// </summary>
        /// <param name="pointPatternSet">IEnumerable of PointPattern objects loaded from an external source.</param>
        public PointPatternAnalyzer(IEnumerable<PointPattern> pointPatternSet) 
            : this(pointPatternSet, 100)
        {

        }

        /// <summary>
        /// Instantiates a new PointPatternAnalyzer with the specified PointPatternSet and precision.
        /// </summary>
        /// <param name="pointPatternSet">IEnumerable of PointPattern objects loaded from an external source.</param>
        /// <param name="precision">Number of interpolation steps to use when comparing two gestures</param>
        public PointPatternAnalyzer(IEnumerable<PointPattern> pointPatternSet, int precision)
        {
            // Instantiate PointPatternAnalyzer class with a PointPatternSet and Precision
            PointPatternSet = pointPatternSet.ToList();
            Precision = precision;
        }
        
        /// <summary>
        /// Gets or sets the number of interpolation steps to use when comparing two gestures.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// Library of saved gestures to compare the gesture to be analyzed against.
        /// </summary>
        public List<PointPattern> PointPatternSet { get; set; }
        
        /// <summary>
        /// Compares the points of a drawn gesture to every learned gesture in PointPatternSet and returns an
        /// array of accuracy probabilities of each gesture ordered by best match first.
        /// </summary>
        /// <param name="points">Points of the current gesture being analyzed.</param>
        /// <returns>Returns an array of accuracy probabilities of each gesture in PointPatternSet ordered by best match first.</returns>
        public PointPatternMatchResult[] GetPointPatternMatchResults(Point[] points, string f)
        {
            // Ensure we have at least 2 points or recognition will fail as we are unable to interpolate between a single point.
            if (points.Length < 2)
                return new PointPatternMatchResult[0];

            // Create a list of PointPatternMatchResults to hold final results and group results of point pattern set comparison
            var comparisonResults = new List<PointPatternMatchResult>();
            var groupComparisonResults = new List<PointPatternMatchResult>();


            var mylist=PointPatternSet.Where(p=>p.Name.StartsWith(f));
            //foreach(var s in mylist)
            //{
            //    Debug.Log(s.Name);
            //}

            // Enumerate each point patterns grouped by name
         //   foreach (var pointPatternSet in PointPatternSet.GroupBy(pp => pp.Name))
                foreach (var pointPatternSet in mylist.GroupBy(pp => pp.Name))
                {
                // Clear out group comparison results from last group comparison
                groupComparisonResults.Clear();

                // Calculate probability of each point pattern in this group
                foreach (var pointPatternCompareTo in pointPatternSet)
                {
                    var result = GetPointPatternMatchResult(pointPatternCompareTo, points);
                    groupComparisonResults.Add(result);
                }

                // Add results of group comparison to final comparison results
                var averageProbability = groupComparisonResults.Average(ppmr => ppmr.Probability);
                var totalSetCount = groupComparisonResults.Sum(ppmr => ppmr.PointPatternSetCount);
                var totalDistance = groupComparisonResults.Average(ppmr => ppmr.Distance);
                var myAverageDistance= groupComparisonResults.Average(ppmr => ppmr.AverageDistance);

                comparisonResults.Add(new PointPatternMatchResult(pointPatternSet.Key, averageProbability, totalSetCount,totalDistance, myAverageDistance));
            }

            // Return comparison results ordered by highest probability
             return comparisonResults.OrderByDescending(ppmr => ppmr.Probability).ToArray();
          //  return comparisonResults.OrderBy(ppmr => ppmr.Distance).ToArray();
        }

        /// <summary>
        /// Compares a points of a single gesture, to the points in a single saved gesture, and returns a accuracy probability.
        /// </summary>
        /// <param name="compareTo">Learned PointPattern from PointPatternSet to compare gesture points to.</param>
        /// <param name="points">Points of the current gesture being analyzed.</param>
        /// <returns>Returns the accuracy probability of the learned PointPattern to the current gesture.</returns>
        public PointPatternMatchResult GetPointPatternMatchResult(PointPattern compareTo, Point[] points)
        {
            //compare points

          

            // Ensure we have at least 2 points or recognition will fail as we are unable to interpolate between a single point.
            if (points.Length < 2)
                 throw new ArgumentOutOfRangeException("hello"/*nameof(points)*/);

            // We'll use an array of doubles that matches the number of interpolation points to hold
            // the dot products of each angle comparison.
            var dotProducts = new double[Precision];
            var myDistance = new double[Precision];

            // We'll need to interpolate the incoming points array and the points of the learned gesture.
            // We do this for each comparison so that we can change the precision at any time and not lose
            // or original learned gesture to multiple interpolations.
            var interpolatedCompareTo = PointPatternMath.GetInterpolatedPointArray(compareTo.Points, Precision);
            var interpolatedPointArray = PointPatternMath.GetInterpolatedPointArray(points, Precision);

            int ij = 0;
            float totalDistance = 0;
            foreach(Point p in interpolatedCompareTo)
            {

                Vector2 v1 = new Vector2((float)p.X, (float)p.Y);
                Vector2 v2 = new Vector2((float)interpolatedPointArray[ij].X, (float)interpolatedPointArray[ij].Y);
                float d=Vector2.Distance(v1, v2);
                myDistance[ij]= Vector2.Distance(v1, v2);
                totalDistance = totalDistance + d;
                ij++;

               

                
            }

           // Debug.Log("distance is " + totalDistance);

            // Next we'll get an array of angles for each interpolated point in the learned and current gesture.
            // We'll get the same number of angles corresponding to the total number of interpolated points.
            var anglesCompareTo = PointPatternMath.GetPointArrayAngles(interpolatedCompareTo);
            var angles = PointPatternMath.GetPointArrayAngles(interpolatedPointArray);
            double averageDistance = myDistance.Average();
            // Now that we have angles for each gesture, we'll get the dot product of every angle equal to 
            // the total number of interpolation points.
            for (var i = 0; i <= anglesCompareTo.Length - 1; i++)
                dotProducts[i] = PointPatternMath.GetDotProduct(anglesCompareTo[i], angles[i]);

            // Convert average dot product to probability since we're using the deviation
            // of the average of the dot products of every interpolated point in a gesture.
            var probability = PointPatternMath.GetProbabilityFromDotProduct(dotProducts.Average());

            // Return PointPatternMatchResult object that holds the results of comparison.
            return new PointPatternMatchResult(compareTo.Name, probability, 1,totalDistance, averageDistance);
        }
    }
}