#ifndef ENVSENSOR_H
#define ENVSENSOR_H
#include <M5UnitENV.h>

#include "Config.h"
#include "Timer.h"

// ENV2センサクラス
class EnvSensor {
   private:
    SHT3X sht30;
    BMP280 bmp;
    float temperature, humidity, pressure;

   public:
    // コンストラクタ
    EnvSensor() : temperature(0), humidity(0), pressure(0) {}
    void begin();
    void update();
    float getTemperature();
    float getHumidity();
    float getPressure();
};
#endif