#include "SensorData.h"

bool EnvSensor::begin() {
  sht30.Begin();
  int retries = 5;
  while (!bme.begin(0x76) && retries > 0) {
    M5.Lcd.println("BMP280 init fail, retrying...");
    delay(1000);
    retries--;
  }
  return retries > 0;
}

void EnvSensor::update(SensorData &data) {
  sht30.UpdateData();
  data.temperature = sht30.GetTemperature();
  data.humidity = sht30.GetRelHumidity();
  data.pressure = bme.readPressure();
}