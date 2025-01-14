namespace ActionGenerate
{
  public static class ToioSettings
  {
    private static int _maxSpd = 100;  // デフォルト値
    private static double _deadZone = 8;  // デフォルト値

    public static int MaxSpd => _maxSpd;
    public static double DeadZone => _deadZone;

    // Toioオブジェクトから設定を更新
    public static void UpdateSettings(Toio toio)
    {
      if (toio != null && toio.Cube != null)
      {
        _maxSpd = toio.Cube.maxSpd;
        _deadZone = toio.Cube.deadzone;
      }
    }

    // 値を直接設定（テスト用など）
    public static void SetSettings(int maxSpd, double deadZone)
    {
      _maxSpd = maxSpd;
      _deadZone = deadZone;
    }
  }
}