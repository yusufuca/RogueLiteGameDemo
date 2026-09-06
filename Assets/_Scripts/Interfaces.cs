using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);
}
public interface IMovementState
{
    void EnterState(Movement movement);
    void UpdateState(Movement movement);
    void ExitState(Movement movement);
}