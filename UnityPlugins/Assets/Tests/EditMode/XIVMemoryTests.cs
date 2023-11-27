using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using XIV.Core.Extensions;
using XIV.Core.TweenSystem.Drivers;
using XIV.Core.DataStructures;

namespace Tests.PlayMode
{
    public class XIVMemoryTests
    {
        private int[] arr;

        [SetUp]
        public void SetUp()
        {
            arr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        [Test]
        [TestCase(0, 5, 0, 5)]
        [TestCase(2, 3, 1, 4)]
        [TestCase(4, 5, 0, 3)]
        public void Log_ValidMemory_PrintsCorrectValues(int startIndex, int length, int sliceStart, int sliceLength)
        {
            // Arrange
            var xivMemory = new XIVMemory<int>(arr, startIndex, length);

            // Act
            var loggedValues = LogMemory(xivMemory);

            // Assert
            CollectionAssert.AreEqual(arr.Skip(startIndex).Take(length).Select(x => x.ToString()), loggedValues);
        }

        [Test]
        [TestCase(0, 5, 0, 5)]
        [TestCase(2, 3, 1, 4)]
        [TestCase(4, 5, 0, 3)]
        public void LogReversed_ValidMemory_PrintsCorrectValues(int startIndex, int length, int sliceStart, int sliceLength)
        {
            // Arrange
            var xivMemory = new XIVMemory<int>(arr, startIndex, length);

            // Act
            xivMemory.Reverse();
            var loggedValues = LogMemory(xivMemory);

            // Assert
            CollectionAssert.AreEqual(arr.Skip(startIndex).Take(length).Reverse().Select(x => x.ToString()), loggedValues);
        }

        [Test]
        [TestCase(0, 5, 0, 5)]
        [TestCase(2, 3, 1, 4)]
        [TestCase(4, 5, 0, 3)]
        public void LogReversedTwice_ValidMemory_PrintsCorrectValues(int startIndex, int length, int sliceStart, int sliceLength)
        {
            // Arrange
            var xivMemory = new XIVMemory<int>(arr, startIndex, length);

            // Act
            xivMemory.Reverse();
            xivMemory.Reverse();
            var loggedValues = LogMemory(xivMemory);

            // Assert
            CollectionAssert.AreEqual(arr.Skip(startIndex).Take(length).Select(x => x.ToString()), loggedValues);
        }

        [Test]
        [TestCase(0, 5, 0, 5)]
        [TestCase(2, 3, 1, 4)]
        [TestCase(4, 5, 0, 3)]
        public void SliceTest_ValidMemory_PrintsCorrectValues(int startIndex, int length, int sliceStart, int sliceLength)
        {
            // Arrange
            var xivMemory = new XIVMemory<int>(arr, startIndex, length);

            // Act
            var slicedMemory = xivMemory.Slice(sliceStart, sliceLength);
            var loggedValues = LogMemory(slicedMemory);

            // Assert
            CollectionAssert.AreEqual(arr.Skip(startIndex + sliceStart).Take(sliceLength).Select(x => x.ToString()), loggedValues);
        }

        [Test]
        [TestCase(0, 5, 0, 5)]
        [TestCase(2, 6, 2, 2)]
        [TestCase(4, 5, 0, 3)]
        public void ReversedSliceTest_ValidMemory_PrintsCorrectValues(int startIndex, int length, int sliceStart, int sliceLength)
        {
            
            // Arrange
            var xivMemory = new XIVMemory<int>(arr, startIndex, length);

            // Act
            xivMemory.Reverse();
            var slicedMemory = xivMemory.Slice(sliceStart, sliceLength);
            var loggedValues = LogMemory(slicedMemory);

            // Assert
            CollectionAssert.AreEqual(arr.Skip(startIndex).Take(length).Reverse().Skip(sliceStart).Take(sliceLength).Select(x => x.ToString()), loggedValues);
        }
        
        static List<string> LogMemory(XIVMemory<int> xivMemory)
        {
            List<string> logs = new();
            for (int i = 0; i < xivMemory.Length; i++)
            {
                logs.Add(xivMemory[i].ToString());
            }
            return logs;
        }
    }
}