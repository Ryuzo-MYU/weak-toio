#include "EnvSensor.h"

#include <M5StickC.h>

Timer envTimer(Config::ENV_UPDATE_INTERVAL);

void EnvSensor::begin() {
    if (!bmp.begin(&Wire, BMP280_I2C_ADDR, 21, 22, 400000U)) {
        Serial.println("Couldn't find BMP280");
        while (1) delay(1);
    }
}

void EnvSensor::update() {
    if (envTimer.isTimeToUpdate()) {
        sht30.update();
        temperature = sht30.cTemp;
        humidity = sht30.humidity;
        pressure = bmp.readPressure();
    }
}

float EnvSensor::getTemperature() { return temperature; }

float EnvSensor::getHumidity() { return humidity; }

float EnvSensor::getPressure() { return pressure; }