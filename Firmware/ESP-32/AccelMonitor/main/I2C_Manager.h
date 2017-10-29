/*
 * I2C_Manager.h
 *
 *  Created on: 31 Jul 2017
 *      Author: Cundell
 */
#include "Utilities.h"
#include "driver/i2c.h"

#ifndef MAIN_I2C_MANAGER_H_
#define MAIN_I2C_MANAGER_H_

#define MMA7660_Addr 0x4C
#define ADXL345_Addr 0x1D


#define GPIO_SDA 27
#define GPIO_SCL 14

extern double AccelOffSet[2];
extern double averagG;
extern uint8_t Calabrating;

esp_err_t I2C_init();
esp_err_t I2C_Write(uint8_t * bytes, uint8_t devAddress, uint8_t regAddress, uint8_t bytes_len);
esp_err_t I2C_Read(uint8_t * bytes, uint8_t devAddress, uint8_t regAddress);
esp_err_t Get_MMA7660_XYZ(double * XYZ);
esp_err_t MMA7660_init();
esp_err_t ADXL345_init();


#endif /* MAIN_I2C_MANAGER_H_ */
