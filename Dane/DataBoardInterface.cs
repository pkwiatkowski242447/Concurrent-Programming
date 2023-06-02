using System.Runtime.Serialization;

namespace Data
{
    public abstract class DataBoardInterface : ISerializable
    {
        public abstract int WidthOfTheBoard { get; }
        public abstract int HeightOfTheBoard { get; }

        public static DataBoardInterface CreateBoard(int widthOfTheTable, int heightOfTheTable)
        {
            return new Board(widthOfTheTable, heightOfTheTable);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Width of the board: ", WidthOfTheBoard);
            info.AddValue("Height of the board: " , HeightOfTheBoard);
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