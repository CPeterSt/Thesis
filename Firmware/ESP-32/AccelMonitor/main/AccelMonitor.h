/*
 * AccelMonitor.h
 *
 *  Created on: 27 Sep 2017
 *      Author: Cundell
 */

#ifndef MAIN_ACCELMONITOR_H_
#define MAIN_ACCELMONITOR_H_

#define COMS_EN_GPIO (17)

void Sample_Save(void *parameter);
void Sample_Stream(void *parameter);
esp_err_t Calabrate_Accelerometer();
void ReadAndSendDataFile(void *ignore);
void Coms_En_task(void *ignore);

extern uint8_t WiFi_Connected;
extern uint16_t loopParameter;
extern int ComsTimerCount;
extern int64_t currentTime, offsetTime;

#endif /* MAIN_ACCELMONITOR_H_ */
