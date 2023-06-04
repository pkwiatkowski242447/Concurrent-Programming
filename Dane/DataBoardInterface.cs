namespace Data
{
    public abstract class DataBoardInterface
    {
        public abstract int WidthOfTheBoard { get; }
        public abstract int HeightOfTheBoard { get; }

        public static DataBoardInterface CreateBoard(int widthOfTheTable, int heightOfTheTable)
        {
            return new Board(widthOfTheTable, heightOfTheTable);
        }

        private class Board : DataBoardInterface
        {
            public override int WidthOfTheBoard { get; }
            public override int HeightOfTheBoard { get; }

            internal Board(int widthOfTheTable, int heightOfTheTable)
            {
                this.WidthOfTheBoard = widthOfTheTable;
                this.HeightOfTheBoard = heightOfTheTable;
            }
        }
    }
}