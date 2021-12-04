using UnityEngine;

public class OctorokBehaviour : EnemyBehaviour
{
    private const float TIME_BETWEEN_ACTIONS = 1.0f;
    private float _lastMovementChange;

    new void Start()
    {
        base.Start();
        _lastMovementChange = Time.time;
    }

    new void Update()
    {
        if (_lastMovementChange + TIME_BETWEEN_ACTIONS <= Time.time)
        {
            _lastMovementChange = Time.time;
            Controller.ClearPersistentInput();

            int action = Random.Range(0, 5);

            // Moves in a direction depending on the action number. 0 == no movement.
            switch (action)
            {
                case 1:
                    Controller.MoveLeft(true);
                    break;
                case 2:
                    Controller.MoveRight(true);
                    break;
                case 3:
                    Controller.MoveDown(true);
                    break;
                case 4:
                    Controller.MoveUp(true);
                    break;
            }
        }

        // Keeps to the area bounds.
        base.Update();
    }
}
