namespace Data
{
    public abstract class SerializationObject
    {
        public static SerializationObject CreateBallCopy(DataBallInterface dataBall)
        {
            return new BallCopy(dataBall);
        }

        public static SerializationObject CreateBoardCopy(DataBoardInterface dataBoard)
        {
            return new BoardCopy(dataBoard);
        }
    }

    internal class BallCopy : SerializationObject
    {
        public int BallId { get; set; }
        public DataPositionInterface BallPosition { get; set; }
        public DataPositionInterface BallVelocityVector { get; set; }

        public BallCopy(DataBallInterface dataBall)
        {
            this.BallPosition = DataPositionInterface.CreatePosition(dataBall.CenterOfTheBall.XCoordinate, dataBall.CenterOfTheBall.YCoordinate);
            this.BallVelocityVector = DataPositionInterface.CreatePosition(dataBall.VelocityVectorOfTheBall.XCoordinate, dataBall.VelocityVectorOfTheBall.YCoordinate);
            this.BallId = dataBall.IdOfTheBall;
        }
    }

    internal class BoardCopy : SerializationObject
    {
        public int WidthOfTheBoard { get; set; }
        public int HeightOfTheBoard { get; set; }

        public BoardCopy(DataBoardInterface dataBoard)
        {
            this.WidthOfTheBoard = dataBoard.WidthOfTheBoard;
            this.HeightOfTheBoard = dataBoard.HeightOfTheBoard;
        }
    }
}
