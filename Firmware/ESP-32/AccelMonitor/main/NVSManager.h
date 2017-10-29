/*
 * NVSManager.h
 *
 *  Created on: 19 Aug 2017
 *      Author: Cundell
 */

#include "nvs_flash.h"
#include "nvs.h"
#include "Utilities.h"


#ifndef MAIN_NVSMANAGER_H_
#define MAIN_NVSMANAGER_H_

esp_err_t initialize_nvs(void);
esp_err_t save_restart_counter(void);
esp_err_t save_to_nvs(char* myChar, uint16_t * currentSize, char * msg, ...);
esp_err_t clear_all_nvs(char * msg, ...);
esp_err_t print_nvs(char * msg, ...);
esp_err_t save_Time_NVS(int64_t timeVal);
esp_err_t save_i64_NVS(int64_t i64Val, char * nameSpace);
esp_err_t read_i64_NVS(int64_t * i64Val, char * nameSpace);
esp_err_t read_Time_NVS(int64_t *timeVal);


#endif /* MAIN_NVSMANAGER_H_ */
