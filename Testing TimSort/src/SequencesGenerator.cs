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
                    return GenerateIncreasingSeq(quantity);
                case 1:
                    return GenerateDecreasingSeq(quantity);
                case 2:
                    return GenerateRandomSeq(quantity);
                case 3:
                    return GenerateSameSeq(quantity);
                case 4:
                    return GeneratePartiallyOrderedSeq(quantity);
                case 5:
                    return GenerateWorstForTimSort(quantity);
                default:
                    throw new NotImplementedException();
            }
        }

        private int[] GenerateIncreasingSeq(int quantity)
        {
            var result = new int[quantity];

            for (int i = 0; i < quantity; i++)
            {
                result[i] = i;
            }
            
            return result;
        }
        
        private int[] GenerateDecreasingSeq(int quantity)
        {
            var result = new int[quantity];

            for (int i = 0; i < quantity; i++)
            {
                result[i] = quantity - i;
            }
            
            return result;
        }
        
        private int[] GenerateRandomSeq(int quantity)
        {
            var result = new int[quantity];
            var rand = new Random();
            
            for (int i = 0; i < quantity; i++)
            {
                result[i] = rand.Next();;
            }
            
            return result;
        }
        
        private int[] GenerateSameSeq(int quantity)
        {
            var result = new int[quantity];
            var rand = new Random();
            int value = rand.Next();
            
            for (int i = 0; i < quantity; i++)
            {
                result[i] = value;
            }
            
            return result;
        }
        
        private int[] GeneratePartiallyOrderedSeq(int quantity)
        {
            return new int[quantity];
        }
        
        private int[] GenerateWorstForTimSort(int quantity)
        {
            return new int[quantity];
        }
    }
}