#include "M5StickCSensor.h"

// 更新タイマー
Timer mpuTimer(Config::MPU_UPDATE_INTERVAL);
Timer vbatTimer(Config::BATTERY_UPDATE_INTERVAL);

// M5StickCの初期化
void M5StickCSensor::begin() {
    M5.begin();
    Wire.begin();
    M5.MPU6886.Init();
}

void M5StickCSensor::update() {
    if (mpuTimer.isTimeToUpdate()) {
        updateMPU();
    }
    if (vbatTimer.isTimeToUpdate()) {
        updateVbat();
    }
}

void M5StickCSensor::updateMPU() {
    M5.MPU6886.getAccelData(&accX, &accY, &accZ);
    M5.MPU6886.getGyroData(&gyroX, &gyroY, &gyroZ);
}

void M5StickCSensor::updateVbat() { vbat = M5.Axp.GetVbatData() * 1.1 / 1000; }

float M5StickCSensor::getAccX() { return accX; }

float M5StickCSensor::getAccY() { return accY; }

float M5StickCSensor::getAccZ() { return accZ; }

float M5StickCSensor::getGyroX() { return gyroX; }

float M5StickCSensor::getGyroY() { return gyroY; }

float M5StickCSensor::getGyroZ() { return gyroZ; }

float M5StickCSensor::getVbat() { return vbat; }