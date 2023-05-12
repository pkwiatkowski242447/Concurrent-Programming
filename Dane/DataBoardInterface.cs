namespace Data
{
    public abstract class DataBoardInterface
    {
        public abstract int WidthOfTheBoard { get; set; }
        public abstract int HeightOfTheBoard { get; set; }

        public static DataBoardInterface CreateBoard(int widthOfTheTable, int heightOfTheTable)
        {
            return new Board(widthOfTheTable, heightOfTheTable);
        }

        private class Board : DataBoardInterface
        {
            public override int WidthOfTheBoard { get; set; }
            public override int HeightOfTheBoard { get; set; }

            public Board(int widthOfTheTable, int heightOfTheTable)
            {
                this.WidthOfTheBoard = widthOfTheTable;
                this.HeightOfTheBoard = heightOfTheTable;
            }
        }
    }
}
