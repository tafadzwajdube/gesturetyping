namespace SwipeType.Example
{
	public struct PointPattern
	{
        public PointPattern(string name, Point[] points)
        {
            Name = name;
            Points = points;
        }

		public string Name { get; set; }

		public Point[] Points { get; set; }
	}
}
