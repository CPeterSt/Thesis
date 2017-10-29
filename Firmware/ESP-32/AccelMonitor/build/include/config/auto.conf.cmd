deps_config := \
	/home/Cundell/esp-idf/components/app_trace/Kconfig \
	/home/Cundell/esp-idf/components/aws_iot/Kconfig \
	/home/Cundell/esp-idf/components/bt/Kconfig \
	/home/Cundell/esp-idf/components/esp32/Kconfig \
	/home/Cundell/esp-idf/components/ethernet/Kconfig \
	/home/Cundell/esp-idf/components/fatfs/Kconfig \
	/home/Cundell/esp-idf/components/freertos/Kconfig \
	/home/Cundell/esp-idf/components/heap/Kconfig \
	/home/Cundell/esp-idf/components/log/Kconfig \
	/home/Cundell/esp-idf/components/lwip/Kconfig \
	/home/Cundell/esp-idf/components/mbedtls/Kconfig \
	/home/Cundell/esp-idf/components/openssl/Kconfig \
	/home/Cundell/esp-idf/components/pthread/Kconfig \
	/home/Cundell/esp-idf/components/spi_flash/Kconfig \
	/home/Cundell/esp-idf/components/spiffs/Kconfig \
	/home/Cundell/esp-idf/components/tcpip_adapter/Kconfig \
	/home/Cundell/esp-idf/components/wear_levelling/Kconfig \
	/home/Cundell/esp-idf/components/bootloader/Kconfig.projbuild \
	/home/Cundell/esp-idf/components/esptool_py/Kconfig.projbuild \
	/c/Users/Cundell/Source/Thesis_2017/Firmware/ESP-32/AccelMonitor/main/Kconfig.projbuild \
	/home/Cundell/esp-idf/components/partition_table/Kconfig.projbuild \
	/home/Cundell/esp-idf/Kconfig

include/config/auto.conf: \
	$(deps_config)


$(deps_config): ;
