using System;
using System.Collections.ObjectModel;

namespace Testing_TimSort
{
    public class SequencesGenerator
    {
        public int[] GenerateSequence(int quantity, int seqType)
        {
            switch (seqType)
            {
                case 0:
                    return new int[1];
                case 1:
                    return new int[1];
                case 2:
                    return new int[1];
                case 3:
                    return new int[1];
                case 4:
                    return new int[1];
                default:
                    throw new NotImplementedException();
            }
        }

        private int[] GenerateIncreasingSeq(int quantity)
        {
            return new int[quantity];
        }
        
        private int[] GenerateDecreasingSeq(int quantity)
        {
            return new int[quantity];
        }
        
        private int[] GenerateRandomSeq(int quantity)
        {
            return new int[quantity];
        }
        
        private int[] GenerateSameSeq(int quantity)
        {
            return new int[quantity];
        }
        
        private int[] GeneratePartiallyOrderedSeq(int quantity)
        {
            return new int[quantity];
        }
    }
}