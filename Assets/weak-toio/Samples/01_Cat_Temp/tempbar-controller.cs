using Evaluation;
using UnityEngine;

namespace Environment
{
  public class TemperatureBarController : MonoBehaviour
  {
    [Header("References")]
    [SerializeField] private RectTransform handleTransform;
    [SerializeField] private CatTemperatureEvaluate temperatureEvaluator;

    [Header("Bar Settings")]
    [SerializeField] private float minTemperature = 10f;
    [SerializeField] private float maxTemperature = 30f;
    [SerializeField] private float handleMovementRange = 100f; // バーの移動可能範囲（片側）

    private void Start()
    {
      if (temperatureEvaluator == null)
      {
        temperatureEvaluator = GetComponent<CatTemperatureEvaluate>();
      }
    }

    private void Update()
    {
      UpdateHandlePosition();
    }

    private void UpdateHandlePosition()
    {
      float currentTemp = temperatureEvaluator.CurrentParam;

      // 温度値を-1から1の範囲にマッピング
      float normalizedTemp = Mathf.InverseLerp(minTemperature, maxTemperature, currentTemp);
      float mappedPosition = Mathf.Lerp(-handleMovementRange, handleMovementRange, normalizedTemp);

      // ハンドルの位置を更新
      Vector2 currentPos = handleTransform.anchoredPosition;
      currentPos.x = mappedPosition;
      handleTransform.anchoredPosition = currentPos;

      // 温度に応じて色を変更
      UpdateHandleColor(currentTemp);
    }

    private void UpdateHandleColor(float temperature)
    {
      var handleImage = handleTransform.GetComponent<UnityEngine.UI.Image>();
      if (handleImage == null) return;

      if (temperature < temperatureEvaluator.SuitableRange.LowerLimit)
      {
        handleImage.color = Color.blue;
      }
      else if (temperature > temperatureEvaluator.SuitableRange.UpperLimit)
      {
        handleImage.color = Color.red;
      }
      else
      {
        handleImage.color = Color.green;
      }
    }

    // Inspectorでの設定を容易にするためのプロパティ
    public float MinTemperature
    {
      get => minTemperature;
      set => minTemperature = value;
    }

    public float MaxTemperature
    {
      get => maxTemperature;
      set => maxTemperature = value;
    }

    public float HandleMovementRange
    {
      get => handleMovementRange;
      set => handleMovementRange = value;
    }
  }
}