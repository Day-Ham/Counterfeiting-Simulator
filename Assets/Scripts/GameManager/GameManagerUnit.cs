using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public abstract class GameManagerUnit : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] protected InputHandler inputHandler;
        [SerializeField] protected CanvasDrawControllerValue canvasDrawController;

        protected CanvasDrawController CanvasDraw;

        protected virtual void Start()
        {
            CanvasDraw = canvasDrawController.Value;

            if (CanvasDraw == null)
            {
                Debug.LogError("CanvasDrawController not found.");
                enabled = false;
                return;
            }

            inputHandler?.Initialize(CanvasDraw, FinishGame);

            InitializeGameMode();
        }

        protected virtual void Update()
        {
            if (GameState.IsGamePaused) return;

            inputHandler?.UpdateInput();
            CanvasDraw?.Tick();
        }

        protected abstract void InitializeGameMode();
        protected abstract void FinishGame();
    }
}
