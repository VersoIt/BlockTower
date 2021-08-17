using UnityEngine;

public class MenuStatementController : MonoBehaviour
{

    private void OnEnable() => MenuStatement.Instance.IsEnabled = true;

    private void OnDisable() => MenuStatement.Instance.IsEnabled = false;
}
