#ifndef SENSORDATA_H
#define SENSORDATA_H
#include "EnvSensor.h"
#include "M5StickCSensor.h"

class SensorData {
   private:
    M5StickCSensor m5;
    EnvSensor envSensor;

   public:
    // コンストラクタ
    SensorData() {
        m5 = M5StickCSensor();
        envSensor = EnvSensor();
    }

    void setup();
    void update();

    float getAccX();
    float getAccY();
    float getAccZ();

    float getGyroX();
    float getGyroY();
    float getGyroZ();

    float getTemperature();
    float getHumidity();
    float getPressure();

    float getVbat();
};
#endif