namespace Testing_TimSort
{
    public struct SortingResult
    {
        public long Time { get; set; }
        public ulong Comparisons { get; set; }
        public ulong Transpositions { get; set; }
        public long Acceleration { get; set; }
    }
}