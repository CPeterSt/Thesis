/*
 * WiFiManager.h
 *
 *  Created on: 20 Aug 2017
 *      Author: Cundell
 */

#include "NVSManager.h"
#include "esp_wifi.h"
#include "esp_event.h"
#include "esp_event_loop.h"
#include "esp_log.h"
#include <lwip/sockets.h>



#ifndef MAIN_WIFIMANAGER_H_
#define MAIN_WIFIMANAGER_H_

void WiFi_Config(char SSID [], char password []);
void socket_server(void);
void TCPIP_State_Machine(char received[][64]);
esp_err_t SplitRecivedData(char*);
int TCPIP_Transmit(char* msg, ...);
void SendData_TCPIP(char * Data, uint8_t checkCounts);
void wifiDisconnected();
void wifiConnected();
esp_err_t event_handler(void *ctx, system_event_t *event);


extern uint16_t samplePeriod;
extern uint16_t sampleCount;

#endif /* MAIN_WIFIMANAGER_H_ */
