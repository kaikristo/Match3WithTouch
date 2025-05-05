using UnityEngine;

namespace Match3
{
    public class InputHandler : MonoBehaviour
    {
        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                HandleInput(touch.phase, touch.position);
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                var phase = Input.GetMouseButtonDown(0) ? TouchPhase.Began :
                    Input.GetMouseButton(0) ? TouchPhase.Moved :
                    TouchPhase.Ended;
                HandleInput(phase, Input.mousePosition);
            }
        }

        private void HandleInput(TouchPhase phase, Vector2 screenPos)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            Vector2 world2D = new Vector2(worldPos.x, worldPos.y);
            RaycastHit2D hit = Physics2D.Raycast(world2D, Vector2.zero);

            if (!hit.collider) return;

            GamePiece piece = hit.collider.GetComponent<GamePiece>();
            if (!piece) return;

            GameGrid grid = piece.GameGridRef;
            if (grid == null) return;

            switch (phase)
            {
                case TouchPhase.Began:
                    grid.PressPiece(piece);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    grid.EnterPiece(piece);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    grid.ReleasePiece();
                    break;
            }
        }
    }
}