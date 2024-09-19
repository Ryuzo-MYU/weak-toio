#ifndef CONFIG_H
#define CONFIG_H

// 通信とセンサの更新に関する設定
struct Config {
  static constexpr char* DEVICE_NAME = "M5-02";                    // Bluetoothデバイス名
  static constexpr int SERIAL_BAUD = 115200;                       // ボーレート
  static constexpr unsigned long LOOP_DELAY = 5000;                // ループ時間 (5秒)
  static constexpr unsigned long BATTERY_UPDATE_INTERVAL = 60000;  // バッテリー情報の更新間隔 (1分)
  static constexpr unsigned long ENV_UPDATE_INTERVAL = 30000;      // ENVの更新間隔 (30秒)
  static constexpr unsigned long SENSOR_UPDATE_INTERVAL = 10000;   // M5StickCセンサの更新間隔 (10秒)
};

#endif