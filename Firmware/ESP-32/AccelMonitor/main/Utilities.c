/*
 * Utilities.c
 *
 *  Created on: 31 Jul 2017
 *      Author: Cundell
 */
#include "Utilities.h"
#include <math.h>
#include "NVSManager.h"
#include "WiFiManager.h"
#include "SDHC_Manager.h"
#include "I2C_Manager.h"
#include "FileManager.h"


/*
 * Function that receives parameters and outputs via UART to enable firmware debugging.
 * class: The class that the debug function is been called from
 * function: The function that the debug function is been called from
 * msg: The msg to be printed
 * type code for the debug message (error_code, warning_code, verbose_code)
 */

void debug_StartUp()
{
	debug_Write("AccelMonitor", "-", 0,StartUp_code, "Version: %s\n", Version_Number);
}

void debug_Write(const char* className, const char* function, uint16_t lineNo, int type, char* msg, ...)
{
	char* string;
	va_list args;

	va_start(args, msg);

	if(0 > vasprintf(&string, msg, args)) string = NULL;    //this is for logging, so failed allocation is not fatal
	va_end(args);

	if(type <= debug_Level)
	{
		char * errorTypeArr = calloc(10, sizeof(char));
		switch (type)
		{
		case Error_code:
			sprintf(errorTypeArr,"Error");
			break;

		case Warning_code:
			sprintf(errorTypeArr,"Warning");
			break;

		case Verbose_code:
			sprintf(errorTypeArr,"Verbose");
			break;
		case Info_code:
			sprintf(errorTypeArr,"Info");
			break;
		case StartUp_code:
			sprintf(errorTypeArr,"\nStarting");
			break;
		}


		printf("%s, %s, %s, %d, %s\n", errorTypeArr, basename(className), function, lineNo, string);
		free(errorTypeArr);

	}
	free(string);

}

char *basename(char const *path)
{
	char *s = {0};
	s = strrchr(path, '/');
	if (!s)
	{
		return strdup(path);
	}
	else
	{
		return strdup(s + 1);
	}
	free(s);
}



