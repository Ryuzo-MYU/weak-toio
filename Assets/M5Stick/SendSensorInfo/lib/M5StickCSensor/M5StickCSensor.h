#ifndef M5STICKCSENSOR_H
#define M5STICKCSENSOR_H
#include <M5Unified.h>

#include "Config.h"
#include "Timer.h"

// センサ情報
class M5StickCSensor {
   private:
    float accX, accY, accZ;     // 加速度
    float gyroX, gyroY, gyroZ;  // ジャイロ
    float vbat;                 // バッテリー情報

   public:
    // コンストラクタ
    M5StickCSensor()
        : accX(0), accY(0), accZ(0), gyroX(0), gyroY(0), gyroZ(0), vbat(0) {}

    void begin();
    void update();
	void setData();
    void updateMPU();
    void updateVbat();

    float getAccX();
    float getAccY();
    float getAccZ();
    float getGyroX();
    float getGyroY();
    float getGyroZ();
    float getVbat();
};
#endif