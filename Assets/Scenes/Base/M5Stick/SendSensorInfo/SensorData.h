#ifndef SENSORDATA_H
#define SENSORDATA_H
#include <M5StickC.h>
#include <Wire.h>
#include <Adafruit_BMP280.h>
#include <SHT3x.h>

// センサ情報
struct SensorData {
  float accX, accY, accZ;
  float gyroX, gyroY, gyroZ;
  float temperature, humidity, pressure;
};

// 各センサの更新タイマー
class Timer {
private:
  unsigned long lastUpdate;
  unsigned long interval;

public:
  Timer(unsigned long interval)
    : interval(interval), lastUpdate(0) {}

  bool isTimeToUpdate() {
    unsigned long currentMillis = millis();
    if (currentMillis - lastUpdate >= interval) {
      lastUpdate = currentMillis;
      return true;
    }
    return false;
  }
};

// ENV2センサクラス
class EnvSensor {
private:
  SHT3x sht30;
  Adafruit_BMP280 bme;

public:
  bool begin();
  void update(SensorData& data);
};

#endif