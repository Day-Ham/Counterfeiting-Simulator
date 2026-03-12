using DaeHanKim.ThisIsTotallyADollar.Drawing;
using DaeHanKim.ThisIsTotallyADollar.Utility;
using UnityEngine;

namespace DaeHanKim.ThisIsTotallyADollar.Core
{
    [DisallowMultipleComponent]
    public abstract class GameManagerUnit : MonoBehaviour
    {
        [Header("Game Manager Value")]
        [SerializeField] protected GameManagerValue _gameManagerValue;

        [Header("Dependencies")]
        [SerializeField] protected InputHandler _inputHandler;
        [SerializeField] protected CanvasDrawControllerValue _canvasDrawController;

        protected CanvasDrawController _canvasDraw;
        protected bool GameIsPaused;

        protected virtual void Awake()
        {
            _gameManagerValue.Value = this;
        }

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
            if (GameIsPaused) return;

            _inputHandler?.UpdateInput();
            _canvasDraw?.Tick();
        }

        protected abstract void InitializeGameMode();
        protected abstract void FinishGame();

        public void PauseGame() => GameIsPaused = true;

        public void ResumeGame() => GameIsPaused = false;
    }
}
