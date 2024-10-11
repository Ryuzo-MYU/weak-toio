#ifndef TIMER_H
#define TIMER_H
#include <Arduino.h>

// 各センサの更新タイマー
class Timer {
private:
  unsigned long interval;
  unsigned long lastUpdate;

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
#endif