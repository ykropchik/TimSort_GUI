namespace Testing_TimSort
{
    public struct ExperimentResult
    {
        public string FileName { get; set; }
        public SortingResult TimSort { get; set; }
        public SortingResult Insertion { get; set; }
        public SortingResult Merge { get; set; }
    }
}