#include "EnvSensor.h"

Timer envTimer(Config::ENV_UPDATE_INTERVAL);

void EnvSensor::begin() {
    if (!sht3x.begin(&Wire, SHT3X_I2C_ADDR, 32, 33, 400000U)) {
        Serial.println("Couldn't find SHT3X");
        while (1) delay(1);
    }

    if (!bmp.begin(&Wire, BMP280_I2C_ADDR, 32, 33, 400000U)) {
        Serial.println("Couldn't find BMP280");
        while (1) delay(1);
    }
    /* Default settings from datasheet. */
    bmp.setSampling(BMP280::MODE_NORMAL,     /* Operating Mode. */
                    BMP280::SAMPLING_X2,     /* Temp. oversampling */
                    BMP280::SAMPLING_X16,    /* Pressure oversampling */
                    BMP280::FILTER_X16,      /* Filtering. */
                    BMP280::STANDBY_MS_500); /* Standby time. */

    // 初期データの取得
    setData();
}

void EnvSensor::update() {
    if (envTimer.isTimeToUpdate()) {
        setData();
    }
}

void EnvSensor::setData() {
    if (sht3x.update()) {
        temperature = sht3x.cTemp;
        humidity = sht3x.humidity;
    }

    if (bmp.update()) {
        pressure = bmp.pressure;
    }
}

float EnvSensor::getTemperature() { return temperature; }

float EnvSensor::getHumidity() { return humidity; }

float EnvSensor::getPressure() { return pressure; }