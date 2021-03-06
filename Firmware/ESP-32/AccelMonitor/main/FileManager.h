/*
 * FileManager.h
 *
 *  Created on: 19 Sep 2017
 *      Author: Cundell
 */

#include "Utilities.h"
#include "esp_spiffs.h"
#include "spiffs_config.h"


#ifndef MAIN_FILEMANAGER_H_
#define MAIN_FILEMANAGER_H_
#define MAX_FILE_SIZE 16384
esp_err_t File_Init();
esp_err_t WriteFile(char*fileName, char * msg, ...);
esp_err_t ClearSPIFlashFile(char*fileName);
//esp_err_t ReadFileLine(char*fileName, char * msg);
void FileUnmount();


#endif /* MAIN_FILEMANAGER_H_ */
