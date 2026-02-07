namespace DaeHanKim.ThisIsTotallyADollar.Drawing
{
    public class SnapshotCurrentCanvasOperation : CanvasOperation
    {
        readonly CanvasDrawController _canvasDrawController;

        public SnapshotCurrentCanvasOperation(CanvasDrawController canvasDrawController)
        {
            _canvasDrawController = canvasDrawController;
        }

        public override void Execute()
        {
            int nextLatestCanvasStateHistoryIndex = (_canvasDrawController.LatestCanvasStateHistoryIndex + 1) % _canvasDrawController.HistorySize;
            CanvasState srcCanvasState = _canvasDrawController.MainCanvasState;
            CanvasState dstCanvasState = _canvasDrawController.CanvasStateHistory[nextLatestCanvasStateHistoryIndex];
            CanvasDrawUtilities.CopyCanvasState(srcCanvasState, dstCanvasState);

            _canvasDrawController.LatestCanvasStateHistoryIndex = nextLatestCanvasStateHistoryIndex;

            if (_canvasDrawController.CurrentCanvasStateHistoryCount >= _canvasDrawController.HistorySize)
            {
                _canvasDrawController.OldestCanvasStateHistoryIndex = (_canvasDrawController.OldestCanvasStateHistoryIndex + 1) % _canvasDrawController.HistorySize;
            }
            else
            {
                _canvasDrawController.CurrentCanvasStateHistoryCount++;
            }
        }
    }
}