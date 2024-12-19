#include "SensorData.h"

void SensorData::setup() {
    m5.begin();
    envSensor.begin();
}

void SensorData::update() {
    m5.update();
    envSensor.update();
}

float SensorData::getAccX() {
    float accX = m5.getAccX();
    return accX;
}

float SensorData::getAccY() {
    float accY = m5.getAccY();
    return accY;
}

float SensorData::getAccZ() {
    float accZ = m5.getAccZ();
    return accZ;
}

float SensorData::getGyroX() {
    float gyroX = m5.getGyroX();
    return gyroX;
}
float SensorData::getGyroY() {
    float gyroY = m5.getGyroX();
    return gyroY;
}
float SensorData::getGyroZ() {
    float gyroZ = m5.getGyroX();
    return gyroZ;
}

float SensorData::getVbat() {
    float vbat = m5.getVbat();
    return vbat;
}

float SensorData::getTemperature() {
    float temp = envSensor.getTemperature();
    return temp;
}

float SensorData::getHumidity() {
    float humidity = envSensor.getHumidity();
    return humidity;
}

float SensorData::getPressure() {
    float pressure = envSensor.getPressure();
    return pressure;
}