// Copyright 2018 Daniil Goncharov <neargye@gmail.com>.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics;
using System.IO;

namespace SwipeType.Example
{
    internal static class Program
    {
        private static void Main()
        {
            SwipeType simpleSwipeType = new MatchSwipeType(File.ReadAllLines("EnglishDictionary.txt"));
            SampleUsingSwipeType(simpleSwipeType);
            Console.ReadKey(true);

            SwipeType distanceSwipeType = new DistanceSwipeType(File.ReadAllLines("EnglishDictionary.txt"));
            SampleUsingSwipeType(distanceSwipeType);
            Console.ReadKey(true);
        }

        private static void SampleUsingSwipeType(SwipeType swipeType)
        {
            

            Stopwatch stopwatch = new Stopwatch();
            string[] testCases =
            {
                "heqerqllo",
                "qwertyuihgfcvbnjk",
                "wertyuioiuytrtghjklkjhgfd",
                "dfghjioijhgvcftyuioiuytr",
                "aserfcvghjiuytedcftyuytre",
                "asdfgrtyuijhvcvghuiklkjuytyuytre",
                "mjuytfdsdftyuiuhgvc",
                "vghjioiuhgvcxsasdvbhuiklkjhgfdsaserty"
            };

            foreach (var s in testCases)
            {
               

                stopwatch.Start();
                var result = swipeType.GetSuggestion(s, 10);
                stopwatch.Stop();
                stopwatch.Reset();

                int length = result.Length;
                for (int i = 0; i < length; ++i)
                {
                    i = i;
                    //Console.WriteLine($"match {i + 1}: {result[i]}");
                }
            }
        }
    }
}
