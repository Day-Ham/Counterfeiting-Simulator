namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    public class UndoLastDrawCanvasOperation : CanvasOperation
    {
        readonly CanvasDrawController _canvasDrawController;

        public UndoLastDrawCanvasOperation(CanvasDrawController canvasDrawController)
        {
            _canvasDrawController = canvasDrawController;
        }

        public override void Execute()
        {
            if (_canvasDrawController.CurrentCanvasStateHistoryCount <= 1)
                return;

            int nextCanvasStateHistoryIndex = (_canvasDrawController.LatestCanvasStateHistoryIndex - 1 + _canvasDrawController.HistorySize) % _canvasDrawController.HistorySize;
            CanvasState srcCanvasState = _canvasDrawController.CanvasStateHistory[nextCanvasStateHistoryIndex];
            CanvasState dstCanvasState = _canvasDrawController.MainCanvasState;
            CanvasDrawUtilities.CopyCanvasState(srcCanvasState, dstCanvasState);

            _canvasDrawController.LatestCanvasStateHistoryIndex = nextCanvasStateHistoryIndex;
            _canvasDrawController.CurrentCanvasStateHistoryCount--;
        }
    }
}