using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Testing_TimSort
{
    public static class SequencesGenerator
    {
        public static int[] GenerateSequence(int quantity, int seqType)
        {
            switch (seqType)
            {
                case 0: return GenerateIncreasingSeq(quantity);
                case 1: return GenerateDecreasingSeq(quantity);
                case 2: return GenerateRandomSeq(quantity);
                case 3: return GenerateSameSeq(quantity);
                case 4: return GeneratePartiallyOrderedSeq(quantity);
                case 5: return GenerateWorstForTimSort(quantity);
                case 6: return GenerateWorstForMergeSort(quantity);
                default: throw new NotImplementedException();
            }
        }

        private static int[] GenerateIncreasingSeq(int quantity)
        {
            var result = new int[quantity];
            for (var i = 0; i < quantity; i++)
            {
                result[i] = i;
            }

            return result;
        }

        private static int[] GenerateDecreasingSeq(int quantity)
        {
            var result = new int[quantity];
            for (var i = 0; i < quantity; i++)
            {
                result[i] = quantity - i;
            }

            return result;
        }

        private static int[] GenerateRandomSeq(int quantity)
        {
            var result = new int[quantity];
            var rand = new Random();
            for (var i = 0; i < quantity; i++)
            {
                result[i] = rand.Next();
            }

            return result;
        }

        private static int[] GenerateSameSeq(int quantity)
        {
            var result = new int[quantity];
            var rand = new Random();
            var value = rand.Next();
            for (var i = 0; i < quantity; i++)
            {
                result[i] = value;
            }

            return result;
        }

        private static int[] GeneratePartiallyOrderedSeq(int quantity)
        {
            var result = GenerateIncreasingSeq(quantity);
            var rand = new Random();
            for (var i = 0; i < quantity / 4; i++)
            {
                var pointer1 = rand.Next(quantity - 1);
                var pointer2 = rand.Next(quantity - 1);
                (result[pointer1], result[pointer2]) = (result[pointer2], result[pointer1]);
            }

            return result;
        }

        private static int[] GenerateWorstForTimSort(int quantity)
        {
            var size = quantity;
            var flag = 0;
            while (size >= 64)
            {
                flag |= size & 1;
                size >>= 1;
            }

            size += flag;
            var result = new int[size * (quantity / size + 1)];
            for (var i = 0; i < quantity / size + 1; i++)
            {
                result[i * size] = 1;
                for (var j = 1; j < size; j++)
                {
                    result[j + size * i] = quantity - j - 1;
                }
            }

            return result;
        }

        private static int[] GenerateWorstForMergeSort(int quantity)
        {
            int[] result = GenerateIncreasingSeq(quantity);
            int n = result.Length;
            
            GenerateWorstCase(result, 0, n - 1);
            
            // Функция для объединения левого и правого подмассивов
            void join(IList<int> arr, IReadOnlyList<int> leftArray, IReadOnlyList<int> rightArray,
                int left, int middle, int right)
            {
                int i;
                for (i = 0; i <= middle - left; i++)
                {
                    arr[i] = leftArray[i];
                }

                for (var j = 0; j < right - middle; j++) arr[i + j] = rightArray[j];
            }
            
            // Функция для хранения альтернативных элементов в левомй и правом подмассивах
            void Split(IReadOnlyList<int> arr, IList<int> leftArray, IList<int> rightArray,
                int left, int middle, int right)
            {
                for (var i = 0; i <= middle - left; i++) leftArray[i] = arr[i * 2];
                for (var i = 0; i < right - middle; i++) rightArray[i] = arr[i * 2 + 1];
            }

            void GenerateWorstCase(int[] arr, int l, int r)
            {
                if (l >= r) return;
                
                var m = l + (r - l) / 2;

                // создаем два вспомогательных массива
                var left = new int[m - l + 1];
                var right = new int[r - m];

                // Сохраняем альтернативные элементы массива
                // в левом и правом подмассивах
                Split(arr, left, right, l, m, r);

                // Рекурсивно делаем тоже самое для левого и правого подмассивов
                GenerateWorstCase(left, l, m);
                GenerateWorstCase(right, m + 1, r);

                // объединяем левый и правый подмассивы
                join(arr, left, right, l, m, r);
            }

            return result;
        }
    }
}