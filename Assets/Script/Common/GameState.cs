// Copyright (C) 2017-2022 gamevanilla. All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement,
// a copy of which is available at http://unity3d.com/company/legal/as_terms.

using System;
using System.Collections.Generic;

namespace CandyMatch3.Game.Common
{
    /// <summary>
    ///  This class stores the state of a game at a given point in time.
    /// </summary>
    public class GameState
    {
        public int score;
        public Dictionary<CandyColor, int> collectedCandies = new Dictionary<CandyColor, int>();
        public Dictionary<SpecialCandyType, int> collectedSpecialCandies = new Dictionary<SpecialCandyType, int>();
        public Dictionary<ElementType, int> collectedElements = new Dictionary<ElementType, int>();
        public Dictionary<SpecialBlockType, int> collectedSpecialBlocks = new Dictionary<SpecialBlockType, int>();
        public Dictionary<CollectableType, int> collectedCollectables = new Dictionary<CollectableType, int>();
        public Dictionary<MultiBlockType, int> collectedMultiBlocks = new Dictionary<MultiBlockType, int>();
        public bool destroyedAllChocolates;

        /// <summary>
        /// Resets the game state to its initial state.
        /// </summary>
        public void Reset()
        {
            score = 0;
            collectedCandies.Clear();
            collectedSpecialCandies.Clear();
            collectedElements.Clear();
            collectedSpecialBlocks.Clear();
            collectedCollectables.Clear();
            foreach (var value in Enum.GetValues(typeof(CandyColor)))
            {
                collectedCandies.Add((CandyColor)value, 0);
            }
            foreach (var value in Enum.GetValues(typeof(CandyColor)))
            {
                collectedSpecialCandies.Add((SpecialCandyType)value, 0);
            }
            foreach (var value in Enum.GetValues(typeof(ElementType)))
            {
                collectedElements.Add((ElementType)value, 0);
            }
            foreach (var value in Enum.GetValues(typeof(SpecialBlockType)))
            {
                collectedSpecialBlocks.Add((SpecialBlockType)value, 0);
            }
            foreach (var value in Enum.GetValues(typeof(CollectableType)))
            {
                collectedCollectables.Add((CollectableType)value, 0);
            }

            destroyedAllChocolates = false;
        }

        public void AddCandy(CandyColor candy)
        {
            collectedCandies[candy] += 1;
        }

        public void AddSpecialCandy(SpecialCandyType specialCandy)
        {
            collectedSpecialCandies[specialCandy] += 1;
        }
        public void AddElement(ElementType element)
        {
            collectedElements[element] += 1;
        }

        public void AddSpecialBlock(SpecialBlockType block)
        {
            collectedSpecialBlocks[block] += 1;
        }

        public void AddCollectable(CollectableType collectable)
        {
            collectedCollectables[collectable] += 1;
        }
    }
}
