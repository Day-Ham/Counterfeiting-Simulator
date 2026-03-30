using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public abstract class GameManagerUnit : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] protected InputHandler _inputHandler;
        [SerializeField] protected CanvasDrawControllerValue _canvasDrawController;

        protected CanvasDrawController _canvasDraw;

        protected virtual void Start()
        {
            _canvasDraw = _canvasDrawController.Value;

            if (_canvasDraw == null)
            {
                Debug.LogError("CanvasDrawController not found.");
                enabled = false;
                return;
            }

            _inputHandler?.Initialize(_canvasDraw, FinishGame);

            InitializeGameMode();
        }

        protected virtual void Update()
        {
            if (GameState.IsGamePaused) return;

            _inputHandler?.UpdateInput();
            _canvasDraw?.Tick();
        }

        protected abstract void InitializeGameMode();
        protected abstract void FinishGame();
    }
}
